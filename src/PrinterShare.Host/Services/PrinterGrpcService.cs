using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using PrinterShare.Contracts.Grpc;

namespace PrinterShare.Host.Services;

public sealed class PrinterGrpcService(PrinterSharingManager sharingManager, JobQueueManager jobs) : PrinterService.PrinterServiceBase
{
    public override async Task GetSharedPrinters(Empty request, IServerStreamWriter<PrinterInfo> responseStream, ServerCallContext context)
    {
        foreach (var printer in sharingManager.SharedPrinters)
        {
            await responseStream.WriteAsync(new PrinterInfo
            {
                PrinterId = printer.PrinterId,
                DisplayName = printer.DisplayName,
                HostName = printer.HostName,
                Model = printer.Model,
                IsDefault = printer.IsDefault,
                IsShared = printer.IsShared,
                Capabilities = new PrinterCapabilities
                {
                    SupportsColor = printer.Capabilities.SupportsColor,
                    SupportsDuplex = printer.Capabilities.SupportsDuplex,
                    SupportsStapling = printer.Capabilities.SupportsStapling,
                    SupportsHolePunch = printer.Capabilities.SupportsHolePunch,
                    PageSizes = { printer.Capabilities.PageSizes },
                    PaperSources = { printer.Capabilities.PaperSources },
                    ResolutionsDpi = { printer.Capabilities.ResolutionsDpi }
                }
            });
        }
    }

    public override async Task<PrintJobResponse> SubmitPrintJob(IAsyncStreamReader<PrintJobChunk> requestStream, ServerCallContext context)
    {
        await using var buffer = new MemoryStream();
        PrintJobMetadata? metadata = null;
        long jobId = 0;

        await foreach (var chunk in requestStream.ReadAllAsync(context.CancellationToken))
        {
            jobId = chunk.JobId;
            metadata ??= chunk.Metadata;
            chunk.Data.WriteTo(buffer);
        }

        if (metadata is null)
        {
            return new PrintJobResponse { Accepted = false, Message = "No metadata supplied" };
        }

        jobs.Enqueue(jobId, metadata, buffer.ToArray());
        return new PrintJobResponse { JobId = jobId, Accepted = true, Message = "Print job accepted" };
    }

    public override Task<JobStatus> GetJobStatus(JobRequest request, ServerCallContext context) =>
        Task.FromResult(jobs.GetStatus(request.JobId));

    public override Task<CancelResponse> CancelJob(JobRequest request, ServerCallContext context)
    {
        var cancelled = jobs.Cancel(request.JobId);
        return Task.FromResult(new CancelResponse { JobId = request.JobId, Cancelled = cancelled, Message = cancelled ? "Cancelled" : "Job not found" });
    }

    public override Task<RegistrationResponse> RegisterClient(ClientInfo request, ServerCallContext context) =>
        Task.FromResult(new RegistrationResponse { Accepted = true, SessionToken = Guid.NewGuid().ToString("N"), Message = "Client registered" });

    public override Task<HeartbeatResponse> Heartbeat(ClientInfo request, ServerCallContext context) =>
        Task.FromResult(new HeartbeatResponse { Ok = true, ServerTime = Timestamp.FromDateTime(DateTime.UtcNow) });
}
