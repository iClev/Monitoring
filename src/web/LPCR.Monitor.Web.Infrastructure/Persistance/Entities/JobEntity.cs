using System;
using System.Collections.Generic;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Entities;

public class JobEntity
{
    /// <summary>
    /// Gets or sets the job id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the job name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the job description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the job processor name.
    /// </summary>
    public string ProcessorName { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates if the job is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the job schedule type id.
    /// </summary>
    public int ScheduleTypeId { get; set; }

    /// <summary>
    /// Gets or sets the CRON schedule format.
    /// </summary>
    public string Schedule { get; set; }

    /// <summary>
    /// Gets or sets the job schedule type.
    /// </summary>
    public JobScheduleTypeEntity ScheduleType { get; set; }

    /// <summary>
    /// Gets or sets a colleciton that contains all the runs of the current job.
    /// </summary>
    public ICollection<JobRunEntity> Runs { get; set; } = new HashSet<JobRunEntity>();

    // TODO: add more properties related to scheduling.
}
