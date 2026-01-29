# Simple class name checker - No Hebrew

$entitiesPath = "src\AccountingSystem.Domain\Entities"

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Checking class names..." -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

$files = @("Tasktype.cs", "Worker.cs", "CompanyTask.cs", "Companyworker.cs", "Role.cs")

foreach ($file in $files) {
    $path = Join-Path $entitiesPath $file
    
    if (Test-Path $path) {
        $content = Get-Content $path -Raw
        
        if ($content -match "public\s+partial\s+class\s+(\w+)") {
            $className = $matches[1]
            $fileBaseName = [System.IO.Path]::GetFileNameWithoutExtension($file)
            
            Write-Host "$file" -ForegroundColor Yellow
            Write-Host "  File name: $fileBaseName" -ForegroundColor White
            Write-Host "  Class name: $className" -ForegroundColor White
            
            if ($className -eq $fileBaseName) {
                Write-Host "  Status: OK" -ForegroundColor Green
            }
            else {
                Write-Host "  Status: MISMATCH!" -ForegroundColor Red
            }
        }
    }
    else {
        Write-Host "$file - NOT FOUND" -ForegroundColor Red
    }
    Write-Host ""
}

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Navigation Properties:" -ForegroundColor Cyan  
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Check CompanyTask.cs
$companyTaskPath = Join-Path $entitiesPath "CompanyTask.cs"
if (Test-Path $companyTaskPath) {
    Write-Host "CompanyTask.cs navigation properties:" -ForegroundColor Yellow
    $lines = Get-Content $companyTaskPath
    $lines | Select-String "public virtual" | ForEach-Object {
        Write-Host "  $_" -ForegroundColor White
    }
}
Write-Host ""

# Check Companyworker.cs
$companyWorkerPath = Join-Path $entitiesPath "Companyworker.cs"
if (Test-Path $companyWorkerPath) {
    Write-Host "Companyworker.cs navigation properties:" -ForegroundColor Yellow
    $lines = Get-Content $companyWorkerPath
    $lines | Select-String "public virtual" | ForEach-Object {
        Write-Host "  $_" -ForegroundColor White
    }
}
Write-Host ""

# Check Role.cs
$rolePath = Join-Path $entitiesPath "Role.cs"
if (Test-Path $rolePath) {
    Write-Host "Role.cs navigation properties:" -ForegroundColor Yellow
    $lines = Get-Content $rolePath
    $lines | Select-String "public virtual" | ForEach-Object {
        Write-Host "  $_" -ForegroundColor White
    }
}