using LPCR.Monitor.Core;
using System;

namespace LPCR.Monitor.Web.Models;

public class JobRunModel
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public DateTime Created { get; init; }

    public DateTime? Started { get; init; }

    public DateTime? Completed { get; init; }

    public JobStatusType Status { get; init; }

    public TimeSpan? ElapsedTime => Completed - Started;
}
