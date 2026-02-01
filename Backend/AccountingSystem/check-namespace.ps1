# Check exact namespace in files

$entitiesPath = "src\AccountingSystem.Domain\Entities"

Write-Host "Checking namespaces..." -ForegroundColor Yellow
Write-Host ""

$files = @("Tasktype.cs", "Worker.cs", "CompanyTask.cs")

foreach ($fileName in $files) {
    $path = Join-Path $entitiesPath $fileName
    
    if (Test-Path $path) {
        $content = Get-Content $path -Raw
        
        Write-Host "$fileName" -ForegroundColor Cyan
        
        # Check namespace
        if ($content -match "namespace\s+([\w\.]+)") {
            $namespace = $matches[1]
            Write-Host "  Namespace: $namespace" -ForegroundColor White
        }
        else {
            Write-Host "  Namespace: NOT FOUND!" -ForegroundColor Red
        }
        
        # Check using statements
        $usings = [regex]::Matches($content, "using\s+([\w\.]+);")
        if ($usings.Count -gt 0) {
            Write-Host "  Using statements:" -ForegroundColor White
            foreach ($using in $usings) {
                Write-Host "    - $($using.Groups[1].Value)" -ForegroundColor Gray
            }
        }
        else {
            Write-Host "  Using statements: NONE" -ForegroundColor Yellow
        }
        
        Write-Host ""
    }
}
