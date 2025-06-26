using ApplicationLayer.Contracts.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer
{
    public static class SpecificationBuilder<T> where T : class
    {
        public static IQueryable<T> BuildQuery(IQueryable<T> query, ISpecification<T> specification)
        {
            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.Includes != null)
            {
                foreach (var include in specification.Includes)
                {
                    query = include(query);
                }

        
            }

            if (specification.OrderByAsec != null)
            {
                query = query.OrderBy(specification.OrderByAsec);
            }

            if (specification.OrderByDesc != null)
            {
                query = query.OrderByDescending(specification.OrderByDesc);
            }

            if (specification.IsPaginated)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}
