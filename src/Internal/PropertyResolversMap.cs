using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace InbarBarkai.Extensions.DependencyInjection.Internal
{
    internal class PropertyResolversMap : IEnumerable<KeyValuePair<PropertyInfo, PropertyResolver>>
    {
        private readonly IDictionary<PropertyInfo, PropertyResolver> _resolvers = new Dictionary<PropertyInfo, PropertyResolver>();

        public int Count => _resolvers.Count;

        public IEnumerator<KeyValuePair<PropertyInfo, PropertyResolver>> GetEnumerator()
            => _resolvers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        public void Add(PropertyInfo propertyInfo, PropertyResolver resolver)
        {
            if (!propertyInfo.CanWrite)
            {
                throw new InvalidOperationException("The provided property cannot be assigned to.");
            }
            _resolvers.Add(propertyInfo, resolver);
        }

        public bool ContainsKey(PropertyInfo propertyInfo)
            => _resolvers.ContainsKey(propertyInfo);
    }
}
