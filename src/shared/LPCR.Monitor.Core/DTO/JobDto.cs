using System;

namespace LPCR.Monitor.Core.DTO;

public record JobDto
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public string ProcessorName { get; init; }

    public bool IsActive { get; init; }

    public string Schedule { get; init; }
}
