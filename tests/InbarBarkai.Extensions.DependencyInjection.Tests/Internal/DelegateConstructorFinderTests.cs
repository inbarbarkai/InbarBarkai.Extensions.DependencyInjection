using System;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    /// <summary>
    /// Contains tests for the <see cref="DelegateConstructorFinder"/> class.
    /// </summary>
    public class DelegateConstructorFinderTests
    {
        /// <summary>
        /// Tests finding a constructor with <see cref="MissingMethodException"/> due to inability to find proper constructor.
        /// </summary>
        [Fact]
        public void FindNoConstructor()
        {
            Action action = () => new DelegateConstructorFinder(ci => false).Find(typeof(TestClass));

            action.Should().Throw<MissingMethodException>();
        }

        /// <summary>
        /// Tests finding a constructor with <see cref="ArgumentNullException"/> due to missing delegate.
        /// </summary>
        [Fact]
        public void FindNoDelegate()
        {
            Action action = () => new DelegateConstructorFinder(null);

            action.Should().Throw<ArgumentNullException>();
        }

        private class TestClass
        {
            private TestClass()
            {

            }
        }
    }
}
