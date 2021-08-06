using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JsonEFTest.GeneratedCode.Flatten
{
    public partial class FlattenedContext : DbContext
    {
        public FlattenedContext()
        {
            Database.AutoTransactionsEnabled = false;
        }

        public FlattenedContext(DbContextOptions<FlattenedContext> options)
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
                optionsBuilder.UseJson("Data Model=FlattenedDocuments;Uri='json_bookstore.json';Json Path='$.bookstore.books;$.bookstore.books.readers'");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.HasKey(e => new { e.BooksId, e.ReadersId });

                entity.ToTable("books");

                entity.Property(e => e.BooksId)
                    .HasColumnName("books:_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ReadersId)
                    .HasColumnName("readers:_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.AuthorFirstName).HasColumnName("author.first-name");

                entity.Property(e => e.AuthorLastName).HasColumnName("author.last-name");

                entity.Property(e => e.Genre).HasColumnName("genre");

                entity.Property(e => e.Isbn).HasColumnName("ISBN");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Publicationdate).HasColumnName("publicationdate");

                entity.Property(e => e.Title).HasColumnName("title");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
