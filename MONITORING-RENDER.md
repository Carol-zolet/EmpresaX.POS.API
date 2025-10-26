# Exemplo de monitoração pós-deploy Render

# 1. Configure Application Insights, New Relic, Datadog ou outro APM no seu app (instruções específicas para .NET)
# 2. Adicione variáveis de ambiente no Render para as chaves de monitoramento
# 3. Exemplo de configuração Application Insights no Program.cs:

// Program.cs
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:ConnectionString"]);

# 4. Adicione alertas no portal do APM para erros, lentidão ou indisponibilidade

# 5. (Opcional) Adicione step no CI/CD para validar status do APM após deploy
