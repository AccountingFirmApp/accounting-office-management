# Add global using to all Entity files

$entitiesPath = "src\AccountingSystem.Domain\Entities"

Write-Host "Adding global using statements..." -ForegroundColor Yellow
Write-Host ""

$files = Get-ChildItem -Path $entitiesPath -Filter "*.cs" -File

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    
    # Check if already has the using
    if ($content -notmatch "global using AccountingSystem\.Domain\.Entities;") {
        
        # Check if file starts with using statements
        if ($content -match "^(using .*?\n)*") {
            # Add at the beginning
            $newContent = "global using AccountingSystem.Domain.Entities;`n" + $content
        }
        else {
            # No usings at all - add before namespace
            $newContent = "global using AccountingSystem.Domain.Entities;`n`n" + $content
        }
        
        Set-Content -Path $file.FullName -Value $newContent -NoNewline
        Write-Host "  Fixed: $($file.Name)" -ForegroundColor Green
    }
    else {
        Write-Host "  Skip: $($file.Name) (already has using)" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "Done! Now run: dotnet build" -ForegroundColor Green