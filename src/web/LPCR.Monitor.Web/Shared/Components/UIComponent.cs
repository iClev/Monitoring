using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace LPCR.Monitor.Web.Shared.Components;

public class UIComponent : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

    [Parameter] 
    public RenderFragment ChildContent { get; set; }
}
