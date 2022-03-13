using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    /// <summary>
    /// Defines the functionality of a service descriptor builder.
    /// </summary>
    public interface IServiceDescriptorBuilder
    {
        /// <summary>
        /// Gets or sets the service lifetime.
        /// </summary>
        /// <value>
        /// The service lifetime.
        /// </value>
        ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// Gets the service types.
        /// </summary>
        /// <value>
        /// The service types.
        /// </value>
        ICollection<Type> ServiceTypes { get; }

        /// <summary>
        /// Gets the type of the implementation.
        /// </summary>
        /// <value>
        /// The type of the implementation.
        /// </value>
        Type ImplementationType { get; }

        /// <summary>
        /// Builds the service descriptors based of the configuration.
        /// </summary>
        /// <returns>The built <see cref="ServiceDescriptor"/> instances.</returns>
        IEnumerable<ServiceDescriptor> Build();
    }
}