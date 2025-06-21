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

            builder.Property(t => t.Category)
                .HasMaxLength(100);
            #endregion


            #region TripTranslations

            builder.HasMany(t => t.TripTranslations)
                .WithOne(t => t.Trip)
                .HasForeignKey(t => t.TripId);
            #endregion

            #region Bookings
            builder.HasMany(t => t.Bookings)
               .WithOne(b => b.Trip)
               .HasForeignKey(b => b.TripId);
            #endregion
            #region Wishlists


            builder.HasMany(t => t.Wishlists)
            .WithOne(w => w.Trip)
            .HasForeignKey(w => w.TripId); 
            #endregion

        }
    }
}
