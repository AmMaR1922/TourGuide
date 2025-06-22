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
    public class TripActivitiesConfigurations : IEntityTypeConfiguration<TripActivities>
    {
        public void Configure(EntityTypeBuilder<TripActivities> builder)
        {
            builder.HasKey(t => new {t.TripId, t.ActivityId});

            builder.HasOne(b => b.Trip)
                .WithMany(t => t.Activities)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Activity)
                .WithMany(a => a.TripActivities)
                .HasForeignKey(b => b.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
