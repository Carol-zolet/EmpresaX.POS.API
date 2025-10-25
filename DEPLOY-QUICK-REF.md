# 🚀 Deploy Quick Reference

## Railway.app (Recomendado)

### Backend API
```bash
# Variáveis necessárias:
DATABASE_URL=<auto-preenchido pelo Railway PostgreSQL>
ASPNETCORE_ENVIRONMENT=Production
CORS_ORIGINS=https://seu-frontend.up.railway.app
JWT_SECRET=<gerar chave forte>
JWT_ISSUER=EmpresaX.POS
JWT_AUDIENCE=EmpresaX.POS.Users
```

### Frontend React
```bash
# Variáveis necessárias:
VITE_API_URL=https://seu-backend.up.railway.app/api/v1
NODE_ENV=production
```

**Deploy Steps:**
1. Push código para GitHub
2. Criar projeto no Railway.app
3. Conectar repositório
4. Adicionar PostgreSQL database
5. Configurar variáveis acima
6. Deploy automático! ✅

---

## Render.com (100% Grátis)

### Backend API
```bash
# Build Command:
dotnet publish -c Release -o out

# Start Command:
dotnet out/EmpresaX.POS.API.dll

# Variáveis:
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
DATABASE_URL=<copiar do PostgreSQL Render>
CORS_ORIGINS=https://seu-frontend.onrender.com
JWT_SECRET=<chave forte>
```

### Frontend React
```bash
# Build Command:
npm install && npm run build

# Publish Directory:
dist

# Variáveis:
VITE_API_URL=https://seu-backend.onrender.com/api/v1
```

---

## Comandos Úteis

### Gerar JWT Secret forte
```powershell
# PowerShell
-join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | ForEach-Object {[char]$_})
```

```bash
# Linux/Mac
openssl rand -base64 32
```

### Testar Health Endpoint
```bash
curl https://sua-api.com/health
```

### Testar API
```bash
curl https://sua-api.com/api/v1/contas
```

---

## Links Rápidos

- 📘 [Guia Completo](./DEPLOY-GUIDE.md)
- 🔒 [Guia de Segurança](./SECURITY-GUIDE.md)
- 📊 [Monitoramento](./MONITORING.md)
- 🔧 [API Docs](./API-DOCUMENTATION.md)

---

## Troubleshooting Rápido

**Erro 502:** Verificar `ASPNETCORE_URLS=http://0.0.0.0:$PORT`

**CORS Error:** Adicionar URL completa com `https://` em `CORS_ORIGINS`

**DB Connection:** Verificar formato da connection string (Railway usa `postgres://`, .NET usa `Host=...`)

**Cold Start:** Configurar cron-job em [cron-job.org](https://cron-job.org) para fazer ping a cada 10min
