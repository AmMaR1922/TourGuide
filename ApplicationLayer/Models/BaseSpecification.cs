using ApplicationLayer.Contracts.Specifications;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Models
{
    public class BaseSpecification<T> : ISpecification<T> where T : class
    {
        public BaseSpecification()
        {}

        public BaseSpecification(Expression<Func<T, bool>> Criteria)
        {
            this.Criteria = Criteria;
        }

        public Expression<Func<T, bool>>? Criteria { get; set; }

        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();
        //public Expression<Func<T, object>>? OrderByAsec { get; set; } = null!;
        //public Expression<Func<T, object>>? OrderByDesc { get; set; } = null!;
        public List<OrderExpression<T>> orderExpressions { get; set; } = new List<OrderExpression<T>>();
        public int Skip { get; set; }
        public int Take { get; set; }


        public void ApplyPagination(int Skip, int Take)
        {
            this.Skip = Skip;
            this.Take = Take;
        }

        public void AddInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            Includes.Add(include);
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy, bool IsDescending = false)
        {
            orderExpressions.Add(new OrderExpression<T>(orderBy, IsDescending));
        }

        public bool IsPaginated { get; set; } = false;
    }
}
