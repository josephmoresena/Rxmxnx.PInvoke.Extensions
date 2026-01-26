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
    Compress-Archive -Path $filesToZip.FullName -DestinationPath $ZipName -Update -CompressionLevel Optimal -Verbose

    if (Test-Path $ZipName)
    {
        $filesToZip | Remove-Item -Force -Verbose
    }
}