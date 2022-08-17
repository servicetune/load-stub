using Load.Stub.Rest.dotNet.Configuration;

namespace Load.Stub.Rest.dotNet.Effects
{
    public class SystemOperations : ISystemOperations
    {
        private readonly ICurrentConfig<AppStubOptions> currentConfig;
        private readonly IOperationRuntimeProvider operationRuntimeProvider;
        private readonly Serilog.ILogger logger;

        public SystemOperations(ICurrentConfig<AppStubOptions> currentConfig, IOperationRuntimeProvider operationRuntimeProvider, Serilog.ILogger logger)
        {
            this.currentConfig = currentConfig;
            this.operationRuntimeProvider = operationRuntimeProvider;
            this.logger = logger;
        }

        public async Task<IOperationRuntime> CreateBackgroundOperation(CancellationToken cancellationToken)
        {
            var loop = currentConfig.Value?.BackgroundLoop;

            while (loop == null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(3000,cancellationToken);
                loop = currentConfig.Value?.BackgroundLoop;
            }


            return operationRuntimeProvider.For(loop);
        }
        public async Task<IOperationRuntime> CreateEndpoint(string endpointName,CancellationToken cancellationToken)
        {
            var eps = currentConfig.Value?.Endpoints;
            while (eps == null && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(3000, cancellationToken);

                eps  = currentConfig.Value?.Endpoints;
            }

            if (eps?.TryGetValue(endpointName, out var ep)??false)
            {
                return operationRuntimeProvider.For(ep);
            }
            else
            {
                logger.Error("Endpoint {endpointName} not found", endpointName);
                throw new Exception();
            }
        }
    }
}