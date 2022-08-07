using System.Diagnostics;
using System.Reflection;

internal class MyHostedService : IHostedService
{
    private readonly CancellationTokenSource tokenSource;
    private readonly Stopwatch stopwatch;
    private Task task;


    public MyHostedService()
    {
        tokenSource = new CancellationTokenSource();
        stopwatch = Stopwatch.StartNew();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        task = PrintLoop(tokenSource.Token);
    }

    /// <summary>
    /// Print elapsed time on top of the scree
    /// </summary>
    /// <returns></returns>
    public async Task PrintLoop(CancellationToken cancellationToken)
    {
        Console.CursorVisible = false;
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Title = $"{Assembly.GetExecutingAssembly().GetName().Name} {stopwatch.Elapsed:hh\\:mm\\:ss}";
            await Task.Delay(1000, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        tokenSource.Cancel();
        try
        {
            await task;
        }
        catch (TaskCanceledException)
        {
            return;
        }
        finally
        {
            tokenSource.Dispose();
        }
    }

}