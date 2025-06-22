using ApplicationLayer.Contracts.Repositories;
using ApplicationLayer.Contracts.Specifications;
using InfrastructureLayer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TourGuideDbContext DbContext;

        public GenericRepository(TourGuideDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public async Task AddAsync(T Entity)
        {
            await DbContext.Set<T>().AddAsync(Entity);
        }

        public void Delete(T Entity)
        {
            DbContext.Set<T>().Remove(Entity);
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsQueryable().AsNoTracking();
        }

        public IQueryable<T> GetAllWithSpecification(ISpecification<T> specification)
        {
            return SpecificationBuilder<T>.BuildQuery(DbContext.Set<T>().AsQueryable(), specification).AsNoTracking().AsQueryable();
                
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public async Task<int> GetCountWithSpecs(ISpecification<T> specification)
        {
            return await SpecificationBuilder<T>.BuildQuery(DbContext.Set<T>().AsQueryable(), specification).AsNoTracking().CountAsync();
        }

        public void Update(T Entity)
        {
            DbContext.Set<T>().Update(Entity);
        }
    }
}
