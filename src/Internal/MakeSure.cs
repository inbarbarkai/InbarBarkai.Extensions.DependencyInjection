using System;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    /// <summary>
    /// This class contains generic validation methods.
    /// </summary>
    internal static class MakeSure
    {
        private const string ValueCannotBeNullOrWhiteSpace = "Value cannot be null or white space.";

        /// <summary>
        /// Makes sure that the given value is not null or white space.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">Thrown when the value is null or white space.</exception>
        internal static void NotNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(MakeSure.ValueCannotBeNullOrWhiteSpace, name);
            }
        }

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
