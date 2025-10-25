# Deploy da EmpresaX.POS.API no Azure App Service

Este guia usa o Azure CLI para provisionar App Service (Linux, .NET 8), PostgreSQL Flexible Server e publicar a API via pacote ZIP.

## Pré-requisitos
- Azure CLI instalado e logado (`az login`)
- PowerShell 7+
- Projeto publicado em `src/publish.zip` (já gerado pelo script de build/publish)

## Passos rápidos

1. Ajuste variáveis (opcional) e execute o script:

```pwsh
cd ./deploy
# Opcional: exporte segredos como variáveis de ambiente para evitar prompt
$env:POSTGRES_ADMIN_PASSWORD = "<sua-senha-forte>"
$env:JWT_KEY = "<seu-jwt-key>"

# Execute com nomes padrão ou personalize
./azure-deploy.ps1 \
  -ResourceGroup empresax-pos-rg \
  -Location brazilsouth \
  -AppServicePlan empresax-pos-plan \
  -WebAppName empresax-pos-api-12345 \
  -PostgresServer empresax-pos-pg-12345 \
  -PostgresDb empresaxposdb \
  -CorsOrigins "https://seu-frontend.com,http://localhost:3000"
```

2. Ao final, o script imprime a URL da API. Teste:

```pwsh
# Healthcheck
irm https://<WebAppName>.azurewebsites.net/health
# Swagger (se habilitado em Production)
start https://<WebAppName>.azurewebsites.net/swagger
```

## App Settings principais
- ConnectionStrings:DefaultConnection → Connection string Npgsql (PostgreSQL)
- Jwt:Key → segredo JWT para autenticação
- Cors:Origins → lista de origens (separe por vírgula)
- ASPNETCORE_ENVIRONMENT=Production
- ASPNETCORE_URLS=http://0.0.0.0:8080

## Observações
- O serviço de Contas atualmente usa um stub em memória. Em produção, substitua por uma implementação persistente (EF Core) ou um serviço real.
- Caso você use migrações do EF, inclua um processo de auto-migrate no startup da aplicação ou crie um script de migração separado.
- Para CI/CD, considere GitHub Actions ou Azure Pipelines.
