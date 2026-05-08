using System.Drawing.Printing;
using PrinterShare.Contracts.Models;

namespace PrinterShare.Host.Services;

public sealed class PrinterDiscoveryService
{
    public IReadOnlyList<SharedPrinter> GetLocalPrinters()
    {
        var defaultPrinter = new PrinterSettings().PrinterName;
        return PrinterSettings.InstalledPrinters
            .Cast<string>()
            .Select(name => new SharedPrinter(
                PrinterId: CreatePrinterId(Environment.MachineName, name),
                DisplayName: name,
                HostName: Environment.MachineName,
                Model: name,
                IsDefault: string.Equals(name, defaultPrinter, StringComparison.OrdinalIgnoreCase),
                IsShared: false,
                Capabilities: PrinterCapabilitySet.Default))
            .ToArray();
    }

    public static string CreatePrinterId(string hostName, string printerName) =>
        Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes($"{hostName}:{printerName}")))[..16];
}
