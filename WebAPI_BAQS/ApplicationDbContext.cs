using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;
using WebAPI_BAQS.Mapping;

namespace WebAPI_BAQS
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new CompaniaMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
