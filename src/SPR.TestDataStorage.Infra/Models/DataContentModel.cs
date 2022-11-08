using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace SPR.TestDataStorage.Infra.Models
{
    public partial class DataContentModel
    {
        public Guid Id { get; set; }
        public Guid DataHeaderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DataSections { get; set; } = null!;
        public string Hash { get; set; } = null!;

        public virtual DataHeaderModel DataHeader { get; set; } = null!;

        internal static void Build(EntityTypeBuilder<DataContentModel> entity)
        {
            entity.ToTable("data_content", "data");

            entity.HasIndex(e => new { e.DataHeaderId, e.Hash }, "data_content_data_header_id_hash_key")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("public.gen_random_uuid()");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.Property(e => e.DataHeaderId).HasColumnName("data_header_id");

            entity.Property(e => e.DataSections)
                .HasColumnType("json")
                .HasColumnName("data_sections");

            entity.Property(e => e.Hash).HasColumnName("hash");

            entity.HasOne(d => d.DataHeader)
                .WithMany(p => p.DataContents)
                .HasForeignKey(d => d.DataHeaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("data_content_data_header_id_fkey");
        }
    }
}
