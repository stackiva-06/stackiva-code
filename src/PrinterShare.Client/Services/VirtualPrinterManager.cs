using System.Diagnostics;

namespace PrinterShare.Client.Services;

public sealed class VirtualPrinterManager
{
    public ProcessStartInfo BuildInstallCommand(string printerName, string hostEndpoint)
    {
        return new ProcessStartInfo
        {
            FileName = "PrinterShare.DriverInstallerService.exe",
            Arguments = $"install --printer \"{printerName}\" --endpoint \"{hostEndpoint}\"",
            UseShellExecute = true,
            Verb = "runas"
        };
    }
}
