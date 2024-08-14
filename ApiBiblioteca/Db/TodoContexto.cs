using ApiBiblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBiblioteca.Db
{
    public class TodoContexto : DbContext

    {
        public TodoContexto(DbContextOptions<TodoContexto> options)
            : base(options)
        {
        }
        public DbSet<LibraryModels> LibraryModels { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseFirebird(configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
