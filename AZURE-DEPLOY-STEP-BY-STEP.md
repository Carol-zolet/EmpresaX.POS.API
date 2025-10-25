# 🚀 Guia Passo a Passo - Deploy no Azure

## ⚠️ IMPORTANTE ANTES DE COMEÇAR

**Custos do Azure:**
- ✅ **App Service F1:** Grátis **PARA SEMPRE**
- ⏰ **PostgreSQL B1ms:** Grátis por **12 MESES**, depois ~R$ 50-100/mês
- ✅ **$200 créditos:** Primeiros 30 dias (conta nova)
- ⏰ Após 12 meses, o PostgreSQL começará a cobrar!

**Alternativas Gratuitas Permanentes:**
- Railway.app (recomendado) - Ver [DEPLOY-GUIDE.md](./DEPLOY-GUIDE.md)
- Render.com - Ver [DEPLOY-GUIDE.md](./DEPLOY-GUIDE.md)

---

## 📋 Pré-requisitos

### 1. Conta Microsoft/Azure
- [ ] Criar conta em [portal.azure.com](https://portal.azure.com)
- [ ] Verificar email e telefone
- [ ] Adicionar cartão de crédito (não será cobrado nos limites grátis)
- [ ] Confirmar $200 créditos (conta nova)

### 2. Ferramentas Necessárias
- [ ] Azure CLI instalado
- [ ] .NET 8 SDK instalado
- [ ] Git configurado
- [ ] Projeto rodando localmente

---

## 🔧 Passo 1: Instalar Azure CLI

### Windows (PowerShell como Administrador):

```powershell
# Opção 1: Via Winget (Recomendado)
winget install Microsoft.AzureCLI

# Opção 2: Via MSI Installer
# Baixar de: https://aka.ms/installazurecliwindows
# Executar o instalador
```

### Verificar Instalação:

```powershell
# Fechar e reabrir PowerShell, depois:
az --version
```

---

## 🔐 Passo 2: Login no Azure

```powershell
# 1. Fazer login (abrirá navegador)
az login

# 2. Verificar assinatura ativa
az account list --output table

# 3. Definir assinatura padrão (se tiver mais de uma)
az account set --subscription "SUBSCRIPTION_ID"
```

---

## 🗂️ Passo 3: Criar Resource Group

```powershell
# Navegar até o diretório do projeto
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API"

# Criar resource group no Brasil (Brazil South = São Paulo)
az group create `
  --name empresax-pos-rg `
  --location brazilsouth

# Verificar criação
az group show --name empresax-pos-rg --output table
```

---

## 🌐 Passo 4: Criar App Service Plan (Grátis)

```powershell
# Criar plano F1 (Free - Grátis para sempre!)
az appservice plan create `
  --name empresax-pos-plan `
  --resource-group empresax-pos-rg `
  --sku F1 `
  --is-linux

# Verificar criação
az appservice plan show `
  --name empresax-pos-plan `
  --resource-group empresax-pos-rg `
  --output table
```

---

## 🖥️ Passo 5: Criar Web App (Backend API)

```powershell
# Criar Web App para .NET 8
az webapp create `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --plan empresax-pos-plan `
  --runtime "DOTNETCORE:8.0"

# Verificar criação
az webapp show `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --output table

# Sua API estará em: https://empresax-pos-api.azurewebsites.net
```

---

## 🗄️ Passo 6: Criar PostgreSQL Database (12 meses grátis)

### ⚠️ ATENÇÃO: PostgreSQL cobra após 12 meses!

```powershell
# Criar PostgreSQL Flexible Server (B1ms - 12 meses grátis)
az postgres flexible-server create `
  --name empresax-pos-db `
  --resource-group empresax-pos-rg `
  --location brazilsouth `
  --admin-user adminempresax `
  --admin-password "SenhaForte123!@#" `
  --sku-name Standard_B1ms `
  --tier Burstable `
  --storage-size 32 `
  --version 14 `
  --public-access 0.0.0.0-255.255.255.255

# ⚠️ TROCAR a senha acima por uma senha forte!
```

### Criar Database:

```powershell
# Criar database dentro do servidor
az postgres flexible-server db create `
  --resource-group empresax-pos-rg `
  --server-name empresax-pos-db `
  --database-name empresax_pos
```

### Obter Connection String:

```powershell
# Mostrar detalhes do servidor
az postgres flexible-server show `
  --name empresax-pos-db `
  --resource-group empresax-pos-rg `
  --output table

# Connection string será:
# Host=empresax-pos-db.postgres.database.azure.com;
# Port=5432;
# Database=empresax_pos;
# Username=adminempresax;
# Password=SenhaForte123!@#;
# SSL Mode=Require;
```

---

## ⚙️ Passo 7: Configurar Variáveis de Ambiente

```powershell
# Gerar JWT Secret (chave forte)
$jwtSecret = -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | ForEach-Object {[char]$_})
Write-Host "JWT Secret gerado: $jwtSecret"

# Configurar variáveis de ambiente
az webapp config appsettings set `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --settings `
    ASPNETCORE_ENVIRONMENT=Production `
    DATABASE_URL="Host=empresax-pos-db.postgres.database.azure.com;Port=5432;Database=empresax_pos;Username=adminempresax;Password=SenhaForte123!@#;SSL Mode=Require" `
    JWT_SECRET="$jwtSecret" `
    JWT_ISSUER="EmpresaX.POS" `
    JWT_AUDIENCE="EmpresaX.POS.Users" `
    CORS_ORIGINS="https://empresax-pos-frontend.azurewebsites.net"

# ⚠️ TROCAR a senha na DATABASE_URL pela senha que você definiu!
```

---

## 📦 Passo 8: Deploy do Backend

### Opção 1: Deploy Local (Recomendado para primeira vez)

```powershell
# 1. Navegar até o projeto da API
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\src"

# 2. Publicar aplicação
dotnet publish -c Release -o ./publish

# 3. Criar arquivo ZIP
Compress-Archive -Path ./publish/* -DestinationPath ./publish.zip -Force

# 4. Fazer deploy do ZIP
az webapp deployment source config-zip `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --src ./publish.zip

# 5. Verificar logs
az webapp log tail `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg
```

### Opção 2: Deploy via GitHub Actions (Automático)

```powershell
# Obter credenciais de publicação
az webapp deployment list-publishing-profiles `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --xml

# Copiar o XML retornado e adicionar como secret no GitHub:
# GitHub Repo → Settings → Secrets → New secret
# Nome: AZURE_WEBAPP_PUBLISH_PROFILE
# Valor: Cole o XML
```

Depois, o arquivo `.github/workflows/ci-cd.yml` já existente fará o deploy automaticamente!

---

## 🎨 Passo 9: Deploy do Frontend (Static Web App)

### Opção 1: Via Azure Static Web Apps (Grátis para sempre)

```powershell
# 1. Criar Static Web App (conectar ao GitHub)
az staticwebapp create `
  --name empresax-pos-frontend `
  --resource-group empresax-pos-rg `
  --source https://github.com/SEU-USUARIO/SEU-REPO `
  --location "Brazil South" `
  --branch main `
  --app-location "/empresax-frontend" `
  --output-location "dist" `
  --login-with-github

# Isso criará um GitHub Action automaticamente!
```

### Opção 2: Via App Service (alternativa)

```powershell
# Criar Web App para frontend
az webapp create `
  --name empresax-pos-frontend `
  --resource-group empresax-pos-rg `
  --plan empresax-pos-plan `
  --runtime "NODE:18-lts"

# Configurar variável de ambiente
az webapp config appsettings set `
  --name empresax-pos-frontend `
  --resource-group empresax-pos-rg `
  --settings `
    VITE_API_URL="https://empresax-pos-api.azurewebsites.net/api/v1"

# Deploy do frontend
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\empresax-frontend"
npm install
npm run build
Compress-Archive -Path ./dist/* -DestinationPath ./dist.zip -Force

az webapp deployment source config-zip `
  --name empresax-pos-frontend `
  --resource-group empresax-pos-rg `
  --src ./dist.zip
```

---

## 🔒 Passo 10: Configurar HTTPS e Segurança

```powershell
# 1. Forçar HTTPS
az webapp update `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --https-only true

# 2. Configurar CORS
az webapp cors add `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --allowed-origins "https://empresax-pos-frontend.azurewebsites.net"

# 3. Configurar Always On (se tiver plano pago)
# Não disponível no F1 Free Tier
```

---

## 🗃️ Passo 11: Executar Migrations

```powershell
# 1. Obter connection string do banco
$connectionString = "Host=empresax-pos-db.postgres.database.azure.com;Port=5432;Database=empresax_pos;Username=adminempresax;Password=SuaSenhaAqui;SSL Mode=Require"

# 2. Executar migrations localmente apontando para Azure
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\src"

$env:ConnectionStrings__DefaultConnection = $connectionString
dotnet ef database update

# ⚠️ TROCAR "SuaSenhaAqui" pela senha real do banco!
```

---

## ✅ Passo 12: Testar Deploy

```powershell
# 1. Testar health check
Invoke-WebRequest -Uri "https://empresax-pos-api.azurewebsites.net/health"

# 2. Testar API
Invoke-WebRequest -Uri "https://empresax-pos-api.azurewebsites.net/api/v1/contas"

# 3. Testar Swagger
Start-Process "https://empresax-pos-api.azurewebsites.net/swagger"

# 4. Testar Frontend
Start-Process "https://empresax-pos-frontend.azurewebsites.net"
```

---

## 📊 Passo 13: Configurar Monitoramento (Opcional)

```powershell
# 1. Criar Application Insights (Grátis até 5GB/mês)
az monitor app-insights component create `
  --app empresax-pos-insights `
  --location brazilsouth `
  --resource-group empresax-pos-rg `
  --application-type web

# 2. Obter Instrumentation Key
$instrumentationKey = az monitor app-insights component show `
  --app empresax-pos-insights `
  --resource-group empresax-pos-rg `
  --query instrumentationKey `
  --output tsv

# 3. Configurar no Web App
az webapp config appsettings set `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --settings `
    APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=$instrumentationKey"

# 4. Ver logs em tempo real
az webapp log tail `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg
```

---

## 🔍 Troubleshooting Comum

### Problema 0: "A assinatura não está registrada para usar o namespace 'Microsoft.OperationalInsights'"

Esse erro acontece quando os resource providers de monitoramento não estão registrados na sua assinatura. Registre e tente novamente:

```powershell
# Registrar providers necessários
az provider register --namespace Microsoft.OperationalInsights
az provider register --namespace Microsoft.Insights

# Verificar até ficar Registered
az provider show --namespace Microsoft.OperationalInsights --query "registrationState"
az provider show --namespace Microsoft.Insights --query "registrationState"
```

Pelo Portal (visual): https://portal.azure.com/#view/Microsoft_Azure_Resources/ProvidersBlade → selecione sua assinatura → procure por `Microsoft.OperationalInsights` e `Microsoft.Insights` → clique em Register. Aguarde 1-3 minutos e tente criar o recurso novamente. Veja também o guia `AZURE-FIX-RESOURCE-PROVIDERS.md`.

### Problema 1: "Site não carrega (HTTP 500)"

```powershell
# Ver logs de erro
az webapp log tail `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg

# Ver configurações
az webapp config show `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg
```

### Problema 2: "Connection to database failed"

```powershell
# Verificar firewall do PostgreSQL
az postgres flexible-server firewall-rule list `
  --name empresax-pos-db `
  --resource-group empresax-pos-rg

# Permitir IP específico
az postgres flexible-server firewall-rule create `
  --name empresax-pos-db `
  --resource-group empresax-pos-rg `
  --rule-name AllowMyIP `
  --start-ip-address SEU_IP `
  --end-ip-address SEU_IP
```

### Problema 3: "CORS error"

```powershell
# Adicionar origem CORS
az webapp cors add `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --allowed-origins "https://seu-frontend.com"

# Listar CORS configurados
az webapp cors show `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg
```

### Problema 4: "App muito lento"

**Solução:** Free Tier (F1) tem cold starts. Considerar upgrade:

```powershell
# Upgrade para B1 (~R$ 40/mês - sem cold starts)
az appservice plan update `
  --name empresax-pos-plan `
  --resource-group empresax-pos-rg `
  --sku B1
```

---

## 💰 Passo 14: Monitorar Custos

```powershell
# Ver consumo atual
az consumption usage list `
  --start-date (Get-Date).AddMonths(-1).ToString('yyyy-MM-dd') `
  --end-date (Get-Date).ToString('yyyy-MM-dd') `
  --output table

# Configurar alerta de custo (exemplo: R$ 100)
az consumption budget create `
  --budget-name empresax-budget `
  --amount 100 `
  --time-grain Monthly `
  --start-date (Get-Date).ToString('yyyy-MM-01') `
  --end-date (Get-Date).AddYears(1).ToString('yyyy-MM-01')
```

---

## 📱 Passo 15: Configurar Custom Domain (Opcional)

```powershell
# 1. Adicionar domínio customizado
az webapp config hostname add `
  --webapp-name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --hostname "api.empresax.com.br"

# 2. Configurar SSL (Let's Encrypt grátis)
az webapp config ssl bind `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --certificate-thumbprint AUTO `
  --ssl-type SNI
```

---

## 🗑️ Limpar Recursos (Se quiser deletar tudo)

```powershell
# ⚠️ CUIDADO: Isso deleta TUDO!
az group delete --name empresax-pos-rg --yes --no-wait
```

---

## 📋 Checklist Final

### Antes de considerar "Deploy Completo":
- [ ] Backend respondendo em `/health`
- [ ] Swagger acessível em `/swagger`
- [ ] Frontend carregando
- [ ] Database conectado (testar endpoints)
- [ ] CORS configurado corretamente
- [ ] HTTPS funcionando
- [ ] Migrations executadas
- [ ] Variáveis de ambiente configuradas
- [ ] Logs funcionando
- [ ] Monitoramento (Application Insights) configurado

### URLs Finais:
```
Backend API:    https://empresax-pos-api.azurewebsites.net
Swagger:        https://empresax-pos-api.azurewebsites.net/swagger
Frontend:       https://empresax-pos-frontend.azurewebsites.net
Health Check:   https://empresax-pos-api.azurewebsites.net/health
```

---

## 🎯 Próximos Passos

### Após Deploy:
1. Configurar CI/CD automático (GitHub Actions)
2. Configurar backup do banco de dados
3. Configurar alertas de monitoramento
4. Documentar URLs e credenciais
5. Testar todos os endpoints
6. Configurar domínio customizado (se tiver)

### Lembrete Importante:
⚠️ **PostgreSQL começa a cobrar após 12 meses (~R$ 50-100/mês)**

Se quiser evitar custos após 12 meses, considere:
- Migrar para Supabase (grátis permanente)
- Migrar para Railway/Render (grátis permanente)
- Fazer upgrade para plano pago do Azure com orçamento planejado

---

## 📞 Suporte

### Comandos Úteis:
```powershell
# Ver todos os recursos
az resource list --resource-group empresax-pos-rg --output table

# Ver custos estimados
az consumption usage list --output table

# Reiniciar Web App
az webapp restart --name empresax-pos-api --resource-group empresax-pos-rg

# Ver logs em tempo real
az webapp log tail --name empresax-pos-api --resource-group empresax-pos-rg
```

### Links Úteis:
- [Portal Azure](https://portal.azure.com)
- [Documentação Azure](https://docs.microsoft.com/azure)
- [Azure Status](https://status.azure.com)
- [Calculadora de Preços](https://azure.microsoft.com/pricing/calculator)

---

**Elaborado por:** GitHub Copilot  
**Data:** 20 de outubro de 2025  
**Status:** ✅ Guia Completo para Deploy no Azure

**⚠️ Lembre-se:** Azure é grátis por 12 meses, depois cobra! Para grátis permanente, use Railway ou Render.
