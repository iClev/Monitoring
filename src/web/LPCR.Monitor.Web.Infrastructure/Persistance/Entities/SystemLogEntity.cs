using System;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Entities;

public class SystemLogEntity
{
    /// <summary>
    /// Gets or sets the log id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the log type id.
    /// </summary>
    public int LogTypeId { get; set; }

    /// <summary>
    /// Gets or sets the log type instance.
    /// </summary>
    public LogTypeEntity LogType { get; set; }

    /// <summary>
    /// Gets or sets the log date.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the log message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the log exception if it's an error.
    /// </summary>
    public string Exception { get; set; }
}