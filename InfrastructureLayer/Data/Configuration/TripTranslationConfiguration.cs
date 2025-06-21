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
    public class TripTranslationConfiguration : IEntityTypeConfiguration<TripTranslations>
    {
        public void Configure(EntityTypeBuilder<TripTranslations> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.LanguageCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(t => t.Title).HasMaxLength(200);
        }
    }
}
