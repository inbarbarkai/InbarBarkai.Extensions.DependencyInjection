using System.Collections.Generic;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    public class ParameterResolverTests
    {
        [MemberData(nameof(EqualsTestData))]
        [Theory]
        public void EqualsTest(object first, object second, bool expected)
        {
            first.Equals(second).Should().Be(expected);
        }

        public static IEnumerable<object[]> EqualsTestData()
        {
            var first = new ParameterResolver(pi => true, (sp, pi) => 1);
            var second = new ParameterResolver(pi => true, (sp, pi) => 1);

            yield return new object[] { first, second, false };
            yield return new object[] { first, null, false };
            yield return new object[] { first, 1, false };
            yield return new object[] { first, first, true };
        }

        [Fact]
        public void EqualsNullTest()
        {
            var first = new ParameterResolver(pi => true, (sp, pi) => 1);
            first.Equals(other: null).Should().BeFalse();
        }
    }
}
