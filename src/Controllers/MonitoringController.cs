using Microsoft.AspNetCore.Mvc;
using EmpresaX.POS.API.Services;

namespace EmpresaX.POS.API.Controllers
{
    /// <summary>
    /// Controller para monitoramento e métricas da aplicação
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MonitoringController : ControllerBase
    {
        private readonly PerformanceMetricsService _metricsService;
        private readonly ILogger<MonitoringController> _logger;

        public MonitoringController(
            PerformanceMetricsService metricsService,
            ILogger<MonitoringController> logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        /// <summary>
        /// Retorna métricas de performance da aplicação
        /// </summary>
        [HttpGet("metrics")]
        public IActionResult GetMetrics()
        {
            var metrics = _metricsService.GetMetrics();
            return Ok(new
            {
                timestamp = DateTime.UtcNow,
                metrics = metrics,
                uptime = GetUptime()
            });
        }

        /// <summary>
        /// Reseta as métricas coletadas
        /// </summary>
        [HttpPost("metrics/reset")]
        public IActionResult ResetMetrics()
        {
            _metricsService.ResetMetrics();
            _logger.LogInformation("Métricas de performance resetadas");
            return Ok(new { message = "Métricas resetadas com sucesso" });
        }

        /// <summary>
        /// Testa o sistema de logging
        /// </summary>
        [HttpPost("test-logging")]
        public IActionResult TestLogging()
        {
            _logger.LogTrace("Log de TRACE - detalhes minuciosos");
            _logger.LogDebug("Log de DEBUG - informação de debug");
            _logger.LogInformation("Log de INFORMATION - evento geral");
            _logger.LogWarning("Log de WARNING - alerta");
            _logger.LogError("Log de ERROR - erro simulado");
            _logger.LogCritical("Log de CRITICAL - erro crítico");

            return Ok(new { message = "Logs de teste gerados com sucesso" });
        }

        /// <summary>
        /// Simula uma exceção para testar o tratamento de erros
        /// </summary>
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new InvalidOperationException("Esta é uma exceção de teste para monitoramento");
        }

        private string GetUptime()
        {
            var uptime = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime();
            return $"{uptime.Days}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
        }
    }
}
