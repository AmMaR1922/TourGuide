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

            #region Bookings
            builder.HasMany(t => t.TripBookings)
               .WithOne(b => b.Trip)
               .HasForeignKey(b => b.TripId);
            #endregion

            #region Wishlists


            builder.HasMany(t => t.Wishlist)
            .WithOne(w => w.Trip)
            .HasForeignKey(w => w.TripId); 
            #endregion

            #region Languages
            builder.HasMany(t => t.TripLanguages)
                .WithOne(t => t.Trip)
                .HasForeignKey(l => l.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TripIncludes
            builder.HasMany(t => t.TripIncludes)
                .WithOne(ti => ti.Trip)
                .HasForeignKey(ti => ti.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TripImages
            builder.HasMany(t => t.TripImages)
                .WithOne(ti => ti.Trip)
                .HasForeignKey(ti => ti.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TripReviews
            builder.HasMany(t => t.TripReviews)
                .WithOne(ti => ti.Trip)
                .HasForeignKey(ti => ti.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region TripActivities
            builder.HasMany(t => t.Activities)
                .WithOne(t => t.Trip)
                .HasForeignKey(ti => ti.TripId);
            #endregion




        }
    }
}
