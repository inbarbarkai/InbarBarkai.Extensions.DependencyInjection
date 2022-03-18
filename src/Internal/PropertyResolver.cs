using System;
using System.Linq.Expressions;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class PropertyResolver
    {
        public Expression<Func<IServiceProvider, PropertyInfo, object>> Resolve { get; }

        public PropertyResolver(Expression<Func<IServiceProvider, PropertyInfo, object>> resolve)
        {
            this.Resolve = resolve;
        }
    }
}
