using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_BAQS.Entities;

namespace WebAPI_BAQS.Mapping
{
    public class CompaniaMap : IEntityTypeConfiguration<Compania>
    {
        public void Configure(EntityTypeBuilder<Compania> builder)
        {
            builder.ToTable("compania")
                  .HasKey(x => x.IdCompania);
        }
    }
}
