using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OneTrack.PM.Core.Specifications
{
    public interface ISpecification<T> where T : class,new()
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        List<string> IncludeStrings { get; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
