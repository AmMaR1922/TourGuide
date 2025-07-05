using ApplicationLayer.Contracts.Specifications;
using ApplicationLayer.Models;
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

            if (specification.Includes != null && specification.Includes.Count()>0)
            {
                foreach (var include in specification.Includes)
                {
                    query = include(query);
                }

        
            }

            //if (specification.OrderByAsec != null)
            //{
            //    query = query.OrderBy(specification.OrderByAsec);
            //}

            //if (specification.OrderByDesc != null)
            //{
            //    query = query.OrderByDescending(specification.OrderByDesc);
            //}

            if (specification.orderExpressions != null && specification.orderExpressions.Count > 0)
            {
                OrderExpression<T> firstOrderExpression = specification.orderExpressions.First();

                query = firstOrderExpression.IsDescending
                    ? query.OrderByDescending(firstOrderExpression.KeySelector) 
                    : query.OrderBy(firstOrderExpression.KeySelector);

                foreach (var expression in specification.orderExpressions.Skip(1))
                {
                    query = expression.IsDescending
                        ? ((IOrderedQueryable<T>)query).ThenByDescending(expression.KeySelector)
                        : ((IOrderedQueryable<T>)query).ThenBy(expression.KeySelector);
                }
            }

            if (specification.IsPaginated)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}
