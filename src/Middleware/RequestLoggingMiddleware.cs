using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System.Diagnostics;

namespace EmpresaX.POS.API.Middleware
{
    /// <summary>
    /// Middleware para logging de requisições HTTP com contexto enriquecido
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();

            // Adiciona informações ao contexto do log
            using (LogContext.PushProperty("RequestId", requestId))
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("UserAgent", context.Request.Headers["User-Agent"].ToString()))
            using (LogContext.PushProperty("ClientIP", context.Connection.RemoteIpAddress?.ToString()))
            {
                try
                {
                    _logger.LogInformation("Iniciando requisição HTTP {Method} {Path}", 
                        context.Request.Method, 
                        context.Request.Path);

                    await _next(context);

                    stopwatch.Stop();
                    var elapsedMs = stopwatch.ElapsedMilliseconds;

                    _logger.LogInformation(
                        "Requisição HTTP {Method} {Path} finalizada com status {StatusCode} em {ElapsedMs}ms",
                        context.Request.Method,
                        context.Request.Path,
                        context.Response.StatusCode,
                        elapsedMs);

                    // Alerta para requisições lentas
                    if (elapsedMs > 3000)
                    {
                        _logger.LogWarning(
                            "Requisição LENTA detectada: {Method} {Path} levou {ElapsedMs}ms",
                            context.Request.Method,
                            context.Request.Path,
                            elapsedMs);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex,
                        "Erro não tratado durante requisição HTTP {Method} {Path} após {ElapsedMs}ms",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
