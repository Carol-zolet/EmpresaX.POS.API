



param(
  [string]$ResourceGroup = "empresax-pos-rg",
  [string]$Location = "brazilsouth",
  [string]$AppServicePlan = "empresax-pos-plan",
  [string]$WebAppName = "empresax-pos-api-$(Get-Random)",
  [string]$PostgresServer = "empresax-pos-pg-$(Get-Random)",
  [string]$PostgresDb = "empresaxposdb",
  [string]$CorsOrigins = "https://seu-frontend.com,https://outro-dominio.com,http://localhost:3000",
  [string]$ZipPath = "..\\src\\publish.zip",
  [string]$PostgresPasswordParam,
  [string]$JwtKeyParam
  )

# Fail fast
$ErrorActionPreference = "Stop"

Write-Host "Verificando Azure CLI login..." -ForegroundColor Cyan
try {
  az account show --only-show-errors | Out-Null
} catch {
  Write-Host "Você não está logado. Abrindo login interativo..." -ForegroundColor Yellow
  az login --only-show-errors | Out-Null
}

# Coleta de segredos de forma segura (ordem: parâmetro > env var > prompt)
if ($PSBoundParameters.ContainsKey('PostgresPasswordParam')) {
  $PostgresPassword = $PostgresPasswordParam
} elseif ($env:POSTGRES_ADMIN_PASSWORD) {
  $PostgresPassword = $env:POSTGRES_ADMIN_PASSWORD
} else {
  $secure = Read-Host -AsSecureString -Prompt "Defina a senha do PostgreSQL (admin)"
  $PostgresPassword = [Runtime.InteropServices.Marshal]::PtrToStringAuto([Runtime.InteropServices.Marshal]::SecureStringToBSTR($secure))
}

if ($PSBoundParameters.ContainsKey('JwtKeyParam')) {
  $JwtKey = $JwtKeyParam
} elseif ($env:JWT_KEY) {
  $JwtKey = $env:JWT_KEY
} else {
  $JwtKey = (Read-Host -Prompt "Defina o JWT:Key (segredo)")
}

$PostgresAdmin = "pgadmin$(Get-Random)"

Write-Host "Criando Resource Group '$ResourceGroup' em '$Location'..." -ForegroundColor Cyan
az group create -n $ResourceGroup -l $Location --only-show-errors | Out-Null

Write-Host "Criando App Service Plan '$AppServicePlan' (Linux, S1)..." -ForegroundColor Cyan
az appservice plan create -g $ResourceGroup -n $AppServicePlan --sku S1 --is-linux --only-show-errors | Out-Null

Write-Host "Criando Web App '$WebAppName' (runtime .NET 8)..." -ForegroundColor Cyan
az webapp create -g $ResourceGroup -p $AppServicePlan -n $WebAppName --runtime "DOTNETCORE:8.0" --only-show-errors | Out-Null

Write-Host "Criando PostgreSQL Flexible Server '$PostgresServer'..." -ForegroundColor Cyan
az postgres flexible-server create `
  -g $ResourceGroup `
  -n $PostgresServer `
  -l $Location `
  -u $PostgresAdmin `
  -p $PostgresPassword `
  --sku-name Standard_B1ms `
  --tier Burstable `
  --version 16 `
  --storage-size 32 `
  --yes `
  --only-show-errors | Out-Null

Write-Host "Criando banco '$PostgresDb'..." -ForegroundColor Cyan
az postgres flexible-server db create -g $ResourceGroup -s $PostgresServer -d $PostgresDb --only-show-errors | Out-Null

Write-Host "Habilitando acesso de serviços do Azure ao PostgreSQL..." -ForegroundColor Cyan
az postgres flexible-server firewall-rule create --resource-group $ResourceGroup --name $PostgresServer --rule-name allowazureservices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0 --only-show-errors | Out-Null

# Monta a connection string Npgsql
$Conn = "Host=$PostgresServer.postgres.database.azure.com;Database=$PostgresDb;Username=$PostgresAdmin;Password=$PostgresPassword;Ssl Mode=Require;Trust Server Certificate=True" 

Write-Host "Configurando App Settings..." -ForegroundColor Cyan
az webapp config appsettings set -g $ResourceGroup -n $WebAppName --settings `
  "ASPNETCORE_ENVIRONMENT=Production" `
  "ASPNETCORE_URLS=http://0.0.0.0:8080" `
  "Cors:PolicyName=DefaultPolicy" `
  "Cors:Origins=$CorsOrigins" `
  "Jwt:Key=$JwtKey" `
  "ConnectionStrings:DefaultConnection=$Conn" `
  --only-show-errors | Out-Null

# Aponta o container para a porta 8080 (App Service Linux)
az webapp config set -g $ResourceGroup -n $WebAppName --generic-configurations '{"linuxFxVersion": "DOTNETCORE|8.0", "properties": {"WEBSITES_PORT": "8080"}}' --only-show-errors | Out-Null

# Deploy do ZIP
$zipFull = Resolve-Path $ZipPath
if (!(Test-Path $zipFull)) {
  Write-Host "Pacote ZIP não encontrado: $zipFull" -ForegroundColor Red
  exit 1
}
Write-Host "Fazendo deploy do pacote: $zipFull" -ForegroundColor Cyan
az webapp deploy -g $ResourceGroup -n $WebAppName --src-path $zipFull --type zip --only-show-errors | Out-Null

# Aguarda warm-up e valida rota /health
$Url = "https://$($WebAppName).azurewebsites.net"
Write-Host "Aguardando warm-up e verificando /health..." -ForegroundColor Cyan
Start-Sleep -Seconds 10
try {
  $health = Invoke-WebRequest -Uri "$Url/health" -UseBasicParsing -TimeoutSec 30
  Write-Host "Health: $($health.StatusCode)" -ForegroundColor Green
} catch {
  Write-Host "Não foi possível validar /health agora. A API pode estar inicializando." -ForegroundColor Yellow
}

Write-Host "\nDeploy concluído." -ForegroundColor Green
Write-Host "URL da API: $Url" -ForegroundColor Green

# Smoke test pós-deploy
Write-Host "\nExecutando smoke test (GET /health e /swagger)..." -ForegroundColor Cyan
try {
  $healthResp = Invoke-WebRequest -Uri "$Url/health" -UseBasicParsing -TimeoutSec 30
  if ($healthResp.StatusCode -eq 200) {
    Write-Host "[OK] /health: 200" -ForegroundColor Green
  } else {
    Write-Host "[WARN] /health: $($healthResp.StatusCode)" -ForegroundColor Yellow
  }
} catch {
  Write-Host "[ERRO] /health não disponível." -ForegroundColor Red
}
try {
  $swaggerResp = Invoke-WebRequest -Uri "$Url/swagger" -UseBasicParsing -TimeoutSec 30
  if ($swaggerResp.StatusCode -eq 200) {
    Write-Host "[OK] /swagger: 200" -ForegroundColor Green
  } else {
    Write-Host "[WARN] /swagger: $($swaggerResp.StatusCode)" -ForegroundColor Yellow
  }
} catch {
  Write-Host "[ERRO] /swagger não disponível." -ForegroundColor Red
}
