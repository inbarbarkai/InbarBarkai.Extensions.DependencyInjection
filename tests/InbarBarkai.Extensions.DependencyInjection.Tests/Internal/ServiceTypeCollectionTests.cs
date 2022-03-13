using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests.Internal
{
    /// <summary>
    /// Contains tests for the <see cref="ServiceTypeCollection"/> class.
    /// </summary>
    public class ServiceTypeCollectionTests
    {
        /// <summary>
        /// Tests the <see cref="ServiceTypeCollection.Clear"/> method.
        /// </summary>
        [Fact]
        public void ClearTest()
        {
            var collection = new ServiceTypeCollection(typeof(SimpleService))
            {
                typeof(ISimpleService1)
            };

            collection.Count.Should().Be(1);

            collection.Clear();

            collection.Count.Should().Be(0);
        }

        /// <summary>
        /// Tests the <see cref="ServiceTypeCollection.Contains(Type)"/> method.
        /// </summary>
        [Fact]
        public void ContainsTest()
        {
            var collection = new ServiceTypeCollection(typeof(SimpleService))
            {
                typeof(ISimpleService1)
            };

            collection.Count.Should().Be(1);

            collection.Contains(typeof(ISimpleService1)).Should().BeTrue();
            collection.Contains(typeof(ISimpleService2)).Should().BeFalse();
        }

        /// <summary>
        /// Tests the <see cref="ServiceTypeCollection.CopyTo(Type[], int)"/> method.
        /// </summary>
        [Fact]
        public void CopyToTest()
        {
            var collection = new ServiceTypeCollection(typeof(SimpleService))
            {
                typeof(ISimpleService1),
                typeof(ISimpleService2)
            };

            var array = new Type[3];
            collection.CopyTo(array, 1);
            array[0].Should().BeNull();
            array[1].Should().Be(typeof(ISimpleService1));
            array[2].Should().Be(typeof(ISimpleService2));
        }

        /// <summary>
        /// Tests the <see cref="ServiceTypeCollection.Remove(Type)"/> method.
        /// </summary>
        [Fact]
        public void RemoveTest()
        {
            var collection = new ServiceTypeCollection(typeof(SimpleService))
            {
                typeof(ISimpleService1),
                typeof(ISimpleService2)
            };

            collection.Count.Should().Be(2);

            collection.Remove(typeof(ISimpleService1)).Should().BeTrue();
            collection.Remove(typeof(ISimpleService1)).Should().BeFalse();
            collection.Count.Should().Be(1);
            collection.First().Should().Be(typeof(ISimpleService2));
        }

        /// <summary>
        /// Tests the <see cref="ServiceTypeCollection.GetEnumerator"/> method.
        /// </summary>
        [Fact]
        public void GetEnumeratorTest()
        {
            var expected = new Type[]
            {
                typeof(ISimpleService1),
                typeof(ISimpleService2)
            };

            var collection = new ServiceTypeCollection(typeof(SimpleService))
            {
                typeof(ISimpleService1),
                typeof(ISimpleService2)
            };

            using (var enumerator1 = collection.GetEnumerator())
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    enumerator1.MoveNext().Should().BeTrue();
                    enumerator1.Current.Should().Be(expected[i]);
                }
            }

            var enumerator2 = ((IEnumerable)collection).GetEnumerator();
            try
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    enumerator2.MoveNext().Should().BeTrue();
                    enumerator2.Current.Should().Be(expected[i]);
                }
            }
            finally
            {
                if (enumerator2 is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// Tests the <see cref="ICollection{T}.IsReadOnly"/> property.
        /// </summary>
        [Fact]
        public void GetIsReadOnlyTest()
        {
            ICollection<Type> collection = new ServiceTypeCollection(typeof(SimpleService));
            collection.IsReadOnly.Should().BeFalse();            
        }
    }
}
