using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class SystemLogEntityConfiguration : IEntityTypeConfiguration<SystemLogEntity>
{
    public void Configure(EntityTypeBuilder<SystemLogEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.LogTypeId).IsRequired();
        builder.HasOne(x => x.LogType)
            .WithMany()
            .HasForeignKey(x => x.LogTypeId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Property(x => x.Date).IsRequired().HasColumnType("DATETIME").HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.Exception).IsRequired(false);
    }
}