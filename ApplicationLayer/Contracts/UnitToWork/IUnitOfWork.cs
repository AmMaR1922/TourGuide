using ApplicationLayer.Contracts.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.UnitToWork
{
    public interface IUnitOfWork
    {
        public IGenericRepository<T> Repository<T>() where T : class;
        public Task<bool> CompleteAsync();
        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
