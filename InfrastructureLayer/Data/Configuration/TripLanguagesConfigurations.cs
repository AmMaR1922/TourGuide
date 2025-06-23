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
    public class TripLanguagesConfigurations : IEntityTypeConfiguration<TripLanguages>
    {
        public void Configure(EntityTypeBuilder<TripLanguages> builder)
        {
            builder.HasKey(tl => new { tl.TripId, tl.LanguageId });

            builder.HasOne(tl => tl.Trip)
                .WithMany(t => t.TripLanguages)
                .HasForeignKey(tl => tl.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tl => tl.Language)
                .WithMany(l => l.TripLanguages)
                .HasForeignKey(tl => tl.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
