using LibSpace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibSpace.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Autor> Autors { get; set; }
		public DbSet<Biblioteca> Bibliotecas { get; set; }
		public DbSet<Bloquear> Bloqueados { get; set; }


	}
}
