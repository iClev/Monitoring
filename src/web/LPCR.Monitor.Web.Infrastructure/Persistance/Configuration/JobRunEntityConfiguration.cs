using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class JobRunEntityConfiguration : IEntityTypeConfiguration<JobRunEntity>
{
    public void Configure(EntityTypeBuilder<JobRunEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("NEWID()");
        builder.Property(x => x.JobId).IsRequired();
        builder.HasOne(x => x.Job)
            .WithMany(x => x.Runs)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.StatusId).IsRequired();
        builder.HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.Created).IsRequired().HasColumnType("DATETIME").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.Started).IsRequired(false).HasColumnType("DATETIME");
        builder.Property(x => x.Completed).IsRequired(false).HasColumnType("DATETIME");
        builder.Property(x => x.Paylaod).IsRequired(false);
        builder.Property(x => x.CanRetry).IsRequired().HasColumnType("BIT");
        builder.HasMany(x => x.Logs)
            .WithOne(x => x.JobRun)
            .HasForeignKey(x => x.JobRunId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
