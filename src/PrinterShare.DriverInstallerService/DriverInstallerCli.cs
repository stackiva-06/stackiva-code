using System.Diagnostics;

namespace PrinterShare.DriverInstallerService;

public static class DriverInstallerCli
{
    public static async Task<int> RunAsync(string[] args)
    {
        if (!string.Equals(args[0], "install", StringComparison.OrdinalIgnoreCase))
        {
            Console.Error.WriteLine("Usage: install --printer <name> --endpoint <https://host:7443>");
            return 2;
        }

        var printerName = GetOption(args, "--printer") ?? "PrinterShare Remote Printer";
        var endpoint = GetOption(args, "--endpoint") ?? "https://localhost:7443";
        var driverRoot = Path.Combine(AppContext.BaseDirectory, "driver");
        var infPath = Path.Combine(driverRoot, "printerdriver.inf");
        var portName = $"PSHARE:{printerName}";

        await RunElevatedAsync("pnputil.exe", $"/add-driver \"{infPath}\" /install");
        var prnPortScript = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "Printing_Admin_Scripts", "en-US", "prnport.vbs");
        await RunElevatedAsync("cscript.exe", $"\"{prnPortScript}\" -a -r \"{portName}\" -h \"{endpoint}\" -o raw -n 9100");
        await RunElevatedAsync("rundll32.exe", $"printui.dll,PrintUIEntry /if /b \"{printerName}\" /f \"{infPath}\" /r \"{portName}\" /m \"PrinterShare Universal Remote Printer\"");
        return 0;
    }

    private static string? GetOption(string[] args, string name)
    {
        var index = Array.IndexOf(args, name);
        return index >= 0 && index + 1 < args.Length ? args[index + 1] : null;
    }

    private static async Task RunElevatedAsync(string fileName, string arguments)
    {
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        }) ?? throw new InvalidOperationException($"Failed to start {fileName}");

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new InvalidOperationException($"{fileName} failed with exit code {process.ExitCode}: {error}");
        }
    }
}
