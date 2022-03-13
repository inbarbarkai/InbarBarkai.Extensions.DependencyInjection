using System;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    /// <summary>
    /// Contains tests for the <see cref="DefaultConstructorFinder"/> class.
    /// </summary>
    public class DefaultConstructorFinderTests
    {
        /// <summary>
        /// Tests finding a constructor with <see cref="MissingMethodException"/> due to inability to find proper constructor.
        /// </summary>
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
