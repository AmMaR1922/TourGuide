using ApplicationLayer.Contracts.Repositories;
using ApplicationLayer.Contracts.UnitToWork;
using InfrastructureLayer.Data.Context;
using InfrastructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TourGuideDbContext DbContext;
        private Dictionary<Type, object> Repositories;


        public UnitOfWork(TourGuideDbContext DbContext)
        {
            this.DbContext = DbContext;
            Repositories = new Dictionary<Type, object>();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await DbContext.Database.BeginTransactionAsync();
        }

        public async Task<bool> CompleteAsync()
        {
            return await DbContext.SaveChangesAsync() > 0;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var Name = typeof(T);
            if (!Repositories.ContainsKey(Name))
            {
                Repositories.Add(Name, new GenericRepository<T>(DbContext));
            }

            return (IGenericRepository<T>)Repositories[Name];
        }

        
    }
}
