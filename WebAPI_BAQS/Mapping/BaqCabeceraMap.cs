using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Mapping
{
    public class BaqCabeceraMap : IEntityTypeConfiguration<BaqCabecera>
    {
        public void Configure(EntityTypeBuilder<BaqCabecera> builder)
        {
            builder.ToTable("baqCabecera")
                .HasKey(x => x.IdCabecera);

            builder.HasOne(x => x.Area).WithMany().HasForeignKey(x => x.IdArea);
        }
    }
}
