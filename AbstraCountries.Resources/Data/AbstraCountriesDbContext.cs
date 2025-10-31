using Microsoft.EntityFrameworkCore;

namespace AbstraCountries.Resources.Data
{
    public class AbstraCountriesDbContext : DbContext
    {
        public AbstraCountriesDbContext(DbContextOptions<AbstraCountriesDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AbstraCountriesDbContext).Assembly);
        }
    }
}
