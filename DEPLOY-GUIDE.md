# 🚀 Guia de Deploy Gratuito - EmpresaX.POS

## ⚠️ IMPORTANTE: Sobre o Azure

**Azure NÃO é grátis permanentemente!**
- ✅ App Service F1: Grátis **para sempre**
- ⏰ PostgreSQL: Grátis apenas **12 meses**, depois cobra ~R$ 50-100/mês
- ⏰ Período gratuito: Apenas para **novos usuários**

**Para deploy gratuito PERMANENTE, use Railway ou Render!**

---

## 📋 Visão Geral

Este guia apresenta **opções de deploy gratuito** para o sistema **EmpresaX.POS** (backend .NET + frontend React):

### ✅ Opções Gratuitas PERMANENTES:
1. **Railway.app** - $5 crédito/mês (suficiente para pequenos projetos)
2. **Render.com** - 100% grátis (com cold starts)

### ⚠️ Opção Temporária (12 meses):
3. **Azure** - Grátis 12 meses, depois cobra

**Incluído no deploy:**
- ✅ Backend API (.NET 8)
- ✅ Frontend React
- ✅ Banco de dados PostgreSQL
- ✅ CI/CD automático
- ✅ Monitoramento básico

**Custo Total Permanente:** R$ 0,00/mês 💰 (Railway ou Render)

---

## 🎯 Opções de Deploy Gratuito

### Opção 1: Railway.app (Recomendado) 🌟

**Por que escolher:**
- ✅ Deploy automático via GitHub
- ✅ PostgreSQL incluso
- ✅ HTTPS automático
- ✅ $5/mês de créditos grátis (suficiente para pequenos projetos)
- ✅ Logs em tempo real

**Limitações:**
- 500 horas/mês (mais que suficiente)
- Após créditos, serviço dorme após inatividade

---

### Opção 2: Render.com 🆓

**Por que escolher:**
- ✅ Totalmente gratuito
- ✅ PostgreSQL gratuito (90 dias, depois renovável)
- ✅ Deploy automático
- ✅ HTTPS automático

**Limitações:**
- Serviço dorme após 15min de inatividade
- Cold start de ~30 segundos

---

### Opção 3: Azure (12 Meses Grátis para Novos Usuários) ☁️

**Por que escolher:**
- ✅ App Service gratuito (F1) - **sempre grátis**
- ✅ PostgreSQL Flexible Server (B1ms) - **12 meses grátis**
- ✅ 750h/mês de VM B1S - **12 meses grátis**
- ✅ Application Insights incluso
- ✅ Professional grade
- ✅ $200 em créditos nos primeiros 30 dias

**Limitações:**
- ⚠️ **12 meses grátis apenas para novos usuários**
- ⚠️ Após 12 meses, PostgreSQL passa a cobrar (~R$ 50-100/mês)
- Requer cartão de crédito (sem cobrança nos limites grátis)
- Mais complexo de configurar
- App Service F1 continua grátis após 12 meses

---

## 🚀 Deploy no Railway.app (Opção Mais Fácil)

### Pré-requisitos
- Conta no [Railway.app](https://railway.app)
- Repositório no GitHub
- Projeto funcionando localmente

### Passo 1: Preparar o Backend

#### 1.1 Criar `railway.json` na raiz do projeto API

```json
{
  "build": {
    "builder": "NIXPACKS",
    "buildCommand": "dotnet publish -c Release -o out"
  },
  "deploy": {
    "startCommand": "dotnet out/EmpresaX.POS.API.dll",
    "healthcheckPath": "/health",
    "healthcheckTimeout": 100,
    "restartPolicyType": "ON_FAILURE",
    "restartPolicyMaxRetries": 10
  }
}
```

#### 1.2 Atualizar `appsettings.json` para usar variáveis de ambiente

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "${DATABASE_URL}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "Cors": {
    "Origins": "${CORS_ORIGINS:http://localhost:3000}"
  }
}
```

#### 1.3 Modificar `Program.cs` para Railway

```csharp
// Adicionar suporte a porta do Railway
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Configurar PostgreSQL do Railway
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString) && connectionString.Contains("DATABASE_URL"))
{
    // Railway fornece URL no formato postgres://user:pass@host:port/db
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}
```

### Passo 2: Deploy do Backend no Railway

#### 2.1 Criar novo projeto

1. Acesse [railway.app](https://railway.app)
2. Clique em **"New Project"**
3. Selecione **"Deploy from GitHub repo"**
4. Autorize o Railway no GitHub
5. Selecione o repositório do projeto

#### 2.2 Adicionar PostgreSQL

1. No projeto Railway, clique em **"+ New"**
2. Selecione **"Database"** → **"PostgreSQL"**
3. Railway criará automaticamente a variável `DATABASE_URL`

#### 2.3 Configurar Variáveis de Ambiente

No painel do serviço da API, vá em **"Variables"** e adicione:

```bash
# Conexão com banco (automática, já preenchida)
DATABASE_URL=postgresql://...

# CORS (URL do frontend que será criado)
CORS_ORIGINS=https://seu-frontend.up.railway.app

# Ambiente
ASPNETCORE_ENVIRONMENT=Production

# JWT (gerar chave forte)
JWT_SECRET=SuaChaveSuperSecretaAqui123!@#
JWT_ISSUER=EmpresaX.POS
JWT_AUDIENCE=EmpresaX.POS.Users

# Serilog
Serilog__MinimumLevel__Default=Information
```

#### 2.4 Deploy

Railway fará deploy automaticamente! Acompanhe os logs.

Sua API estará disponível em: `https://empresax-api-production.up.railway.app`

### Passo 3: Deploy do Frontend no Railway

#### 3.1 Criar `railway.json` no projeto frontend

```json
{
  "build": {
    "builder": "NIXPACKS",
    "buildCommand": "npm install && npm run build"
  },
  "deploy": {
    "startCommand": "npm run start",
    "healthcheckPath": "/",
    "healthcheckTimeout": 100
  }
}
```

#### 3.2 Atualizar URL da API no frontend

Criar arquivo `.env.production`:

```bash
VITE_API_URL=https://empresax-api-production.up.railway.app/api/v1
```

No código, usar:

```typescript
const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api/v1';
```

#### 3.3 Adicionar serviço frontend

1. No mesmo projeto Railway, clique em **"+ New"**
2. Selecione **"GitHub Repo"**
3. Escolha a pasta do frontend (ou mesmo repo)
4. Configure o **Root Directory** se necessário

#### 3.4 Variáveis de ambiente do frontend

```bash
VITE_API_URL=https://empresax-api-production.up.railway.app/api/v1
NODE_ENV=production
```

### Passo 4: Configurar Custom Domain (Opcional)

1. No serviço, vá em **"Settings"** → **"Domains"**
2. Clique em **"Generate Domain"** para subdomínio gratuito
3. Ou configure seu próprio domínio

---

## 🎨 Deploy no Render.com (100% Gratuito)

### Passo 1: Backend no Render

#### 1.1 Criar Web Service

1. Acesse [render.com](https://render.com)
2. Clique em **"New +"** → **"Web Service"**
3. Conecte seu repositório GitHub
4. Configure:

```yaml
Name: empresax-api
Environment: Docker (ou .NET)
Branch: main
Build Command: dotnet publish -c Release -o out
Start Command: dotnet out/EmpresaX.POS.API.dll
```

#### 1.2 Variáveis de Ambiente

```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
DATABASE_URL=(será preenchido após criar o banco)
CORS_ORIGINS=https://seu-frontend.onrender.com
JWT_SECRET=SuaChaveSecreta123
```

### Passo 2: PostgreSQL no Render

1. Clique em **"New +"** → **"PostgreSQL"**
2. Escolha o plano **Free**
3. Copie a **Internal Database URL**
4. Cole em `DATABASE_URL` no backend

**Importante:** PostgreSQL gratuito expira em 90 dias, mas pode ser renovado.

### Passo 3: Frontend no Render

#### 3.1 Criar Static Site

1. **"New +"** → **"Static Site"**
2. Configure:

```yaml
Name: empresax-frontend
Build Command: npm install && npm run build
Publish Directory: dist (ou build)
```

#### 3.2 Variáveis de Ambiente

```bash
VITE_API_URL=https://empresax-api.onrender.com/api/v1
NODE_ENV=production
```

### Passo 4: Evitar Cold Starts (Opcional)

Use um serviço como [cron-job.org](https://cron-job.org) para fazer ping na API a cada 10 minutos:

```bash
# Criar job para:
GET https://empresax-api.onrender.com/health
# A cada 10 minutos
```

---

## ☁️ Deploy no Azure (12 Meses Grátis)

### ⚠️ Importante sobre o Azure Free Tier

**O que é grátis para sempre:**
- ✅ App Service F1 (frontend/backend)
- ✅ 5 GB de armazenamento Blob
- ✅ Application Insights (5 GB dados/mês)

**O que é grátis por 12 meses (novos usuários):**
- ⏰ PostgreSQL Flexible Server B1ms (750h/mês)
- ⏰ VM B1S (750h/mês)
- ⏰ Após 12 meses, começa a cobrar!

**Recomendação:** Use Azure apenas se você:
1. É novo usuário (tem 12 meses grátis)
2. Planeja migrar ou pagar após 12 meses
3. Quer $200 crédito inicial
4. Precisa compliance/professional grade

**Para longo prazo gratuito, use Railway ou Render!**

### Pré-requisitos
- Conta Microsoft (nova para ter 12 meses grátis)
- Azure CLI instalado
- Cartão de crédito (sem cobrança nos limites grátis)

### Passo 1: Backend no Azure App Service

#### 1.1 Criar Resource Group

```powershell
# Login
az login

# Criar resource group
az group create --name empresax-rg --location brazilsouth

# Criar App Service Plan (Free F1)
az appservice plan create `
  --name empresax-plan `
  --resource-group empresax-rg `
  --sku F1 `
  --is-linux
```

#### 1.2 Criar Web App

```powershell
az webapp create `
  --name empresax-api `
  --resource-group empresax-rg `
  --plan empresax-plan `
  --runtime "DOTNET|8.0"
```

#### 1.3 Configurar Deployment

```powershell
# Configurar deploy do GitHub
az webapp deployment source config `
  --name empresax-api `
  --resource-group empresax-rg `
  --repo-url https://github.com/seu-usuario/seu-repo `
  --branch main `
  --manual-integration
```

#### 1.4 Configurar Variáveis

```powershell
az webapp config appsettings set `
  --name empresax-api `
  --resource-group empresax-rg `
  --settings `
    ASPNETCORE_ENVIRONMENT=Production `
    DATABASE_URL="sua-connection-string" `
    JWT_SECRET="sua-chave"
```

### Passo 2: PostgreSQL no Azure

```powershell
# Criar PostgreSQL Flexible Server (B1ms - 12 meses grátis)
az postgres flexible-server create `
  --name empresax-db `
  --resource-group empresax-rg `
  --location brazilsouth `
  --admin-user adminuser `
  --admin-password "SenhaForte123!" `
  --sku-name Standard_B1ms `
  --tier Burstable `
  --version 14 `
  --storage-size 32

# Permitir acesso do App Service
az postgres flexible-server firewall-rule create `
  --name empresax-db `
  --resource-group empresax-rg `
  --rule-name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0
```

### Passo 3: Frontend no Azure Static Web Apps

```powershell
# Static Web Apps é GRÁTIS para sempre!
az staticwebapp create `
  --name empresax-frontend `
  --resource-group empresax-rg `
  --source https://github.com/seu-usuario/seu-repo `
  --location "Brazil South" `
  --branch main `
  --app-location "/empresax-frontend" `
  --output-location "dist"
```

---

## 🗄️ Opções de Banco de Dados Gratuito

### 1. **Supabase PostgreSQL** (Recomendado)
- ✅ 500 MB storage
- ✅ Sempre online
- ✅ Backup automático
- ✅ Dashboard visual
- 🔗 [supabase.com](https://supabase.com)

**Como usar:**
1. Criar projeto no Supabase
2. Copiar connection string
3. Usar no `appsettings.json`

### 2. **ElephantSQL**
- ✅ 20 MB storage
- ✅ PostgreSQL gerenciado
- 🔗 [elephantsql.com](https://elephantsql.com)

### 3. **Neon.tech**
- ✅ 3 GB storage
- ✅ Serverless PostgreSQL
- ✅ Branching de banco de dados
- 🔗 [neon.tech](https://neon.tech)

---

## 🔒 Configurações de Segurança para Produção

### 1. Configurar HTTPS Only

**Railway/Render:** Automático ✅

**Azure:**
```powershell
az webapp update `
  --name empresax-api `
  --resource-group empresax-rg `
  --https-only true
```

### 2. Configurar CORS Correto

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        var allowedOrigins = builder.Configuration["CORS_ORIGINS"]?.Split(',') 
            ?? new[] { "https://seu-frontend.com" };
            
        policy.WithOrigins(allowedOrigins)
              .AllowedMethods("GET", "POST", "PUT", "DELETE")
              .AllowedHeaders("Content-Type", "Authorization")
              .AllowCredentials();
    });
});

app.UseCors("Production");
```

### 3. Configurar Secrets Seguros

**Nunca commite:**
- ❌ Connection strings
- ❌ JWT secrets
- ❌ API keys

**Use variáveis de ambiente em todos os deploys!**

### 4. Habilitar Rate Limiting

```csharp
// Já documentado no SECURITY-GUIDE.md
builder.Services.AddRateLimiter(/* ... */);
```

---

## 📊 Monitoramento Gratuito

### 1. **UptimeRobot** (Recomendado)
- ✅ 50 monitores gratuitos
- ✅ Alertas por email
- ✅ Status page público
- 🔗 [uptimerobot.com](https://uptimerobot.com)

**Configuração:**
1. Criar conta
2. Adicionar monitor HTTP(S)
3. URL: `https://sua-api.com/health`
4. Intervalo: 5 minutos

### 2. **Better Uptime**
- ✅ Monitoramento de uptime
- ✅ Alertas (email, Slack, SMS)
- ✅ Status page
- 🔗 [betteruptime.com](https://betteruptime.com)

### 3. **Sentry (Errors)**
- ✅ 5.000 eventos/mês grátis
- ✅ Rastreamento de erros
- ✅ Performance monitoring
- 🔗 [sentry.io](https://sentry.io)

**Integração:**
```powershell
dotnet add package Sentry.AspNetCore
```

```csharp
// Program.cs
builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://sua-dsn@sentry.io/projeto";
    options.TracesSampleRate = 1.0;
});
```

---

## 🚦 Checklist de Deploy

### Antes do Deploy
- [ ] Testes passando localmente
- [ ] Build sem erros
- [ ] Migrations prontas
- [ ] Variáveis de ambiente documentadas
- [ ] CORS configurado
- [ ] JWT secrets gerados
- [ ] Health check endpoint funcionando

### Durante o Deploy
- [ ] Connection string configurada
- [ ] HTTPS habilitado
- [ ] Variáveis de ambiente configuradas
- [ ] Migrations executadas
- [ ] Logs funcionando

### Após o Deploy
- [ ] API respondendo (teste `/health`)
- [ ] Frontend carregando
- [ ] Autenticação funcionando
- [ ] Banco de dados conectado
- [ ] Monitoramento configurado
- [ ] Backup configurado

---

## 🆘 Troubleshooting Comum

### Problema: "Connection refused" no PostgreSQL

**Solução:**
```csharp
// Verificar formato da connection string
// Railway/Render usam formato especial:
postgres://user:password@host:5432/database

// Converter para formato .NET:
Host=host;Port=5432;Database=database;Username=user;Password=password;SSL Mode=Require
```

### Problema: CORS errors

**Solução:**
1. Verificar `CORS_ORIGINS` nas variáveis de ambiente
2. Incluir protocolo completo: `https://frontend.com`
3. Sem barra final: ❌ `https://frontend.com/`

### Problema: 502 Bad Gateway

**Solução:**
1. Verificar porta: usar `$PORT` ou `PORT` environment variable
2. Bind em `0.0.0.0`, não `localhost`
3. Verificar logs: `railway logs` ou Render dashboard

### Problema: Cold starts lentos (Render)

**Solução:**
1. Usar cron-job.org para fazer ping a cada 10min
2. Otimizar startup (remover seeds desnecessários)
3. Considerar upgrade para plano pago (~$7/mês)

---

## 💰 Comparação de Custos

| Serviço | Grátis | Limitações | Upgrade | Permanente? |
|---------|--------|------------|---------|-------------|
| **Railway** | $5 crédito/mês | 500h/mês, dorme | $5/mês | ✅ Sim |
| **Render** | ✅ Grátis | Cold starts | $7/mês | ✅ Sim |
| **Azure** | ⏰ 12 meses | Requer cartão, depois cobra | Variável | ❌ Não* |
| **Supabase DB** | ✅ Grátis | 500 MB | $25/mês | ✅ Sim |
| **UptimeRobot** | ✅ Grátis | 50 monitores | $7/mês | ✅ Sim |

**\*Azure App Service F1 é grátis para sempre, mas PostgreSQL cobra após 12 meses (~R$ 50-100/mês)**

### Custo Após 12 Meses

| Opção | Meses 1-12 | Após 12 meses |
|-------|------------|---------------|
| **Railway + Supabase** | R$ 0 | R$ 0 (crédito mensal) |
| **Render + Supabase** | R$ 0 | R$ 0 |
| **Azure** | R$ 0 | R$ 50-100/mês* |

**\*Azure PostgreSQL começa a cobrar após período gratuito**

**Total Recomendado:** **R$ 0,00/mês para sempre** (Railway $5 crédito + Render Frontend + Supabase DB)

---

## 🎯 Recomendação Final

### Para Desenvolvimento/MVP/Produção Pequena (MELHOR OPÇÃO):
✅ **Railway.app** (Backend + PostgreSQL) + **Render** (Frontend)
- ✅ Deploy mais fácil
- ✅ **Grátis para sempre** (Railway $5 crédito mensal)
- ✅ Logs em tempo real
- ✅ Sem cold starts no Railway
- ✅ PostgreSQL incluso

### Para Produção 100% Gratuita (ZERO CUSTO):
✅ **Render.com** (Backend + Frontend) + **Supabase** (PostgreSQL)
- ✅ **100% gratuito para sempre**
- ✅ Supabase com backup automático
- ✅ Sem necessidade de cartão
- ⚠️ Cold starts após 15min inatividade
- **Solução:** Configurar cron-job.org para ping a cada 10min

### Para Produção Profissional com Budget:
✅ **Railway Paid** ($5-20/mês) ou **Render Paid** ($7+/mês)
- ✅ Sem cold starts
- ✅ Melhor performance
- ✅ Suporte profissional
- ✅ Escalabilidade

### ⚠️ Azure (NÃO Recomendado para Grátis Permanente):
❌ **Não use Azure** se quer grátis permanente
- ⚠️ Grátis apenas **12 meses**
- ⚠️ PostgreSQL cobra após período (~R$ 50-100/mês)
- ✅ Use apenas se:
  - Você é novo usuário e quer $200 crédito inicial
  - Planeja pagar após 12 meses
  - Precisa compliance/certificações
  - Empresa já usa Azure

---

## 📚 Recursos Adicionais

### Documentação Oficial
- [Railway Docs](https://docs.railway.app)
- [Render Docs](https://render.com/docs)
- [Azure Docs](https://docs.microsoft.com/azure)
- [Supabase Docs](https://supabase.com/docs)

### Tutoriais em Vídeo
- [Deploy .NET no Railway (YouTube)](https://youtube.com/watch?v=...)
- [Deploy React no Render (YouTube)](https://youtube.com/watch?v=...)

### Comunidades
- [Railway Discord](https://discord.gg/railway)
- [Render Community](https://community.render.com)
- [Azure Brasil](https://www.meetup.com/pt-BR/azurebr/)

---

## 🚀 Quick Start (5 minutos)

```bash
# 1. Clone o projeto
git clone https://github.com/seu-usuario/empresax-pos.git
cd empresax-pos

# 2. Configure variáveis locais
cp .env.example .env

# 3. Teste localmente
dotnet run --project src/EmpresaX.POS.API
cd empresax-frontend && npm run dev

# 4. Commit e push
git add .
git commit -m "Ready for deploy"
git push

# 5. Deploy no Railway
# - Acesse railway.app
# - New Project → GitHub Repo
# - Adicione PostgreSQL
# - Configure variáveis
# - Deploy automático! 🎉
```

---

**Última Atualização:** 20 de outubro de 2025  
**Autor:** GitHub Copilot  
**Custo Total:** **R$ 0,00/mês** 💰✨
