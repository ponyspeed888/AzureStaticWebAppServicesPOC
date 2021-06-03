using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AzureStaticWebAppServices.Models
{
    public partial class AzureStaticWebAppServicesContext : DbContext
    {
        
        public string dbName = "AzureStaticWebAppServices.db";

        public AzureStaticWebAppServicesContext(string dbName)
        {
            this.dbName = dbName;
        }



        public AzureStaticWebAppServicesContext()
        {
        }

        public AzureStaticWebAppServicesContext(DbContextOptions<AzureStaticWebAppServicesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<FormPosted> FormPosteds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlite($"DataSource=s:\\{dbName}");
                optionsBuilder.UseSqlite($"DataSource={dbName}");
                //if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android) optionsBuilder.UseSqlite($"DataSource=/sdcard/{dbName}");
}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).ValueGeneratedOnAdd();

                entity.Property(e => e.Password).HasDefaultValueSql("password");
            });

            modelBuilder.Entity<FormPosted>(entity =>
            {
                entity.Property(e => e.FormPostedId).ValueGeneratedOnAdd();

                entity.Property(e => e.PostedTime).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
