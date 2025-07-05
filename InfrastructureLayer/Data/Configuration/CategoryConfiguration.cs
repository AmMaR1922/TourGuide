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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            #region Id
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .UseIdentityColumn(1, 1);
            #endregion

            builder.HasMany(c=>c.CategoryTranslations)
                .WithOne(c=>c.Category)
                .HasForeignKey(c=>c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            #region Name

            builder.HasIndex(c => c.Name)
                .IsUnique(true);

            builder.Property(c => c.Name)
                .IsRequired(true); 
            #endregion

        }
    }
}
