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
            modelBuilder.ApplyConfiguration(new AreaMap());
            modelBuilder.ApplyConfiguration(new BaqCabeceraMap());
            modelBuilder.ApplyConfiguration(new BaqDetalleMap());
            modelBuilder.ApplyConfiguration(new TareaMap());
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<BaqCabecera> BaqCabeceras { get; set; }
        public DbSet<BaqDetalle> BaqDetalles { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
    }
}
