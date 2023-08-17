using System;
using System.Linq.Expressions;

namespace ResearchHive.Wrapper
{
    public class Filter<T>
    {
        public bool Condition { get; set; }
        public Expression<Func<T, bool>> Expression { get; set; }
    }
}