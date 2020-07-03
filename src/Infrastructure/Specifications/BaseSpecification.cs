using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }


        public int PageSize { get; private set; }
        public int PageNumber { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;
        public string SortBy { get; private set; }
        public string SortDir { get; private set; }

        protected BaseSpecification(
            Expression<Func<T, bool>> criteria,
            BaseFilterDto baseFilter = null,
            Dictionary<string, Expression<Func<T, object>>> columnMaps = null
            )
        {
            Criteria = criteria;

            if (baseFilter != null
                && baseFilter.PageSize > 0
                && baseFilter.PageNumber > 0
                && baseFilter.IsPagingEnabled)
            {
                ApplyOrderBy(_ => _.Id);
                ApplyPaging(baseFilter.PageSize, baseFilter.PageNumber);
            }

            if (baseFilter != null
                && !string.IsNullOrEmpty(baseFilter.SortBy)
                && !string.IsNullOrEmpty(baseFilter.SortDirection)
                && columnMaps != null
                && columnMaps.ContainsKey(baseFilter.SortBy))
            {
                if (baseFilter.SortDirection.Equals("asc", StringComparison.CurrentCultureIgnoreCase))
                {
                    ApplyOrderBy(_ => columnMaps[baseFilter.SortBy]);
                }
                else if (baseFilter.SortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    ApplyOrderByDescending(_ => columnMaps[baseFilter.SortBy]);
                }
            }
        }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddIncludes<TProperty>(Func<IncludeAggregator<T>, IIncludeQuery<T, TProperty>> includeGenerator)
        {
            var includeQuery = includeGenerator(new IncludeAggregator<T>());
            IncludeStrings.AddRange(includeQuery.Paths);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected virtual void ApplyPaging(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            IsPagingEnabled = true;
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        //Not used anywhere at the moment, but someone requested an example of setting this up.
        protected virtual void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }
    }
}
