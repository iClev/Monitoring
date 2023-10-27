using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class JobStatusTypeEntityConfiguration : IEntityTypeConfiguration<JobStatusTypeEntity>
{
    public void Configure(EntityTypeBuilder<JobStatusTypeEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasColumnType("VARCHAR(20)").HasMaxLength(20);

        var initialData = Enum.GetValues<JobStatusType>()
            .Select(x => new JobStatusTypeEntity
            {
                Id = (int)x,
                Name = x.ToString()
            });

        builder.HasData(initialData);
    }
}
