using LPCR.Monitor.Core.DTO;
using LPCR.Monitor.Core.DTO.System;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LPCR.Monitor.Core.Http;

public interface IMonitoringApi
{
    [Get("/api/jobs")]
    Task<IEnumerable<JobDto>> GetJobsAsync();

    [Post("/api/jobs/{jobId}/runs")]
    Task<JobRunDto> CreateJobRunAsync(Guid jobId);

    [Post("/api/jobruns/{jobRunId}/{newStatus}")]
    Task UpdateJobRunStatusAsync(Guid jobRunId, JobStatusType newStatus);

    [Post("/api/jobruns/{jobRunId}/logs")]
    Task CreateJobRunLogAsync(Guid jobRunId, CreateJobRunLogDto log);

    [Post("/api/system/logs")]
    Task CreateSystemLogAsync([Body] CreateSystemLogDto log);
}
