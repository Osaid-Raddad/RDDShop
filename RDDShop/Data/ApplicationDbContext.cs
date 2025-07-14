using Microsoft.EntityFrameworkCore;
using RDDShop.Models.Category;

namespace RDDShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           base.OnConfiguring(optionsBuilder);
           optionsBuilder.UseSqlServer("Server=DESKTOP-420PRC7;Database=RDDShop;TrustServerCertificate=True; Trusted_Connection=True");

        }   
    }
}
