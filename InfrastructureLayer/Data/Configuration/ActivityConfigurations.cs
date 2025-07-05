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
    public class ActivityConfigurations : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            #region Id
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                 .UseIdentityColumn(1, 1);
            #endregion


            #region ActivityTrans Relation
            builder.HasMany(a => a.ActivityTranslations)
                   .WithOne(a => a.Activity)
                   .HasForeignKey(a => a.ActivityId)
                   .OnDelete(DeleteBehavior.Cascade); 
            #endregion
        }
    }
}
