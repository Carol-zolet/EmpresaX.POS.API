# 🚀 Deploy no Azure via VS Code (Opção Mais Fácil)

## ✅ Extensões Instaladas

Acabei de instalar para você:
- ✅ **Azure App Service** - Deploy do backend
- ✅ **Azure Resources** - Gerenciar recursos
- ✅ **Azure Static Web Apps** - Deploy do frontend

---

## 🎯 Passo a Passo Visual

### **PARTE 1: Login no Azure**

1. **Abrir Painel do Azure:**
   - Pressionar `Ctrl+Shift+P`
   - Digitar: `Azure: Sign In`
   - Pressionar `Enter`
   - Uma página do navegador abrirá
   - Fazer login com sua conta Microsoft

2. **Verificar Login:**
   - Olhar na barra lateral esquerda do VS Code
   - Clicar no ícone do Azure (símbolo de nuvem)
   - Você verá sua assinatura listada

---

### **PARTE 2: Criar Recursos no Azure**

#### Opção A: Via Portal Web (Recomendado para iniciantes)

1. Abrir [portal.azure.com](https://portal.azure.com)
2. Seguir os passos do arquivo `AZURE-DEPLOY-STEP-BY-STEP.md` (Passos 3-7)
3. Voltar aqui para fazer o deploy do código

#### Opção B: Via VS Code (Mais rápido, mas cria automaticamente)

Pular para **PARTE 3** - o VS Code criará tudo automaticamente!

---

### **PARTE 3: Deploy do Backend (.NET API)**

1. **Preparar o Projeto:**
   ```powershell
   # No terminal do VS Code
   cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\src"
   
   # Verificar se compila
   dotnet build -c Release
   ```

2. **Deploy via VS Code:**
   
   **Método 1: Clique Direito (Mais Fácil)**
   - No Explorer do VS Code, clicar com botão direito na pasta `src`
   - Selecionar: **"Deploy to Web App..."**
   - Seguir os prompts:
     - Selecionar assinatura do Azure
     - Escolher: **"+ Create new Web App... (Advanced)"**
     - Nome: `empresax-pos-api`
     - Resource Group: **"+ Create new resource group"** → `empresax-pos-rg`
     - Runtime Stack: **".NET 8 (LTS)"**
     - OS: **Linux**
     - Location: **Brazil South**
     - App Service Plan: **"+ Create new App Service plan"** → `empresax-pos-plan`
     - Pricing Tier: **F1 (Free)**
     - Application Insights: **"Skip for now"** (configurar depois)
   - Aguardar a criação (2-3 minutos)
   - Confirmar deploy quando perguntado
   - Aguardar deploy (3-5 minutos)

   **Método 2: Painel do Azure**
   - Na barra lateral, clicar no ícone do **Azure**
   - Expandir: **APP SERVICE**
   - Clicar no ícone **"+"** (Create New Web App)
   - Seguir os mesmos prompts acima
   - Após criar, clicar com botão direito no app
   - Selecionar: **"Deploy to Web App..."**

3. **Verificar Deploy:**
   - No painel Azure → APP SERVICE
   - Clicar com botão direito no app `empresax-pos-api`
   - Selecionar: **"Browse Website"**
   - URL: `https://empresax-pos-api.azurewebsites.net`

---

### **PARTE 4: Configurar Banco de Dados**

**Infelizmente PostgreSQL precisa ser criado via Portal ou CLI.**

#### Via Portal Azure (Mais Fácil):

1. Ir em [portal.azure.com](https://portal.azure.com)
2. Clicar em **"Create a resource"**
3. Buscar: **"Azure Database for PostgreSQL"**
4. Selecionar: **"Flexible Server"**
5. Configurar:
   - **Resource Group:** `empresax-pos-rg` (mesmo do App Service)
   - **Server name:** `empresax-pos-db`
   - **Location:** `Brazil South`
   - **PostgreSQL version:** `14`
   - **Workload type:** `Development` (Burstable, B1ms - 12 meses grátis)
   - **Authentication:** Username/password
   - **Admin username:** `adminempresax`
   - **Password:** [escolher senha forte]
   - **Networking:** `Allow public access from any Azure service`
6. Clicar em **"Review + create"** → **"Create"**
7. Aguardar criação (5-10 minutos)

#### Criar Database:

1. No Portal Azure, ir até o servidor PostgreSQL criado
2. Clicar em **"Databases"** no menu lateral
3. Clicar em **"+ Add"**
4. Nome: `empresax_pos`
5. Clicar em **"Save"**

#### Obter Connection String:

1. No servidor PostgreSQL, clicar em **"Connect"**
2. Copiar: **"Connection strings"** → **"ADO.NET"**
3. Ficará algo como:
   ```
   Host=empresax-pos-db.postgres.database.azure.com;
   Port=5432;
   Database=empresax_pos;
   Username=adminempresax;
   Password={sua_senha};
   SSL Mode=Require;
   ```

---

### **PARTE 5: Configurar Variáveis de Ambiente no VS Code**

1. **No Painel do Azure do VS Code:**
   - Expandir: **APP SERVICE**
   - Expandir sua assinatura
   - Clicar com botão direito em: `empresax-pos-api`
   - Selecionar: **"Open in Portal"**

2. **No Portal Azure:**
   - No menu lateral, clicar em: **"Configuration"** (ou "Configuração")
   - Clicar na aba: **"Application settings"**
   - Clicar em: **"+ New application setting"**

3. **Adicionar as seguintes variáveis:**

   | Name | Value |
   |------|-------|
   | `ASPNETCORE_ENVIRONMENT` | `Production` |
   | `DATABASE_URL` | `Host=empresax-pos-db.postgres.database.azure.com;Port=5432;Database=empresax_pos;Username=adminempresax;Password=SUA_SENHA;SSL Mode=Require` |
   | `JWT_SECRET` | [gerar chave aleatória forte de 32+ caracteres] |
   | `JWT_ISSUER` | `EmpresaX.POS` |
   | `JWT_AUDIENCE` | `EmpresaX.POS.Users` |
   | `CORS_ORIGINS` | `https://empresax-pos-frontend.azurewebsites.net` |

4. **Gerar JWT_SECRET (no terminal do VS Code):**
   ```powershell
   -join ((65..90) + (97..122) + (48..57) | Get-Random -Count 32 | ForEach-Object {[char]$_})
   ```
   Copiar o resultado e usar como `JWT_SECRET`

5. **Salvar:**
   - Clicar em: **"Save"** (topo da página)
   - Confirmar: **"Continue"**
   - Aguardar restart automático do app

---

### **PARTE 6: Executar Migrations**

**Via Terminal do VS Code:**

```powershell
# 1. Definir connection string temporária
$env:ConnectionStrings__DefaultConnection = "Host=empresax-pos-db.postgres.database.azure.com;Port=5432;Database=empresax_pos;Username=adminempresax;Password=SUA_SENHA;SSL Mode=Require"

# 2. Navegar até o projeto
cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\src"

# 3. Executar migrations
dotnet ef database update

# Se não tiver EF Tools instalado:
dotnet tool install --global dotnet-ef
dotnet ef database update
```

---

### **PARTE 7: Deploy do Frontend (React)**

1. **Preparar Build:**
   ```powershell
   # No terminal do VS Code
   cd "C:\Users\Carol\OneDrive - PUCRS - BR\Área de Trabalho\Projetos\EmpresaX.POS.API\empresax-frontend"
   
   # Instalar dependências
   npm install
   
   # Criar build de produção
   npm run build
   ```

2. **Configurar API URL:**
   - Criar arquivo `.env.production` na pasta `empresax-frontend`:
   ```
   VITE_API_URL=https://empresax-pos-api.azurewebsites.net/api/v1
   ```
   - Rebuild:
   ```powershell
   npm run build
   ```

3. **Deploy via Static Web Apps:**
   
   **Método 1: Via Extensão (Recomendado)**
   - Clicar com botão direito na pasta `empresax-frontend`
   - Selecionar: **"Deploy to Static Web App..."**
   - Seguir prompts:
     - Selecionar assinatura
     - Escolher: **"Create new Static Web App..."**
     - Nome: `empresax-pos-frontend`
     - Region: **Brazil South**
     - Build preset: **React**
     - App location: `/empresax-frontend`
     - Output location: `dist`
   - Aguardar deploy (5-10 minutos)

   **Método 2: Via App Service (Alternativa)**
   - Clicar com botão direito na pasta `empresax-frontend/dist`
   - Selecionar: **"Deploy to Web App..."**
   - Criar novo Web App:
     - Nome: `empresax-pos-frontend`
     - Runtime: **Node 18 LTS**
     - Pricing: **F1 (Free)**
   - Aguardar deploy

4. **Verificar:**
   - Abrir: `https://empresax-pos-frontend.azurewebsites.net`

---

### **PARTE 8: Configurar CORS (Importante!)**

1. **No Painel Azure do VS Code:**
   - Clicar com botão direito em: `empresax-pos-api`
   - Selecionar: **"Open in Portal"**

2. **No Portal:**
   - Menu lateral → **"CORS"**
   - Em **"Allowed Origins"**, adicionar:
     - `https://empresax-pos-frontend.azurewebsites.net`
     - `http://localhost:5173` (para desenvolvimento local)
   - Marcar: **"Enable Access-Control-Allow-Credentials"**
   - Clicar em: **"Save"**

---

### **PARTE 9: Testar Tudo**

1. **Health Check:**
   - No VS Code, abrir: `https://empresax-pos-api.azurewebsites.net/health`
   - Deve retornar: `Healthy`

2. **Swagger:**
   - Abrir: `https://empresax-pos-api.azurewebsites.net/swagger`
   - Testar endpoints

3. **Frontend:**
   - Abrir: `https://empresax-pos-frontend.azurewebsites.net`
   - Fazer login
   - Testar funcionalidades

---

### **PARTE 10: Ver Logs em Tempo Real (VS Code)**

1. **No Painel Azure:**
   - Expandir: **APP SERVICE**
   - Clicar com botão direito em: `empresax-pos-api`
   - Selecionar: **"Start Streaming Logs"**

2. **Terminal do VS Code mostrará logs em tempo real!**

---

## 🔧 Recursos Úteis do VS Code

### Comandos Rápidos (Ctrl+Shift+P):

```
Azure: Sign In                          - Login no Azure
Azure: Open in Portal                   - Abrir recurso no portal
Azure App Service: Deploy to Web App    - Deploy direto
Azure App Service: Start Streaming Logs - Ver logs
Azure App Service: Restart              - Reiniciar app
Azure App Service: Browse Website       - Abrir no navegador
```

### Atalhos do Painel Azure:

- **Botão direito em App Service:**
  - Deploy to Web App
  - Browse Website
  - Start/Stop/Restart
  - Open in Portal
  - Start Streaming Logs
  - Delete
  
- **Botão direito em Configuration:**
  - Upload Local Settings (sync .env)

---

## 📊 Monitorar Custos no VS Code

1. **Instalar extensão:**
   - Já está instalada: **Azure Resources**

2. **Ver recursos:**
   - No painel Azure, expandir: **RESOURCES**
   - Ver todos os recursos por grupo
   - Ver custos estimados (hover nos recursos)

---

## 🎯 Deploy Automático (GitHub Actions)

### Configurar CI/CD:

1. **No VS Code:**
   - Clicar com botão direito no app: `empresax-pos-api`
   - Selecionar: **"Configure Deployment Source..."**
   - Escolher: **"GitHub"**
   - Autenticar no GitHub
   - Selecionar repositório e branch
   - O VS Code criará o workflow automaticamente!

2. **Resultado:**
   - Arquivo `.github/workflows/azure-webapps-dotnet-core.yml` será criado
   - Todo push na branch `main` fará deploy automático!

---

## 🐛 Troubleshooting

### Problema: "Deploy failed"
**Solução:**
1. Verificar logs no painel Azure
2. Verificar se a pasta correta foi selecionada
3. Tentar rebuild: `dotnet clean && dotnet build -c Release`

### Problema: "Can't connect to database"
**Solução:**
1. No Portal Azure → PostgreSQL → **Networking**
2. Adicionar regra: **"Allow access to Azure services"**
3. Verificar connection string nas configurações do app

### Problema: "CORS error"
**Solução:**
1. Configurar CORS conforme **PARTE 8**
2. Verificar se URL do frontend está correta
3. Reiniciar app após alterar CORS

### Problema: "Site muito lento"
**Explicação:**
- **F1 Free Tier** tem "cold starts" (primeiro acesso demora)
- App hiberna após 20 minutos sem uso
- Upgrade para **B1** (~R$ 40/mês) remove cold starts

---

## ✅ Checklist Final

- [ ] Extensões do Azure instaladas
- [ ] Login no Azure feito
- [ ] Backend deployado e respondendo
- [ ] PostgreSQL criado e configurado
- [ ] Migrations executadas
- [ ] Variáveis de ambiente configuradas
- [ ] Frontend deployado
- [ ] CORS configurado
- [ ] Testes de health check OK
- [ ] Swagger acessível
- [ ] Frontend conectando na API
- [ ] Logs funcionando

---

## 📱 URLs Finais

```
Backend API:    https://empresax-pos-api.azurewebsites.net
Swagger:        https://empresax-pos-api.azurewebsites.net/swagger
Frontend:       https://empresax-pos-frontend.azurewebsites.net
Health Check:   https://empresax-pos-api.azurewebsites.net/health
Portal Azure:   https://portal.azure.com
```

---

## 💡 Dicas Profissionais

### 1. Deploy Rápido (Após configuração inicial):
- Botão direito na pasta → **Deploy to Web App**
- Selecionar o app existente
- Pronto! (~2 minutos)

### 2. Ver Logs Instantaneamente:
- `Ctrl+Shift+P` → `Azure App Service: Start Streaming Logs`

### 3. Comparar Configurações:
- Local vs Azure
- Botão direito → **Download Remote Settings**

### 4. Rollback Rápido:
- No Portal Azure → **Deployment slots**
- Ou via VS Code: **Swap Slots**

---

## 🚀 Próximos Passos

1. **Configurar CI/CD automático** (deploy a cada push)
2. **Adicionar Application Insights** (monitoramento avançado)
3. **Configurar domínio customizado** (se tiver)
4. **Configurar backup automático** do banco
5. **Adicionar staging slot** (testar antes de produção)

---

## ⚠️ Lembrete de Custos

| Recurso | Custo |
|---------|-------|
| App Service F1 | ✅ **Grátis para sempre** |
| Static Web App | ✅ **Grátis para sempre** |
| PostgreSQL B1ms | ⏰ **Grátis por 12 meses** (~R$ 50-100/mês depois) |
| Application Insights | ✅ **Grátis até 5GB/mês** |

**Total estimado após 12 meses:** ~R$ 50-100/mês (só o PostgreSQL)

**Alternativas gratuitas permanentes:**
- Railway.app (recomendado)
- Render.com
- Supabase (PostgreSQL grátis)

---

**Elaborado por:** GitHub Copilot  
**Data:** 20 de outubro de 2025  
**Método:** Deploy via VS Code (Opção Profissional)  
**Status:** ✅ Guia Completo Passo a Passo
