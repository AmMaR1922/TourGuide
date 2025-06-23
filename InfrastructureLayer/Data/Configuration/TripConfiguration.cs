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
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            #region Id
            builder.HasKey(t => t.Id);
            #endregion

            #region Price
            builder.Property(t => t.Price)
                   .HasPrecision(10, 2)
                   .IsRequired();
            #endregion

            #region Duration
            builder.Property(t => t.Duration)
              .HasMaxLength(100);
            #endregion

            #region Category
            builder.HasOne(t => t.Category)
                .WithMany(c => c.Trips)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TripImages
            builder.HasMany(t => t.TripImages)
                .WithOne(ti => ti.Trip)
                .HasForeignKey(ti => ti.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

        }
    }
}
