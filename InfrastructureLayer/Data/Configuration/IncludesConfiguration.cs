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
    public class IncludesConfiguration : IEntityTypeConfiguration<Includes>
    {
        public void Configure(EntityTypeBuilder<Includes> builder)
        {
            #region Id
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id)
                 .UseIdentityColumn(1, 1);
            #endregion

            #region Name
            builder.Property(i => i.Name)
                   .IsRequired(true);
            #endregion


            #region Relation With includetrans
            builder.HasMany(i => i.IncludesTranslations)
                  .WithOne(i => i.Includes)
                  .HasForeignKey(t => t.IncludeId)
                  .OnDelete(DeleteBehavior.Cascade);

            #endregion







        }
    }
}
