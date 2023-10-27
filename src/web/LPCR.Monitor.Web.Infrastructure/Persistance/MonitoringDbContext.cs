using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace LPCR.Monitor.Web.Infrastructure.Persistance;

internal class MonitoringDbContext : DbContext, IMonitoringDatabase
{
    public DbSet<JobEntity> Jobs => Set<JobEntity>();

    public DbSet<JobRunEntity> JobRuns => Set<JobRunEntity>();

    public DbSet<JobRunLogEntity> JobRunLogs => Set<JobRunLogEntity>();

    public DbSet<JobStatusTypeEntity> JobStatusTypes => Set<JobStatusTypeEntity>();

    public DbSet<JobScheduleTypeEntity> JobScheduleTypes => Set<JobScheduleTypeEntity>();

    public DbSet<LogTypeEntity> LogTypes => Set<LogTypeEntity>();

    public DbSet<SystemLogEntity> SystemLogs => Set<SystemLogEntity>();

    public MonitoringDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MonitoringDbContext).Assembly);
    }
}
