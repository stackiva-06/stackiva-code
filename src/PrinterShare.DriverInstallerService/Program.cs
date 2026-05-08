using PrinterShare.DriverInstallerService;

if (args.Length > 0)
{
    return await DriverInstallerCli.RunAsync(args);
}

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options => options.ServiceName = "PrinterShare Driver Installer");
builder.Services.AddHostedService<Worker>();
await builder.Build().RunAsync();
return 0;
