using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JsonEFTest.GeneratedCode.Relation
{
    public partial class RelationalContext : DbContext
    {
        public RelationalContext()
        {
            Database.AutoTransactionsEnabled = false;
        }

        public RelationalContext(DbContextOptions<RelationalContext> options)
            : base(options)
        {
            Database.AutoTransactionsEnabled = false;
        }

        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Readers> Readers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseJson("Data Model=Relational;Uri='json_bookstore.json';Json Path='$.bookstore.books;$.bookstore.books.readers'");
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

            modelBuilder.Entity<Readers>(entity =>
            {
                entity.ToTable("readers");

                entity.Property(e => e.Id)
                    .HasColumnName("_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.BooksId)
                    .IsRequired()
                    .HasColumnName("books_id")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Name).HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
