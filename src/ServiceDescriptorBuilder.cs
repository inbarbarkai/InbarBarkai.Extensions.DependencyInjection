using System;
using System.Collections.Generic;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains the common logic for service descriptor builders.
    /// </summary>
    /// <seealso cref="InbarBarkai.Extensions.DependencyInjection.IServiceDescriptorBuilder" />
    public abstract class ServiceDescriptorBuilder : IServiceDescriptorBuilder
    {
        /// <inheritdoc />
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <inheritdoc />
        public ICollection<Type> ServiceTypes { get; }

        /// <inheritdoc />
        public Type ImplementationType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptorBuilder"/> class.
        /// </summary>
        /// <param name="serviceType">The implementation type of the service.</param>
        protected ServiceDescriptorBuilder(Type serviceType)
        {
            MakeSure.NotNull(serviceType, nameof(serviceType));
            this.ImplementationType = serviceType;
            this.ServiceTypes = new ServiceTypeCollection(serviceType);
            this.ServiceLifetime = ServiceLifetime.Transient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDescriptorBuilder"/> class.
        /// </summary>
        /// <param name="builder">The builder to copy the configuration from.</param>
        protected ServiceDescriptorBuilder(IServiceDescriptorBuilder builder)
        {
            this.ImplementationType = builder.ImplementationType;
            this.ServiceTypes = builder.ServiceTypes;
            this.ServiceLifetime = builder.ServiceLifetime;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IServiceDescriptorBuilder"/> for the specifed <paramref name="serviceType"/> type.
        /// </summary>
        /// <param name="serviceType">The implementation type of the service.</param>
        /// <returns>The newly created <see cref="IServiceDescriptorBuilder"/>.</returns>
        public static IServiceDescriptorBuilder Create(Type serviceType)
        {
            return new SimpleServiceDescriptorBuilder(serviceType);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IServiceDescriptorBuilder"/> for the specifed <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">The implementation type of the service.</typeparam>
        /// <returns>The newly created <see cref="IServiceDescriptorBuilder"/>.</returns>
        public static IServiceDescriptorBuilder Create<T>() where T : class
        {
            return ServiceDescriptorBuilder.Create(typeof(T));
        }

        /// <inheritdoc />
        public abstract IEnumerable<ServiceDescriptor> Build();
    }
}