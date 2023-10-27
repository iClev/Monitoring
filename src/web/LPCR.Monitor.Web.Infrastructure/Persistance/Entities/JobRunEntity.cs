using System;
using System.Collections.Generic;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Entities;

public class JobRunEntity
{
    /// <summary>
    /// Gets or sets the job run id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the job id.
    /// </summary>
    public Guid JobId { get; set; }

    /// <summary>
    /// Gets or sets the job instance.
    /// </summary>
    public JobEntity Job { get; set; }

    /// <summary>
    /// Gets or sets the job status id.
    /// </summary>
    public int StatusId { get; set; }

    /// <summary>
    /// Gets or sets the job status instance.
    /// </summary>
    public JobStatusTypeEntity Status { get; set; }

    /// <summary>
    /// Gets or sets the date when the job run has been created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the date when the job run has been started.
    /// </summary>
    public DateTime? Started { get; set; }

    /// <summary>
    /// Gets or sets the date when the job run has been completed.
    /// </summary>
    public DateTime? Completed { get; set; }

    /// <summary>
    /// Gets or sets the job run additionnal payload.
    /// </summary>
    /// <remarks>
    /// Not used yet.
    /// </remarks>
    public string Paylaod { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the job run can be restarted.
    /// </summary>
    /// <remarks>
    /// Not used yet.
    /// </remarks>
    public bool CanRetry { get; set; }

    /// <summary>
    /// Gets or sets a collection that contains all the logs associated to this job run.
    /// </summary>
    public ICollection<JobRunLogEntity> Logs { get; set; } = new HashSet<JobRunLogEntity>();
}
