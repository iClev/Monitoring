using System;
using System.ComponentModel.DataAnnotations;

namespace LPCR.Monitor.Core.DTO;

public record CreateJobRunLogDto
{
    [Required]
    public LogType Type { get; init; }

    [Required]
    public DateTime Date { get; init; }

    [Required]
    public string Message { get; init; }

    public string Exception { get; init; }
}