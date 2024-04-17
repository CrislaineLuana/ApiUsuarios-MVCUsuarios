using ApiUsuarios_.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiUsuarios_.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {        
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
