using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using AbstraCountries.Api.Controllers;
using AbstraCountries.Resources.Data;

namespace AbstraCountries.Tests.Api
{
    public class AbstraCountriesApplicationFactory : WebApplicationFactory<CountriesController>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<AbstraCountriesDbContext>>();
                services.RemoveAll<AbstraCountriesDbContext>();

                // Add an in-memory DbContext for testing
                const string dbName = "TestDb";

                services.AddDbContext<AbstraCountriesDbContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });

                // Build service provider and seed test data
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AbstraCountriesDbContext>();
                db.Database.EnsureCreated();

                if (!db.Countries.Any())
                {
                    db.Countries.AddRange(
                        new Country { Id = "CR", Name = "Costa Rica" },
                        new Country { Id = "US", Name = "United States" }
                    );
                    db.SaveChanges();
                }

            });
        }
    }
}
