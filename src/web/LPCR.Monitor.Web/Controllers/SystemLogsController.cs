using LPCR.Monitor.Core.DTO.System;
using LPCR.Monitor.Web.Infrastructure.Persistance;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Controllers;

[Route("api/system/logs")]
[ApiController]
public class SystemLogsController : ControllerBase
{
    private readonly IMonitoringDatabase _database;

    public SystemLogsController(IMonitoringDatabase database)
    {
        _database = database;
    }

    [HttpGet]
    public IActionResult GetSystemLogs()
    {
        // TODO
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateSystemLogAsync([FromBody] CreateSystemLogDto systemLogDto)
    {
        await _database.SystemLogs.AddAsync(new SystemLogEntity
        {
            LogTypeId = (int)systemLogDto.Type,
            Message = systemLogDto.Message,
            Date = systemLogDto.Date,
            Exception = systemLogDto.Exception
        });
        await _database.SaveChangesAsync();

        return Ok();
    }
}
