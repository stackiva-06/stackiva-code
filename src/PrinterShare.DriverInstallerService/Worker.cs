namespace PrinterShare.DriverInstallerService;

public sealed class Worker(ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Driver installer service running as LocalSystem");
        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }
}
