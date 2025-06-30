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
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //builder.Property(e => e.PasswordHash)
            //  .IsRequired();

            builder.Property(e => e.Email)
              .IsRequired();


            builder.HasIndex(e=>e.Email)
                .IsUnique();
             

            //builder.Property(e => e.PhoneNumber)
            //  .IsRequired();

            builder.Property(e => e.UserName)
              .IsRequired();

            builder.HasIndex(e => e.UserName)
            .IsUnique();
        }
    }
}
