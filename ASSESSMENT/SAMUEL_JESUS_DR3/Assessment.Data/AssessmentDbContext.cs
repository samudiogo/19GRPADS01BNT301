using AssessmentDomain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace Assessment.Data
{
    public class AssessmentDbContext : DbContext
    {
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        
        public AssessmentDbContext() : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configuração para EF usar SP nas seguintes entidades:
            modelBuilder.Entity<Friend>().MapToStoredProcedures();
            modelBuilder.Entity<State>().MapToStoredProcedures();
            modelBuilder.Entity<Country>().MapToStoredProcedures();
            

            //configuração para evitar plurarização do nome das tabelas:
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(GetType()));

            base.OnModelCreating(modelBuilder);
        }

    }
}
