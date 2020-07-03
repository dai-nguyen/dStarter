using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }


        int PageSize { get; }
        int PageNumber { get; }
        bool IsPagingEnabled { get; }
        string SortBy { get; }
        string SortDir { get; }
    }
}
