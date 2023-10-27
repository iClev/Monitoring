using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace LPCR.Monitor.Web.Infrastructure.Persistance.Configuration;

internal sealed class JobScheduleTypeEntityConfiguration : IEntityTypeConfiguration<JobScheduleTypeEntity>
{
    public void Configure(EntityTypeBuilder<JobScheduleTypeEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasColumnType("VARCHAR(20)").HasMaxLength(20);

        var initialData = Enum.GetValues<JobScheduleType>()
            .Select(x => new JobScheduleTypeEntity
            {
                Id = (int)x,
                Name = x.ToString()
            });

        builder.HasData(initialData);
    }
}