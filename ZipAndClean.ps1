param (
    [Parameter(Mandatory = $true)]
    [string]$ZipName,

    [Parameter(Mandatory = $true)]
    [string[]]$Patterns
)
$filesToZip = @()
foreach ($pat in $Patterns)
{
    $found = Get-ChildItem -Path $pat -File -ErrorAction SilentlyContinue
    if ($found)
    {
        $filesToZip += $found
    }
}
$filesToZip = $filesToZip | Select-Object -Unique
if ($filesToZip.Count -gt 0)
{
    Add-Type -AssemblyName System.IO.Compression
    Add-Type -AssemblyName System.IO.Compression.FileSystem

    if (Test-Path $ZipName)
    {
        Remove-Item -LiteralPath $ZipName -Force
    }

    $zip = [System.IO.Compression.ZipFile]::Open(
        $ZipName,
        [System.IO.Compression.ZipArchiveMode]::Create
    )

    try
    {
        foreach ($file in $filesToZip)
        {
            [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile(
                $zip,
                $file.FullName,
                $file.Name,
                [System.IO.Compression.CompressionLevel]::Optimal
            ) | Out-Null

            Remove-Item -LiteralPath $file.FullName -Force -Verbose
        }
    }
    finally
    {
        $zip.Dispose()
    }
}
