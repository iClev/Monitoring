using LPCR.Monitor.Core;
using System;

namespace LPCR.Monitor.Web.Models;

public class JobRunLogModel
{
    public int Id { get; init; }

    public DateTime Date { get; init; }

    public LogType LogType { get; init; }

    public string Message { get; init; }

    public string Exception { get; init; }

    public bool ShowException { get; set; }

    public bool HasException => !string.IsNullOrWhiteSpace(Exception);

    // TODO: This behaviour needs to be relocated into its own UI component.
    public void ToggleExceptionPanel() => ShowException = !ShowException;
}