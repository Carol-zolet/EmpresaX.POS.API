$Script:BaseUrl = "http://localhost:7000"
$Script:Token = $null
$Script:UserData = $null

function Connect-EmpresaXApi {
    param([string]$Login, [string]$Password)
    try {
        $payload = @{ Login = $Login; Senha = $Password } | ConvertTo-Json
        $response = Invoke-RestMethod -Uri "$Script:BaseUrl/api/v1/auth/login" -Method POST -Body $payload -ContentType "application/json"
        if ($response.Sucesso) {
            $Script:Token = $response.Token
            $Script:UserData = $response.Usuario
            Write-Host "✅ Conectado: $($response.Usuario.Nome)" -ForegroundColor Green
            return $response
        }
    } catch {
        Write-Host "❌ Erro: $($_.Exception.Message)" -ForegroundColor Red
        throw
    }
}

function Show-EmpresaXStatus {
    Write-Host "`n🔍 STATUS API" -ForegroundColor Blue
    if ($Script:Token) {
        Write-Host "✅ CONECTADO - $($Script:UserData.Nome)" -ForegroundColor Green
    } else {
        Write-Host "❌ DESCONECTADO" -ForegroundColor Red
    }
}

function Invoke-EmpresaXApiCall {
    param([string]$Endpoint, [string]$Method = "GET", $Body = $null)
    if (-not $Script:Token) { throw "Use Connect-EmpresaXApi primeiro" }
    
    $headers = @{ 'Authorization' = "Bearer $Script:Token"; 'Content-Type' = 'application/json' }
    $params = @{ Uri = "$Script:BaseUrl$Endpoint"; Method = $Method; Headers = $headers }
    if ($Body) { $params.Body = $Body | ConvertTo-Json }
    
    return Invoke-RestMethod @params
}

Export-ModuleMember -Function Connect-EmpresaXApi, Show-EmpresaXStatus, Invoke-EmpresaXApiCall
