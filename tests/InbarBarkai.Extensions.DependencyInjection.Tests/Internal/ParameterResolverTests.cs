using System.Collections.Generic;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    /// <summary>
    /// Contains tests for the <see cref="ParameterResolver"/> class.
    /// </summary>
    public class ParameterResolverTests
    {
        /// <summary>
        /// Tests the <see cref="ParameterResolver.Equals(object)"/> method.
        /// </summary>
        /// <param name="first">The left comparison operand.</param>
        /// <param name="second">The right comparison operand.</param>
        /// <param name="expected">if set to <c>true</c> the compared objects should be equal; otherwise <c>false</c>.</param>
        [MemberData(nameof(EqualsTestData))]
        [Theory]
        public void EqualsTest(object first, object second, bool expected)
        {
            first.Equals(second).Should().Be(expected);
        }

        /// <summary>
        /// Gets the data for the <see cref="EqualsTest(object, object, bool)"/> test.
        /// </summary>
        /// <returns>The data for the <see cref="EqualsTest(object, object, bool)"/> test.</returns>
        public static IEnumerable<object[]> EqualsTestData()
        {
            var first = new ParameterResolver(pi => true, (sp, pi) => 1);
            var second = new ParameterResolver(pi => true, (sp, pi) => 1);

            yield return new object[] { first, second, false };
            yield return new object[] { first, null, false };
            yield return new object[] { first, 1, false };
            yield return new object[] { first, first, true };
        }

        /// <summary>
        /// Tests the <see cref="ParameterResolver.Equals(ParameterResolver)"/> method when comparing to a null value.
        /// </summary>
        [Fact]
        public void EqualsNullTest()
        {
            var first = new ParameterResolver(pi => true, (sp, pi) => 1);
            first.Equals(other: null).Should().BeFalse();
        }
    }
}
