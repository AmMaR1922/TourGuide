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
    public class LanguageConfigurations : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            #region Id
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).UseIdentityColumn(1, 1);
            #endregion

            #region Code
            builder.Property(l => l.Code).IsRequired();
            builder.Property(l => l.Code).HasMaxLength(4);
            #endregion

            #region Name
            builder.Property(l => l.Name).IsRequired();
            #endregion

            #region TripTransRelation  
            builder.HasMany(l => l.TripTranslations)
                 .WithOne(l => l.Language)
                 .HasForeignKey(l => l.LanguageId)
                 .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region IncludeTransRelation
            builder.HasMany(l => l.IncludesTranslations)
                  .WithOne(l => l.Language)
                  .HasForeignKey(l => l.LanguageId)
                  .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region CategoryTransRelation

            builder.HasMany(l => l.CategoryTranslations)
                .WithOne(l => l.Language)
                .HasForeignKey(l => l.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion


            #region ActivityTransRelation
            builder.HasMany(l => l.ActivityTranslation)
                  .WithOne(l => l.Language)
                  .HasForeignKey(l => l.LanguageId)
                  .OnDelete(DeleteBehavior.Cascade);

            #endregion



             

        }
    }
}
