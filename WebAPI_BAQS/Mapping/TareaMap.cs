using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Mapping
{
    public class TareaMap : IEntityTypeConfiguration<Tarea>
    {
        public void Configure(EntityTypeBuilder<Tarea> builder)
        {
            builder.ToTable("tarea")
                .HasKey(x => x.IdTarea);

            builder.HasOne(x => x.BaqCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
        }
    }
}
