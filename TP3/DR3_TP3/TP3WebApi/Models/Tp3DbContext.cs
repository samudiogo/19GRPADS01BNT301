using System.Data.Entity;

namespace TP3WebApi.Models
{
    public class Tp3DbContext : DbContext
    {
        public Tp3DbContext() : base("DefaultConnection") { }

        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().MapToStoredProcedures();

            base.OnModelCreating(modelBuilder);
        }

    }
}