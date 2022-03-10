using System;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    /// <summary>
    /// This class contains generic validation methods.
    /// </summary>
    internal static class MakeSure
    {
        /// <summary>
        /// Makes sure that the given value is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        internal static void NotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
