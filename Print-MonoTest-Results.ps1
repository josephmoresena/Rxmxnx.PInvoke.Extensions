$files = @(Get-ChildItem -Path . -Filter "*TestResult.xml")

# Si no hay archivos, salimos silenciosamente (sin output)
if ($files.Count -eq 0) { exit 0 }

foreach ($file in $files) {
    try {
        [xml]$xml = Get-Content $file.FullName
        
        $assemblyName = $xml.DocumentElement.GetAttribute("name")
        $arch = ""
        $envNode = $xml.SelectSingleNode("//environment")
        if ([string]::IsNullOrWhiteSpace($assemblyName)) { $assemblyName = $file.Name }
        if ($envNode -ne $null) { $arch = $envNode.GetAttribute("os-architecture") }
        if ([string]::IsNullOrWhiteSpace($arch)) { $arch = $env:PROCESSOR_ARCHITECTURE }

        Write-Host "`n📦 Test Assembly: $assemblyName ($arch)" -ForegroundColor Magenta
        
        $testCases = $xml.SelectNodes("//test-case")
        
        foreach ($test in $testCases) {
            $name = $test.fullname
            
            # Cálculo de duración
            $durationSec = [double]$test.duration
            $durationMs = $durationSec * 1000
            $timeStr = ""
            
            if ($durationMs -lt 1) {
                $timeStr = "[< 1 ms]"
            } 
            elseif ($durationSec -lt 1) {
                $msInt = [int][math]::Round($durationMs)
                $timeStr = "[{0} ms]" -f $msInt
            } 
            else {
                $totalSec = [int][math]::Round($durationSec)
                
                if ($totalSec -lt 60) {
                    $timeStr = "[{0} s]" -f $totalSec
                } 
                elseif ($totalSec -lt 3600) {
                    $m = [math]::Floor($totalSec / 60)
                    $s = $totalSec % 60
                    $timeStr = "[{0} m {1} s]" -f $m, $s
                } 
                else {
                    $h = [math]::Floor($totalSec / 3600)
                    $rem = $totalSec % 3600
                    $m = [math]::Floor($rem / 60)
                    $s = $rem % 60
                    $timeStr = "[{0} h {1} m {2} s]" -f $h, $m, $s
                }
            }
            
            if ($test.result -eq "Passed") {
                Write-Host "  ✅ $name $timeStr" -ForegroundColor Green
            } elseif ($test.result -eq "Failed") {
                Write-Host "  ❌ $name $timeStr" -ForegroundColor Red
            } else {
                Write-Host "  ⚠️ $name $timeStr" -ForegroundColor Yellow
            }
        }
    } catch {
        # Nothing
    } finally {
        if (Test-Path $file.FullName) {
            Remove-Item -Path $file.FullName -Force
        }
    }
}