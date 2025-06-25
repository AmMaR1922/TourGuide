using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Contracts.Specifications
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>>? Criteria { get; set; }
        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? Includes { get; }
        Expression<Func<T, object>>? OrderByAsec { get; set; }
        Expression<Func<T, object>>? OrderByDesc { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        bool IsPaginated { get; set; }
    }
}
