using System.Diagnostics;

namespace EmpresaX.POS.API.Services
{
    /// <summary>
    /// Serviço para coleta de métricas de performance
    /// </summary>
    public class PerformanceMetricsService
    {
        private readonly ILogger<PerformanceMetricsService> _logger;
        private readonly Dictionary<string, PerformanceMetric> _metrics = new();
        private readonly object _lock = new();

        public PerformanceMetricsService(ILogger<PerformanceMetricsService> logger)
        {
            _logger = logger;
        }

        public IDisposable MeasureOperation(string operationName)
        {
            return new OperationTimer(operationName, this);
        }

        public void RecordMetric(string operationName, long durationMs)
        {
            lock (_lock)
            {
                if (!_metrics.ContainsKey(operationName))
                {
                    _metrics[operationName] = new PerformanceMetric(operationName);
                }

                _metrics[operationName].Record(durationMs);
            }

            if (durationMs > 3000)
            {
                _logger.LogWarning("Operação lenta detectada: {Operation} levou {DurationMs}ms", 
                    operationName, durationMs);
            }
        }

        public Dictionary<string, object> GetMetrics()
        {
            lock (_lock)
            {
                var result = new Dictionary<string, object>();
                foreach (var metric in _metrics.Values)
                {
                    result[metric.Name] = new
                    {
                        metric.Count,
                        metric.AverageDurationMs,
                        metric.MinDurationMs,
                        metric.MaxDurationMs,
                        metric.TotalDurationMs
                    };
                }
                return result;
            }
        }

        public void ResetMetrics()
        {
            lock (_lock)
            {
                _metrics.Clear();
            }
        }

        private class OperationTimer : IDisposable
        {
            private readonly string _operationName;
            private readonly PerformanceMetricsService _service;
            private readonly Stopwatch _stopwatch;

            public OperationTimer(string operationName, PerformanceMetricsService service)
            {
                _operationName = operationName;
                _service = service;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _service.RecordMetric(_operationName, _stopwatch.ElapsedMilliseconds);
            }
        }
    }

    public class PerformanceMetric
    {
        public string Name { get; }
        public long Count { get; private set; }
        public long TotalDurationMs { get; private set; }
        public long MinDurationMs { get; private set; } = long.MaxValue;
        public long MaxDurationMs { get; private set; }
        public double AverageDurationMs => Count > 0 ? (double)TotalDurationMs / Count : 0;

        public PerformanceMetric(string name)
        {
            Name = name;
        }

        public void Record(long durationMs)
        {
            Count++;
            TotalDurationMs += durationMs;
            MinDurationMs = Math.Min(MinDurationMs, durationMs);
            MaxDurationMs = Math.Max(MaxDurationMs, durationMs);
        }
    }
}
