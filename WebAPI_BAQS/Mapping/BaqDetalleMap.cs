using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Mapping
{
    public class BaqDetalleMap : IEntityTypeConfiguration<BaqDetalle>
    {
        public void Configure(EntityTypeBuilder<BaqDetalle> builder)
        {
            builder.ToTable("baqDetalle")
                .HasKey(x => x.IdDetalle);


            builder.HasOne(x => x.BaqCabecera).WithMany().HasForeignKey(x => x.IdCabecera);
        }
    }
}
