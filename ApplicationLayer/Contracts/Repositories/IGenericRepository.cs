using ApplicationLayer.Contracts.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public IQueryable<T> GetAllWithSpecification(ISpecification<T> specification);
        public Task<int> GetCountWithSpecs(ISpecification<T> specification);
        public Task<T?> GetByIdAsync(int id);
        public IQueryable<T> GetAll();
        public Task AddAsync(T Entity);
        public void Update(T Entity);
        public void Delete(T Entity);
    }
}
