using System;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal static class ConstructorFinderHelper
    {
        internal static Exception ThrowMissingConstructorException(Type type)
            => new MissingMethodException($"Could not find constructor for '{type.FullName}'.");
    }
}
