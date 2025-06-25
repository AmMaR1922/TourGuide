using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data.Configuration
{
    public class TripIncludesConfigurations : IEntityTypeConfiguration<TripIncludes>
    {
        public void Configure(EntityTypeBuilder<TripIncludes> builder)
        {
            builder.HasKey(ti => new { ti.TripId, ti.IncludesId });

            builder.HasOne(ti => ti.Trip)
                .WithMany(t => t.TripIncludes)
                .HasForeignKey(ti => ti.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ti => ti.Includes)
                .WithMany(i => i.TripIncludes)
                .HasForeignKey(ti => ti.IncludesId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
