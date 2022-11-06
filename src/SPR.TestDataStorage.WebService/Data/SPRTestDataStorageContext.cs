using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SPR.TestDataStorage.WebService.Models;

namespace SPR.TestDataStorage.WebService.Data
{
    public partial class SPRTestDataStorageContext : DbContext
    {

        public SPRTestDataStorageContext(DbContextOptions<SPRTestDataStorageContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataContentModel> DataContents { get; set; } = null!;
        public virtual DbSet<DataHeaderModel> DataHeaders { get; set; } = null!;
        public virtual DbSet<DataSectionModel> DataSections { get; set; } = null!;
        public virtual DbSet<ObjectTypeModel> ObjectTypes { get; set; } = null!;
        public virtual DbSet<ObjectTypeDataSectionModel> ObjectTypeDataSections { get; set; } = null!;
        public virtual DbSet<ProjectModel> Projects { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto");

            modelBuilder.Entity<DataContentModel>(DataContentModel.Build);
            modelBuilder.Entity<DataHeaderModel>(DataHeaderModel.Build);
            modelBuilder.Entity<DataSectionModel>(DataSectionModel.Build);
            modelBuilder.Entity<ObjectTypeModel>(ObjectTypeModel.Build);
            modelBuilder.Entity<ObjectTypeDataSectionModel>(ObjectTypeDataSectionModel.Build);
            modelBuilder.Entity<ProjectModel>(ProjectModel.Build);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
