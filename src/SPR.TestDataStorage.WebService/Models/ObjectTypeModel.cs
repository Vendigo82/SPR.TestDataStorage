using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SPR.TestDataStorage.WebService.Models
{
    public partial class ObjectTypeModel
    {
        public ObjectTypeModel()
        {
            DataHeaders = new HashSet<DataHeaderModel>();
        }

        public Guid Id { get; set; }
        public string SystemName { get; set; } = null!;

        public virtual ICollection<DataHeaderModel> DataHeaders { get; set; }

        internal static void Build(EntityTypeBuilder<ObjectTypeModel> entity)
        {
            entity.ToTable("object_type", "data");

            entity.HasIndex(e => e.SystemName, "object_type_system_name_key")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("public.gen_random_uuid()");

            entity.Property(e => e.SystemName).HasColumnName("system_name");
        }
    }
}
