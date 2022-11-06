using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SPR.TestDataStorage.WebService.Models
{
    public partial class DataHeaderModel
    {
        public DataHeaderModel()
        {
            DataContents = new HashSet<DataContentModel>();
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ObjectTypeId { get; set; }
        public string ObjectIdentity { get; set; } = null!;
        public string DataName { get; set; } = null!;

        public virtual ObjectTypeModel ObjectType { get; set; } = null!;
        public virtual ProjectModel Project { get; set; } = null!;
        public virtual ICollection<DataContentModel> DataContents { get; set; }

        internal static void Build(EntityTypeBuilder<DataHeaderModel> entity)
        {
            entity.ToTable("data_header", "data");

            entity.HasIndex(e => new { e.ProjectId, e.ObjectTypeId, e.ObjectIdentity, e.DataName }, "data_header_project_id_object_type_id_object_identity_data__key")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("public.gen_random_uuid()");

            entity.Property(e => e.DataName).HasColumnName("data_name");

            entity.Property(e => e.ObjectIdentity).HasColumnName("object_identity");

            entity.Property(e => e.ObjectTypeId).HasColumnName("object_type_id");

            entity.Property(e => e.ProjectId).HasColumnName("project_id");

            entity.HasOne(d => d.ObjectType)
                .WithMany(p => p.DataHeaders)
                .HasForeignKey(d => d.ObjectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_header_object_type_id_fkey");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.DataHeaders)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_header_project_id_fkey");
        }
    }
}
