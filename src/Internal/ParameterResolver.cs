using System;
using System.Linq.Expressions;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class ParameterResolver : IEquatable<ParameterResolver>
    {
        public Func<ParameterInfo, bool> IsMatch { get; }
        public Expression<Func<IServiceProvider, ParameterInfo, object>> Resolve { get; }

        public ParameterResolver(Func<ParameterInfo, bool> isMatch, Expression<Func<IServiceProvider, ParameterInfo, object>> resolve)
        {
            this.IsMatch = isMatch;
            this.Resolve = resolve;
        }

        public override bool Equals(object obj)
        {
            if (obj is ParameterResolver other)
            {
                return Equals(other);
            }
            return base.Equals(obj);
        }

        public bool Equals(ParameterResolver other)
        {
            if (other == null)
            {
                return false;
            }
            return IsMatch == other.IsMatch
                && Resolve == other.Resolve;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(IsMatch);
            hashCode.Add(Resolve);
            return hashCode.ToHashCode();
        }
    }
}
