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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.TripDate).IsRequired();
            builder.Property(b => b.CreatedAt).IsRequired();
            builder.Property(b => b.Adults).IsRequired();
            builder.Property(b => b.Children).IsRequired();

            builder.Property(b => b.Status)
                .HasConversion<int>();

            builder.HasOne(b => b.Trip)
                .WithMany(t => t.TripBookings)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.HasIndex(b => b.TripDate)
                .IsUnique(false);

        }
    }
}
