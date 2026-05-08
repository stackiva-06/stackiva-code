using System.Collections.Concurrent;
using PrinterShare.Contracts.Grpc;

namespace PrinterShare.Host.Services;

public sealed class JobQueueManager
{
    private readonly ConcurrentDictionary<long, JobStatus> jobs = new();

    public JobStatus Enqueue(long jobId, PrintJobMetadata metadata, byte[] payload)
    {
        var status = new JobStatus
        {
            JobId = jobId,
            State = JobState.Queued,
            ProgressPercent = 0,
            Message = $"Queued {metadata.DocumentName} ({payload.Length:N0} bytes)"
        };
        jobs[jobId] = status;
        return status;
    }

    public JobStatus GetStatus(long jobId) => jobs.TryGetValue(jobId, out var status)
        ? status
        : new JobStatus { JobId = jobId, State = JobState.Unknown, Message = "Job not found" };

    public bool Cancel(long jobId)
    {
        if (!jobs.TryGetValue(jobId, out var status)) return false;
        status.State = JobState.Cancelled;
        status.Message = "Cancelled by client";
        return true;
    }
}
