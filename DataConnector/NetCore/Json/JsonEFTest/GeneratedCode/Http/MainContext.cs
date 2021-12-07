using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JsonEFTest.GeneratedCode.Http
{
    public partial class MainContext : DbContext
    {
        public MainContext()
        {
            Database.AutoTransactionsEnabled = false;
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
            Database.AutoTransactionsEnabled = false;
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseJson("Data Model=Relational;Uri='http://45.125.239.138:8088/api/Album';Json Path='$.Album'; api config file = 'D:\\api_config.xml';Use Pool=true; username='admin'; password='123456'");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.AlbumId);

                entity.Property(e => e.ArtistId)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId);

                entity.Property(e => e.Address)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.City)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Company)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Country)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Fax)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.State)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();

                entity.Property(e => e.SupportRepId)
                    .IsRequired()
                    .ValueGeneratedOnUpdate();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
