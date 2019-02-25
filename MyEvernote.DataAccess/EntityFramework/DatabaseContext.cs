using MyEvernote.Entities;
using System.Data.Entity;

namespace MyEvernote.DataAccess.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // FluentAPI
            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments)
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes)
                .WithRequired(l => l.Note)
                .WillCascadeOnDelete(true);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}