using System;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    /// <summary>
    /// Defines the logic of a constructor finder.
    /// </summary>
    public interface IConstrcutorFinder
    {
        /// <summary>
        /// Finds the constructor of the specified type.
        /// </summary>
        /// <param name="type">The type to find the constructor to.</param>
        /// <returns>The constructor of the <paramref name="type"/>.</returns>
        ConstructorInfo Find(Type type);
    }
}
