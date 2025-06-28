using DomainLayer.Common;
using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Data.Context
{
    public class TourGuideDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public TourGuideDbContext(DbContextOptions<TourGuideDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);

            // Apply IsDeleted filter to all entities inheriting BaseEntity
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                    && entityType.BaseType == null)
                {
                    // e => e.IsDeleted == false
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var isDeletedProperty = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var filter = Expression.Lambda(
                        Expression.Equal(isDeletedProperty, Expression.Constant(false)),
                        parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripImages> TripImages { get; set; }
        public DbSet<TripReviews> TripReviews { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Includes> Includes { get; set; }
        public DbSet<TripIncludes> TripIncludes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<TripLanguages> TripLanguages { get; set; }
        public DbSet<TripActivities> TripActivities { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

    }
}
