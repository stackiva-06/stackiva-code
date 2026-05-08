using PrinterShare.Host.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton<PrinterDiscoveryService>();
builder.Services.AddSingleton<PrinterSharingManager>();
builder.Services.AddSingleton<JobQueueManager>();
builder.Services.AddHostedService<PrinterStatusMonitor>();
builder.Services.AddHostedService<MdnsAdvertisementService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(https =>
    {
        https.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.RequireCertificate;
        https.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
    });
});

var app = builder.Build();
app.MapGrpcService<PrinterGrpcService>();
app.MapGet("/health", () => Results.Ok(new { service = "PrinterShare.Host", status = "ready" }));
app.Run();
