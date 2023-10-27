using System.ComponentModel.DataAnnotations;

namespace LPCR.Monitor.Core.DTO;

public record CreateJobDto
{
    [Required]
    public string Name { get; init; }

    public string Description { get; init; }

    [Required]
    public string ProcessorName { get; init; }

    public bool IsActive { get; init; }

    [Required]
    public string Schedule { get; init; }
}
