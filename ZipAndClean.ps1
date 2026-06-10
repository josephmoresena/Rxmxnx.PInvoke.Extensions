param (
    [Parameter(Mandatory = $true)]
    [string]$ZipName,

    [Parameter(Mandatory = $true)]
    [string[]]$Patterns
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-SevenZipCommand {
    $commands = @('7z', '7zz', '7za')
    foreach ($cmd in $commands) {
        $found = Get-Command $cmd -ErrorAction SilentlyContinue
        if ($null -ne $found) {
            return $found.Source
        }
    }
    return $null
}

function Compress-DirectoryWithZipArchive {
    param (
        [Parameter(Mandatory = $true)]
        [string]$SourceDirectory,

        [Parameter(Mandatory = $true)]
        [string]$DestinationZip
    )

    Add-Type -AssemblyName System.IO.Compression
    Add-Type -AssemblyName System.IO.Compression.FileSystem

    if (Test-Path -LiteralPath $DestinationZip) {
        Remove-Item -LiteralPath $DestinationZip -Force
    }

    [System.IO.Compression.ZipFile]::CreateFromDirectory(
        $SourceDirectory,
        $DestinationZip,
        [System.IO.Compression.CompressionLevel]::Optimal,
        $false
    )
}

$resolvedZipName = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($ZipName)
$zipDirectory = Split-Path -Parent $resolvedZipName

if ([string]::IsNullOrWhiteSpace($zipDirectory)) {
    $zipDirectory = (Get-Location).ProviderPath
}

if (-not (Test-Path -LiteralPath $zipDirectory)) {
    New-Item -ItemType Directory -Path $zipDirectory -Force | Out-Null
}

$tempRoot = if ($env:RUNNER_TEMP) { $env:RUNNER_TEMP } else { [System.IO.Path]::GetTempPath() }
$stagingDirectory = Join-Path $tempRoot ("zip-stage-{0}" -f [System.Guid]::NewGuid().ToString('N'))
$tempZipName = Join-Path $tempRoot ("zip-output-{0}.zip" -f [System.Guid]::NewGuid().ToString('N'))

New-Item -ItemType Directory -Path $stagingDirectory -Force | Out-Null

try {
    $filesToZip = foreach ($pat in $Patterns) {
        Get-ChildItem -Path $pat -File -ErrorAction SilentlyContinue
    }

    $filesToZip = @($filesToZip | Sort-Object -Property FullName -Unique)

    if ($filesToZip.Count -eq 0) {
        Write-Host 'No files matched the provided patterns.'
        return
    }

    foreach ($file in $filesToZip) {
        $destination = Join-Path $stagingDirectory $file.Name

        if (Test-Path -LiteralPath $destination) {
            Remove-Item -LiteralPath $destination -Force
        }

        Move-Item -LiteralPath $file.FullName -Destination $destination -Force -Verbose
    }

    $sevenZip = Get-SevenZipCommand

    if ($null -ne $sevenZip -and ($IsWindows -or $IsLinux -or $IsMacOS)) {
        if (Test-Path -LiteralPath $tempZipName) {
            Remove-Item -LiteralPath $tempZipName -Force
        }

        Push-Location $stagingDirectory
        try {
            & $sevenZip a -tzip -mx=9 -mmt=on $tempZipName '*' | Write-Host
            if ($LASTEXITCODE -ne 0) {
                throw "7-Zip failed with exit code $LASTEXITCODE."
            }
        }
        finally {
            Pop-Location
        }
    }
    else {
        Compress-DirectoryWithZipArchive -SourceDirectory $stagingDirectory -DestinationZip $tempZipName
    }

    if (-not (Test-Path -LiteralPath $tempZipName)) {
        throw "Zip file was not created: $tempZipName"
    }

    if ((Get-Item -LiteralPath $tempZipName).Length -le 0) {
        throw "Zip file is empty: $tempZipName"
    }

    if (Test-Path -LiteralPath $resolvedZipName) {
        Remove-Item -LiteralPath $resolvedZipName -Force
    }

    Move-Item -LiteralPath $tempZipName -Destination $resolvedZipName -Force
    Remove-Item -LiteralPath $stagingDirectory -Recurse -Force -Verbose

    Write-Host "Created zip: $resolvedZipName"
}
catch {
    Write-Error $_
    throw
}
finally {
    if (Test-Path -LiteralPath $tempZipName) {
        Remove-Item -LiteralPath $tempZipName -Force -ErrorAction SilentlyContinue
    }

    if (Test-Path -LiteralPath $stagingDirectory) {
        Remove-Item -LiteralPath $stagingDirectory -Recurse -Force -ErrorAction SilentlyContinue
    }
}
