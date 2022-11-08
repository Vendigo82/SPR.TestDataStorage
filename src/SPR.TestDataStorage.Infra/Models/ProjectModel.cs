using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SPR.TestDataStorage.Infra.Models
{
    public partial class ProjectModel
    {
        public ProjectModel()
        {
            DataHeaders = new HashSet<DataHeaderModel>();
        }

        public Guid Id { get; set; }
        public string SystemName { get; set; } = null!;

        public virtual ICollection<DataHeaderModel> DataHeaders { get; set; }

        internal static void Build(EntityTypeBuilder<ProjectModel> entity)
        {
            entity.ToTable("project", "data");

            entity.HasIndex(e => e.SystemName, "project_system_name_key")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("public.gen_random_uuid()");

            entity.Property(e => e.SystemName).HasColumnName("system_name");
        }
    }
}
