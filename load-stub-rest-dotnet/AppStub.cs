using Load.Stub.Rest.dotNet.Effects;
using Load.Stub.Rest.dotNet.Model;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Load.Stub.Rest.dotNet
{
    public class AppStub : IHostedService
    {
        private CancellationTokenSource tokenSource;
        private Task task;
        private readonly ILogger<AppStub> logger;
        private readonly ISystemOperations systemOperations;



        public AppStub(ILogger<AppStub> logger,
                       ISystemOperations systemOperations)
        {
            this.logger = logger;
            this.systemOperations = systemOperations;
            tokenSource = new CancellationTokenSource();
            
            task = Task.CompletedTask;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            task = BackgroundLoop(tokenSource.Token);

            return Task.CompletedTask;
        }

        private async Task BackgroundLoop(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;
                    
                    using var loop = await systemOperations.CreateBackgroundOperation(cancellationToken);

                    if (loop == null)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
                        continue;
                    }
                    await loop.Execute(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("BackgroundLoop cancelled.");
                    return;
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "BackgroundLoop failed.");
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            tokenSource.Cancel();

            await task.WaitAsync(cancellationToken);

            tokenSource.Dispose();
        }
    }
}
