using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class JobEntityConfiguration : IEntityTypeConfiguration<JobEntity>
{
    public void Configure(EntityTypeBuilder<JobEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
        builder.Property(x => x.Name).IsRequired().HasColumnType("VARCHAR(50)").HasMaxLength(50);
        builder.Property(x => x.Description).IsRequired(false).HasColumnType("VARCHAR(255)").HasMaxLength(255);
        builder.Property(x => x.ProcessorName).IsRequired().HasColumnType("VARCHAR(255)").HasMaxLength(255);
        builder.Property(x => x.IsActive).IsRequired().HasColumnType("BIT");
        builder.Property(x => x.ScheduleTypeId).IsRequired().HasDefaultValue((int)JobScheduleType.Scheduled);
        builder.Property(x => x.Schedule).IsRequired(false).HasColumnType("VARCHAR(20)");
        builder.HasOne(x => x.ScheduleType)
            .WithMany()
            .HasForeignKey(x => x.ScheduleTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(x => x.Runs)
            .WithOne(x => x.Job)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
