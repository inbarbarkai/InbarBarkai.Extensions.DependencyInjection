using System;
using System.Linq;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class DelegateConstructorFinder : IConstrcutorFinder
    {
        private readonly Predicate<ConstructorInfo> _isMatch;

        public DelegateConstructorFinder(Predicate<ConstructorInfo> isMatch)
        {
            MakeSure.NotNull(isMatch, nameof(isMatch));
            _isMatch = isMatch;
        }

        public ConstructorInfo Find(Type type)
        {
            var constructor = type.GetConstructors().FirstOrDefault(i => _isMatch(i));
            if (constructor == null)
            {
                throw ConstructorFinderHelper.ThrowMissingConstructorException(type);
            }
            return constructor;
        }
    }
}
