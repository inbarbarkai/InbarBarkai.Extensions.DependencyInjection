using System;
using System.Collections;
using System.Collections.Generic;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class ServiceTypeCollection : ICollection<Type>
    {
        private readonly ICollection<Type> _serviceTypes = new HashSet<Type>();
        private readonly Type _implementationType;

        public int Count => _serviceTypes.Count;

        bool ICollection<Type>.IsReadOnly => false;

        public ServiceTypeCollection(Type implementationType)
        {
            MakeSure.NotNull(implementationType, nameof(implementationType));
            _implementationType = implementationType;
        }

        public void Add(Type item)
        {
            MakeSure.NotNull(item, nameof(item));
            if (!item.IsAssignableFrom(_implementationType))
            {
                throw new ArgumentException($"The type '{_implementationType.FullName}' cannot be assigned to '{item.FullName}'", nameof(item));
            }
            if (!item.IsInterface)
            {
                throw new ArgumentException("Cannot add service type that is not an interface.", nameof(item));
            }
            _serviceTypes.Add(item);
        }

        public void Clear()
            => _serviceTypes.Clear();

        public bool Contains(Type item)
            => _serviceTypes.Contains(item);

        public void CopyTo(Type[] array, int arrayIndex)
            => _serviceTypes.CopyTo(array, arrayIndex);

        public IEnumerator<Type> GetEnumerator()
            => _serviceTypes.GetEnumerator();

        public bool Remove(Type item)
            => _serviceTypes.Remove(item);

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
