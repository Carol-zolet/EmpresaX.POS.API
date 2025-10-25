# Guia de Monitoramento - EmpresaX POS API

## 📊 Visão Geral

Este sistema possui monitoramento completo com logs estruturados, métricas de performance e alertas automáticos.

## 🔍 Logs Estruturados (Serilog)

### Configuração

Os logs são configurados em `appsettings.json` e incluem:

- **Console**: Logs formatados para desenvolvimento
- **Arquivo**: Logs diários em `logs/empresax-pos-{Date}.log`
- **Arquivo de Erros**: Apenas erros críticos em `logs/errors/empresax-pos-errors-{Date}.log`

### Níveis de Log

| Nível | Uso | Exemplo |
|-------|-----|---------|
| Trace | Detalhes muito específicos | Valores de variáveis durante debug |
| Debug | Informações de debug | Estado de objetos durante execução |
| Information | Eventos gerais | Requisições HTTP, operações concluídas |
| Warning | Situações anormais não críticas | Requisições lentas, cache miss |
| Error | Erros recuperáveis | Exceções tratadas, falhas de validação |
| Critical | Erros críticos | Falhas do sistema, perda de dados |

### Contexto Enriquecido

Todos os logs incluem automaticamente:

```json
{
  "Timestamp": "2025-10-20T15:30:45.123",
  "Level": "Information",
  "Message": "Requisição HTTP GET /api/v1/contas finalizada",
  "RequestId": "abc-123-def-456",
  "RequestPath": "/api/v1/contas",
  "RequestMethod": "GET",
  "ClientIP": "192.168.1.100",
  "UserAgent": "Mozilla/5.0...",
  "ElapsedMs": 245,
  "MachineName": "SERVER-01",
  "ThreadId": 42,
  "Application": "EmpresaX.POS.API"
}
```

### Localização dos Logs

```
logs/
├── empresax-pos-20251020.log          # Todos os logs do dia
├── empresax-pos-20251021.log
├── ...
└── errors/
    ├── empresax-pos-errors-20251020.log  # Apenas erros
    └── empresax-pos-errors-20251021.log
```

### Retenção

- **Logs gerais**: 30 dias
- **Logs de erro**: 90 dias

## 📈 Métricas de Performance

### Endpoints de Monitoramento

#### GET /api/v1/monitoring/metrics
Retorna métricas coletadas sobre operações da aplicação.

**Response:**
```json
{
  "timestamp": "2025-10-20T15:30:00Z",
  "uptime": "5d 12h 30m 15s",
  "metrics": {
    "GetAllContas": {
      "count": 1523,
      "averageDurationMs": 145.5,
      "minDurationMs": 23,
      "maxDurationMs": 3421,
      "totalDurationMs": 221606
    },
    "CreateConta": {
      "count": 89,
      "averageDurationMs": 67.2,
      "minDurationMs": 45,
      "maxDurationMs": 234,
      "totalDurationMs": 5980
    }
  }
}
```

#### POST /api/v1/monitoring/metrics/reset
Reseta as métricas coletadas (útil após deploys).

#### POST /api/v1/monitoring/test-logging
Gera logs de teste em todos os níveis.

#### GET /api/v1/monitoring/test-error
Simula uma exceção para testar o tratamento de erros.

### Usando Métricas no Código

```csharp
public class MeuService
{
    private readonly PerformanceMetricsService _metrics;
    
    public async Task<Result> MinhaOperacao()
    {
        using (_metrics.MeasureOperation("MinhaOperacao"))
        {
            // Seu código aqui
            return await ExecutarOperacao();
        }
    }
}
```

## ⚠️ Alertas Automáticos

### Requisições Lentas

Quando uma requisição leva mais de 3 segundos:

```log
[WARN] Requisição LENTA detectada: GET /api/v1/relatorios/completo levou 4523ms
RequestId: abc-123-def-456
RequestPath: /api/v1/relatorios/completo
ClientIP: 192.168.1.100
```

### Operações Lentas

Quando uma operação medida excede o threshold:

```log
[WARN] Operação lenta detectada: GerarRelatorioCompleto levou 5234ms
```

### Erros Não Tratados

Todos os erros não tratados são logados com stack trace completo:

```log
[ERROR] Erro não tratado: Object reference not set to an instance of an object
RequestId: abc-123-def-456
RequestPath: /api/v1/produtos/99999
Method: GET
Exception: System.NullReferenceException at...
```

## 🏥 Health Checks

### Endpoint: GET /health

Monitora o status da aplicação e dependências:

```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0345678",
  "entries": {
    "PostgreSQL": {
      "status": "Healthy",
      "description": "Database connection successful",
      "duration": "00:00:00.0234567"
    },
    "Memory": {
      "status": "Healthy",
      "data": {
        "allocated": "125 MB",
        "gen0": 45,
        "gen1": 12,
        "gen2": 3
      }
    }
  }
}
```

**Status possíveis:**
- `Healthy`: Tudo funcionando normalmente
- `Degraded`: Sistema funcional mas com problemas
- `Unhealthy`: Sistema com falhas críticas

## 📧 Configuração de Alertas por Email

Em `appsettings.json`:

```json
{
  "Monitoring": {
    "EnableDetailedErrors": true,
    "EnablePerformanceMetrics": true,
    "SlowRequestThresholdMs": 3000,
    "AlertEmailRecipients": [
      "admin@empresax.com",
      "devops@empresax.com"
    ]
  }
}
```

## 📊 Application Insights (Azure)

### Configuração

Em `appsettings.json`:

```json
{
  "ApplicationInsights": {
    "ConnectionString": "InstrumentationKey=xxx;IngestionEndpoint=https://...",
    "EnableAdaptiveSampling": true,
    "EnablePerformanceCounterCollectionModule": true
  }
}
```

### Telemetria Coletada

- **Requisições HTTP**: Tempo de resposta, status, path
- **Dependências**: Calls para DB, APIs externas
- **Exceções**: Todas as exceções com stack trace
- **Métricas customizadas**: Performance de operações
- **Traces**: Logs estruturados do Serilog

### Dashboard do Application Insights

Acesse: https://portal.azure.com > Application Insights > {seu-app}

**Dashboards disponíveis:**
- Overview: Visão geral de requests, failures, response time
- Performance: Análise de performance por endpoint
- Failures: Detalhes de todas as exceções
- Metrics: Gráficos customizados
- Live Metrics: Monitoramento em tempo real

## 🔔 Alertas do Azure

Configure alertas automáticos no Azure Monitor:

### 1. Taxa de Erro Alta
- **Condição**: Taxa de falha > 5% por 5 minutos
- **Ação**: Email + SMS para time de ops

### 2. Tempo de Resposta Alto
- **Condição**: Tempo médio > 2 segundos por 10 minutos
- **Ação**: Email para desenvolvedor

### 3. Disponibilidade
- **Condição**: Disponibilidade < 99% por 5 minutos
- **Ação**: Email + SMS + criar incident

## 🛠️ Ferramentas de Análise

### 1. Seq (Development)

Agregador de logs estruturados local:

```bash
docker run -d --name seq -e ACCEPT_EULA=Y -p 5341:80 datalust/seq:latest
```

Configure em `appsettings.Development.json`:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341" }
      }
    ]
  }
}
```

Acesse: http://localhost:5341

### 2. Grafana + Prometheus

Para métricas e dashboards customizados:

```yaml
# docker-compose.yml
services:
  prometheus:
    image: prom/prometheus
    ports:
      - "9090:9090"
  
  grafana:
    image: grafana/grafana
    ports:
      - "3001:3000"
```

### 3. ELK Stack

Para análise avançada de logs:

- **Elasticsearch**: Armazenamento de logs
- **Logstash**: Pipeline de processamento
- **Kibana**: Visualização e análise

## 📝 Boas Práticas

### 1. Use Níveis Apropriados

```csharp
// ❌ Errado
_logger.LogInformation("Erro ao processar: {Error}", ex.Message);

// ✅ Correto
_logger.LogError(ex, "Erro ao processar pedido {PedidoId}", pedidoId);
```

### 2. Adicione Contexto

```csharp
// ❌ Sem contexto
_logger.LogInformation("Operação concluída");

// ✅ Com contexto
_logger.LogInformation(
    "Operação {Operation} concluída para usuário {UserId} em {ElapsedMs}ms",
    "ProcessarPedido",
    userId,
    elapsedMs);
```

### 3. Use Structured Logging

```csharp
// ❌ String interpolation
_logger.LogInformation($"Usuário {usuario.Nome} fez login");

// ✅ Structured logging
_logger.LogInformation("Usuário {UserName} fez login", usuario.Nome);
```

### 4. Não Logue Dados Sensíveis

```csharp
// ❌ Expõe senha
_logger.LogDebug("Login com senha: {Password}", password);

// ✅ Seguro
_logger.LogDebug("Tentativa de login para usuário {UserEmail}", email);
```

## 🎯 KPIs Monitorados

| Métrica | Target | Alerta |
|---------|--------|--------|
| Disponibilidade | 99.9% | < 99% |
| Tempo de Resposta (P95) | < 1s | > 2s |
| Taxa de Erro | < 1% | > 5% |
| Requisições/min | - | > 1000 |
| Uso de Memória | < 2GB | > 3GB |
| Uso de CPU | < 70% | > 85% |

## 📞 Suporte

Em caso de alertas ou problemas:

1. **Verifique dashboards**: Application Insights / Grafana
2. **Analise logs recentes**: `logs/empresax-pos-{Date}.log`
3. **Verifique health check**: `GET /health`
4. **Contate o time**: devops@empresax.com

---

**Atualizado em:** Outubro 2025
