using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data.Configuration
{
    public class TripReviewsConfigurations : IEntityTypeConfiguration<TripReviews>
    {
        public void Configure(EntityTypeBuilder<TripReviews> builder)
        {
            builder.HasKey(ti => new { ti.TripId, ti.UserId });

            builder.HasOne(tr => tr.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
