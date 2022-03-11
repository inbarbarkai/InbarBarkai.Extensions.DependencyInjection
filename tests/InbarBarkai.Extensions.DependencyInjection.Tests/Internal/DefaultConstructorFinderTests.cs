using System;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    public class DefaultConstructorFinderTests
    {
        [Fact]
        public void FindNotConstructor()
        {
            Action action = () => new DefaultConstructorFinder().Find(typeof(TestClass));

            action.Should().Throw<MissingMethodException>();
        }

        private class TestClass
        {
            private TestClass()
            {

            }
        }
    }
}
