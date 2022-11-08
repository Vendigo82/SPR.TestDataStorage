using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SPR.TestDataStorage.Infra.Models
{
    public partial class ObjectTypeDataSectionModel
    {
        public Guid ObjectTypeId { get; set; }
        public Guid DataSectionId { get; set; }

        public virtual DataSectionModel DataSection { get; set; } = null!;
        public virtual ObjectTypeModel ObjectType { get; set; } = null!;

        internal static void Build(EntityTypeBuilder<ObjectTypeDataSectionModel> entity)
        {
            entity.HasNoKey();

            entity.ToTable("object_type_data_section", "data");

            entity.HasIndex(e => new { e.ObjectTypeId, e.DataSectionId }, "object_type_data_section_object_type_id_data_section_id_key")
                .IsUnique();

            entity.Property(e => e.DataSectionId).HasColumnName("data_section_id");

            entity.Property(e => e.ObjectTypeId).HasColumnName("object_type_id");

            entity.HasOne(d => d.DataSection)
                .WithMany()
                .HasForeignKey(d => d.DataSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("object_type_data_section_data_section_id_fkey");

            entity.HasOne(d => d.ObjectType)
                .WithMany()
                .HasForeignKey(d => d.ObjectTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("object_type_data_section_object_type_id_fkey");
        }
    }
}
