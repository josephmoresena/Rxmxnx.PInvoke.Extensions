param (
    [Parameter(Mandatory=$true)]
    [string]$ZipName,

    [Parameter(Mandatory=$true)]
    [string]$Patterns # "*A*,*B*"
)

$patternList = $Patterns -split "," | ForEach-Object { $_.Trim() }
$filesToZip = Get-ChildItem -Path . -File | Where-Object { 
    $fileName = $_.Name
    ($patternList | Where-Object { $fileName -like $_ }) 
}

if ($filesToZip.Count -gt 0) {
    Compress-Archive -Path $filesToZip.FullName -DestinationPath $zipName -Update -CompressionLevel Optimal -Verbose
    if (Test-Path $zipName) {
        $filesToZip | Remove-Item -Force -Verbose
    } else {
        exit 1
    }
}