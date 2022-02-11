using Microsoft.EntityFrameworkCore;

namespace Prueba_Tecnica_LCFV.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Article> Articulos { get; set; }
    }
}
