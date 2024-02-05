using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Models
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-IOIMR09\\SQLEXPRESS; initial catalog=UdemyCovid19Db; TrustServerCertificate=True; integrated security=true;");
        }

        public DbSet<Covid> Covids { get; set; }
    }
}
