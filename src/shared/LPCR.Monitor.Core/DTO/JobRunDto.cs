using System;

namespace LPCR.Monitor.Core.DTO;

public record JobRunDto
{
    public Guid Id { get; init; }

    public Guid JobId { get; init; }

    public JobStatusType Status { get; init; }

    public DateTime Created { get; init; }

    public DateTime? Started { get; init; }

    public DateTime? Completed { get; init; }
}
