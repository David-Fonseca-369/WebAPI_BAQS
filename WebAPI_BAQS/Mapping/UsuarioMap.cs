
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
                .HasKey(x => x.IdUsuario);

            //Refrencias a las tablas
            builder.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.IdRol);
            builder.HasOne(x => x.Compania).WithMany().HasForeignKey(x => x.IdCompania);
        }
    }
}
