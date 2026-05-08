using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using PrinterShare.Contracts.Grpc;

namespace PrinterShare.Client.Services;

public sealed class PrinterDiscoveryClient
{
    public async Task<IReadOnlyList<PrinterInfo>> GetSharedPrintersAsync(Uri hostUri, CancellationToken cancellationToken)
    {
        using var channel = GrpcChannel.ForAddress(hostUri);
        var client = new PrinterService.PrinterServiceClient(channel);
        using var call = client.GetSharedPrinters(new Empty(), cancellationToken: cancellationToken);
        var printers = new List<PrinterInfo>();
        await foreach (var printer in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            printers.Add(printer);
        }
        return printers;
    }
}
