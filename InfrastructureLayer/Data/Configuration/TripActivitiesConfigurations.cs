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
            
        }
    }
}
