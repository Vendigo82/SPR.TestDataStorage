using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SPR.TestDataStorage.Infra.Models
{
    public partial class DataSectionModel
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; } = null!;

        internal static void Build(EntityTypeBuilder<DataSectionModel> entity)
        {
            entity.ToTable("data_section", "data");

            entity.HasIndex(e => e.SystemName, "data_section_system_name_key")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("public.gen_random_uuid()");

            entity.Property(e => e.SystemName).HasColumnName("system_name");
        }
    }
}
