using System.Collections.Concurrent;
using PrinterShare.Contracts.Models;

namespace PrinterShare.Host.Services;

public sealed class PrinterSharingManager(PrinterDiscoveryService discovery)
{
    private readonly ConcurrentDictionary<string, SharedPrinter> sharedPrinters = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> accessControl = new();

    public IReadOnlyCollection<SharedPrinter> SharedPrinters => sharedPrinters.Values.ToArray();

    public SharedPrinter SharePrinter(string printerId)
    {
        var printer = discovery.GetLocalPrinters().Single(p => p.PrinterId == printerId);
        var shared = printer with { IsShared = true };
        sharedPrinters[printerId] = shared;
        return shared;
    }

    public bool UnsharePrinter(string printerId) => sharedPrinters.TryRemove(printerId, out _);

    public bool IsClientAllowed(string printerId, string clientId) =>
        !accessControl.TryGetValue(printerId, out var allowedClients) || allowedClients.Contains(clientId);

    public void AllowClient(string printerId, string clientId) =>
        accessControl.AddOrUpdate(printerId, _ => [clientId], (_, clients) =>
        {
            clients.Add(clientId);
            return clients;
        });
}
