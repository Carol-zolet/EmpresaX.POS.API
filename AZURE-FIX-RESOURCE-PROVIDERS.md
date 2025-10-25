# ⚙️ Corrigir erro: "A assinatura não está registrada para usar o namespace 'Microsoft.OperationalInsights'"

Esse erro ocorre quando sua assinatura Azure ainda não registrou o provedor de recursos (resource provider) responsável pelo Log Analytics (e geralmente também precisa do `Microsoft.Insights` para Application Insights).

Sem esse registro:
- Não é possível criar Log Analytics Workspace
- Application Insights pode falhar ao criar ou vincular
- Scripts de deploy/monitoramento dão erro

---

## ✅ Solução Rápida

Você pode resolver pelo Portal (cliques) ou pela CLI (comandos).

### Opção A) Portal do Azure (mais visual)

1. Acesse: https://portal.azure.com/#view/Microsoft_Azure_Resources/ProvidersBlade
2. No topo, selecione sua **Subscription** correta
3. Busque e clique em cada provedor abaixo e depois clique em **Register**:
   - `Microsoft.OperationalInsights`
   - `Microsoft.Insights`
   - (Opcional, mas útil) `Microsoft.Web`, `Microsoft.ContainerRegistry`, `Microsoft.App`
4. Aguarde o status mudar para **Registered** (pode levar 1-3 minutos)
5. Atualize a página se necessário

### Opção B) Azure CLI (PowerShell)

Abra o PowerShell (pode ser no VS Code) e execute:

```powershell
# 1) Login (se ainda não fez)
az login

# 2) Verificar assinatura ativa
az account show --output table
# (se necessário) definir outra assinatura
# az account set --subscription "<SUBSCRIPTION_ID>"

# 3) Registrar os providers necessários
az provider register --namespace Microsoft.OperationalInsights
az provider register --namespace Microsoft.Insights

# (Opcional)
az provider register --namespace Microsoft.Web
az provider register --namespace Microsoft.ContainerRegistry
az provider register --namespace Microsoft.App

# 4) Verificar o status até ficar 'Registered'
az provider show --namespace Microsoft.OperationalInsights --query "registrationState"
az provider show --namespace Microsoft.Insights --query "registrationState"

# 5) Listar todos e conferir
az provider list --query "[].{Namespace:namespace, State:registrationState}" --out table
```

Se aparecer `NotRegistered`, aguarde 1-3 minutos e rode o comando de `show` novamente.

---

## ▶️ Depois do Registro: criar Log Analytics e App Insights

Quando os providers estiverem `Registered`, você pode criar os recursos de monitoramento normalmente.

```powershell
# Variáveis (ajuste os nomes conforme seu padrão)
$rg = "empresax-pos-rg"
$location = "brazilsouth"
$lawName = "empresax-law"
$appInsightsName = "empresax-pos-insights"

# 1) Criar Log Analytics Workspace
az monitor log-analytics workspace create `
  --resource-group $rg `
  --workspace-name $lawName `
  --location $location

# 2) Pegar a resource id do workspace (para vincular no Insights)
$workspaceId = az monitor log-analytics workspace show `
  --resource-group $rg `
  --workspace-name $lawName `
  --query id -o tsv

# 3) Criar Application Insights ligado ao Workspace
az monitor app-insights component create `
  --app $appInsightsName `
  --location $location `
  --resource-group $rg `
  --application-type web `
  --kind web `
  --workspace $workspaceId

# 4) Obter Connection String
$aiConn = az monitor app-insights component show `
  --app $appInsightsName `
  --resource-group $rg `
  --query connectionString -o tsv

Write-Host "Application Insights Connection String:" $aiConn
```

Com a connection string em mãos, configure no App Service:

```powershell
az webapp config appsettings set `
  --name empresax-pos-api `
  --resource-group empresax-pos-rg `
  --settings APPLICATIONINSIGHTS_CONNECTION_STRING="$aiConn"
```

---

## 🔍 Dicas
- O registro de provider é por **assinatura** (subscription). Se trocar de assinatura, pode precisar registrar novamente.
- Às vezes o Portal demora para atualizar o status, mas o comando `az provider show` é confiável.
- Se você usa políticas corporativas, pode precisar de permissão de Owner/Contributor na assinatura para registrar providers.

---

## ✅ Checklist
- [ ] `Microsoft.OperationalInsights` = Registered
- [ ] `Microsoft.Insights` = Registered
- [ ] Log Analytics Workspace criado
- [ ] Application Insights criado e vinculado ao Workspace
- [ ] Connection String aplicada no App Service

Se quiser, posso rodar os comandos com você passo a passo. É só me dizer se prefere Portal ou CLI! 💪
