using Microsoft.EntityFrameworkCore;

namespace UdemySignalR.API.Models
{
    public class AppDbContext: DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=DESKTOP-IOIMR09\\SQLEXPRESS; initial catalog=UdemySignalR; TrustServerCertificate=True; integrated security=true;");
        }


        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
