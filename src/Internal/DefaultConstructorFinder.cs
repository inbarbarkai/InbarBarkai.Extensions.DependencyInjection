using System;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class DefaultConstructorFinder : IConstrcutorFinder
    {
        public ConstructorInfo Find(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new MissingMethodException($"Could not find constructor for '{type.FullName}'.");
            }
            return constructors[0];
        }
    }
}
