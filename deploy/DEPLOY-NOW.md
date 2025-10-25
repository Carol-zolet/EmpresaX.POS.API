# Deploy Rápido para Azure

## Preparação completa ✅
- Build Release: OK
- Testes: 106/106 PASS
- Pacote ZIP: `src/publish.zip`
- Script: `deploy/azure-deploy.ps1` (corrigido para DOTNETCORE:8.0)
- Login Azure: OK

## Execute agora (copie e cole no PowerShell)

```pwsh
# 1. Ajuste o PATH para encontrar az
$env:Path = 'C:\Program Files\Microsoft SDKs\Azure\CLI2\wbin;' + $env:Path

# 2. Navegue até a pasta de deploy
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\deploy"

# 3. Defina nome único para PostgreSQL
$pgName = "empresax-pg-$(Get-Random -Min 10000 -Max 99999)"

# 4. Execute o deploy (será prompt para senha Postgres e JWT Key)
./azure-deploy.ps1 `
  -ResourceGroup empresax-pos-rg-v2 `
  -Location brazilsouth `
  -AppServicePlan empresax-pos-plan-v2 `
  -WebAppName empresax-pos-api-carol `
  -PostgresServer $pgName `
  -PostgresDb empresaxposdb `
  -CorsOrigins "http://localhost:3000,https://empresax-pos-api-carol.azurewebsites.net"
```

## Durante a execução
- **Senha PostgreSQL**: Escolha uma senha forte (mín. 8 caracteres, com maiúsculas, minúsculas, números e símbolos)
  - Exemplo: `PgSecure2025!Carol`
- **JWT Key**: Escolha uma chave secreta longa
  - Exemplo: `JwtSecureKey2025TokenCarol!!`

## O que o script faz (demora ~5-10 minutos)
1. Cria Resource Group `empresax-pos-rg-v2`
2. Cria App Service Plan (Linux S1)
3. Cria Web App com .NET 8 (`empresax-pos-api-carol`)
4. Cria PostgreSQL Flexible Server (nome gerado aleatoriamente)
5. Cria banco `empresaxposdb`
6. Configura firewall PostgreSQL
7. Define App Settings (ConnectionStrings, JWT, CORS, environment)
8. Faz deploy do ZIP
9. Tenta validar GET /health

## Após conclusão
Você verá no terminal:
```
Deploy concluído.
URL da API: https://empresax-pos-api-carol.azurewebsites.net
```

Valide:
```pwsh
# Health check
Invoke-WebRequest -Uri "https://empresax-pos-api-carol.azurewebsites.net/health" -UseBasicParsing

# Swagger (se habilitado em production)
start "https://empresax-pos-api-carol.azurewebsites.net/swagger"
```

## Se houver erro
- **"ResourceGroupBeingDeleted"**: Aguarde 1-2 minutos e tente novamente
- **"Server name already used"**: O script já gera nome aleatório; se persistir, mude `$pgName` manualmente
- **Runtime not supported**: Já corrigido para `DOTNETCORE:8.0`

## Limpar recursos (quando quiser)
```pwsh
az group delete -n empresax-pos-rg-v2 --yes
```
