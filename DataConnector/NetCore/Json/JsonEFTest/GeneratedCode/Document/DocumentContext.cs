using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JsonEFTest.GeneratedCode.Document
{
    public partial class DocumentContext : DbContext
    {
        public DocumentContext()
        {
            Database.AutoTransactionsEnabled = false;
        }

        public DocumentContext(DbContextOptions<DocumentContext> options)
            : base(options)
        {
            Database.AutoTransactionsEnabled = false;
        }

        public virtual DbSet<Books> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseJson("Data Model=Document;Uri='json_bookstore.json';Json Path='$.bookstore.books';");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("books");

                entity.Property(e => e.AuthorFirstName).HasColumnName("author.first-name");

                entity.Property(e => e.AuthorLastName).HasColumnName("author.last-name");

                entity.Property(e => e.Genre).HasColumnName("genre");

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Isbn).HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Publicationdate).HasColumnName("publicationdate");

                entity.Property(e => e.Readers).HasColumnName("readers");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
