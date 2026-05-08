namespace PrinterShare.Host.Services;

public sealed class MdnsAdvertisementService(ILogger<MdnsAdvertisementService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("PrinterShare mDNS advertisement service started");
        await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
    }
}
