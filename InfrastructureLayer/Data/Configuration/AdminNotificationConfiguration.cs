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
    public class AdminNotificationConfiguration : IEntityTypeConfiguration<AdminNotification>
    {
        public void Configure(EntityTypeBuilder<AdminNotification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Type)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(n => n.Message).IsRequired();
            builder.Property(n => n.IsRead).IsRequired();
            builder.Property(n => n.CreatedAt).IsRequired();

            builder.HasOne(n => n.RelatedBooking)
                .WithMany()
                .HasForeignKey(n => n.RelatedBookingId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
