using Prometheus;
using Prometheus.DotNetRuntime;
using System.Diagnostics;

namespace Load.Stub.Rest.dotNet
{
    public class MetricServiceHost : IHostedService
    {
        private readonly IMetricServer metrics;
        private readonly Serilog.ILogger logger;
        private IDisposable statsCollector;
        private IDisposable diagnostocPrometheus;
        private IDisposable evenyCounterAdapter;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MetricServiceHost(Serilog.ILogger logger)
        {
            metrics = new KestrelMetricServer(port: 9090);
            this.logger = logger;
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Task StartAsync(CancellationToken cancellationToken)
        {
            metrics.Start();

            diagnostocPrometheus = DiagnosticSourceAdapter.StartListening();
            evenyCounterAdapter = EventCounterAdapter.StartListening();

            statsCollector = DotNetRuntimeStatsBuilder.Default().StartCollecting();

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            statsCollector.Dispose();
            diagnostocPrometheus.Dispose();
            evenyCounterAdapter.Dispose();
            await metrics.StopAsync();
            logger.Information("Application metrics collection ended. Cpu consumed {cpuTime}. Runtime {runtime}", Process.GetCurrentProcess().TotalProcessorTime, DateTime.Now - Process.GetCurrentProcess().StartTime);

        }
    }
}
