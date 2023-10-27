using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class LogTypeEntityConfiguration : IEntityTypeConfiguration<LogTypeEntity>
{
    public void Configure(EntityTypeBuilder<LogTypeEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasColumnType("VARCHAR(20)").HasMaxLength(20);

        var initialData = Enum.GetValues<LogType>()
            .Select(x => new LogTypeEntity
            {
                Id = (int)x,
                Name = x.ToString()
            });

        builder.HasData(initialData);
    }
}
