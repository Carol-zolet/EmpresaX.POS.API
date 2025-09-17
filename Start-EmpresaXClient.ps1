# Start-EmpresaXClient.ps1 - Início rápido
Write-Host "🚀 EmpresaX.POS API Client" -ForegroundColor Cyan

Import-Module ".\API-Clients\PowerShell\EmpresaXAuth.psm1" -Force

Write-Host "📋 Comandos disponíveis:" -ForegroundColor Yellow
Write-Host "- Start-EmpresaXDemo                           # Demo completo" -ForegroundColor White
Write-Host "- Connect-EmpresaXApi -Login admin -Password admin123  # Conectar" -ForegroundColor White
Write-Host "- Show-EmpresaXStatus                          # Ver status" -ForegroundColor White
Write-Host "- Invoke-EmpresaXApiCall -Endpoint '/endpoint' # Chamar API" -ForegroundColor White

Write-Host "`n💡 Para começar rapidamente, digite:" -ForegroundColor Green
Write-Host "Start-EmpresaXDemo" -ForegroundColor Cyan
