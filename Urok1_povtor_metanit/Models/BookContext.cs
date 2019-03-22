using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Urok1_povtor_metanit.Models
{
    public class BookContext : DbContext 
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasMany(c => c.Books)
                .WithMany(s => s.Authors)
                .Map(t => t.MapLeftKey("AuthorId")
                .MapRightKey("BookId")
                .ToTable("AuthorBook"));
        }
    }
}