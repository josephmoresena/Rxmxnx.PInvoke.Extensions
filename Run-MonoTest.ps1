param(
    [Parameter(Mandatory = $true)]
    [string]$MonoPath
)

$testProjects = [ordered]@{
    "Rxmxnx.PInvoke.Common.Tests" = "Common"
    "Rxmxnx.PInvoke.CString.Tests" = "CString"
    "Rxmxnx.PInvoke.Extensions.Tests" = "Extensions"
    "Rxmxnx.PInvoke.Buffers.Tests" = "Buffers"
}

$executionFailed = $false

foreach ($projName in $testProjects.Keys)
{
    $shortName = $testProjects[$projName]
    $searchPattern = "src/Test/$projName/bin/*/netstandard2.1"
    $foundPaths = @(Resolve-Path $searchPattern -ErrorAction SilentlyContinue)

    if ($foundPaths.Count -eq 0)
    {
        Write-Error "❌ Build output not found for project: $projName"
        Write-Error "   Expected pattern: $searchPattern"
        $executionFailed = $true
        continue
    }

    $binPath = $foundPaths[0].Path
    foreach ($p in $foundPaths)
    {
        if ($p.Path -match "Release")
        {
            $binPath = $p.Path
            break
        }
    }
    $runnerExe = "$binPath/nunitlite-runner.exe"
    $dllPath = "$binPath/$projName.dll"

    $resultFile = "$shortName.TestResult.xml"

    Write-Host "`n🚀 Running: $shortName ($resultFile)..." -ForegroundColor Cyan
    Write-Host "   Path: $binPath" -ForegroundColor DarkGray

    & $MonoPath $runnerExe $dllPath "-labels=All" "--result=$resultFile"

    if ($LASTEXITCODE -ne 0)
    {
        Write-Error "❌ Tests failed in: $shortName"
        $executionFailed = $true
    }
}

if ($executionFailed)
{
    exit 1
}