using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    public abstract class ServiceDescriptorBuilder : IServiceDescriptorBuilder
    {
        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Transient;

        public ICollection<Type> ServiceTypes { get; set; } = new HashSet<Type>();

        public Type ImplementationType { get; }

        protected ServiceDescriptorBuilder(Type serviceType)
        {
            this.ImplementationType = serviceType;
        }

        public static IServiceDescriptorBuilder Create(Type serviceType)
        {
            if (serviceType is null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }
            return new SimpleServiceDescriptorBuilder(serviceType);
        }

        public static IServiceDescriptorBuilder Create<T>()
        {
            return ServiceDescriptorBuilder.Create(typeof(T));
        }

        public abstract void AddTo(IServiceCollection services);
    }
}