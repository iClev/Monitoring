using System;
using System.ComponentModel.DataAnnotations;

namespace LPCR.Monitor.Core.DTO.System;

public record CreateSystemLogDto
{
    [Required]
    public LogType Type { get; init; }

    [Required]
    public string Message { get; init; }

    [Required]
    public DateTime Date { get; init; }

    public string Exception { get; set; }
}
