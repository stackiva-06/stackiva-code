namespace PrinterShare.Host.Services;

public sealed class PrinterStatusMonitor(ILogger<PrinterStatusMonitor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogDebug("Polling printer status from Windows spooler");
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
