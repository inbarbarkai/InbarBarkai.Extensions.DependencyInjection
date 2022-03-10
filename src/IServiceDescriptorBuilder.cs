using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    public interface IServiceDescriptorBuilder
    {
        ServiceLifetime ServiceLifetime { get; set; }

        ICollection<Type> ServiceTypes { get; set; }

        Type ImplementationType { get; }

        void AddTo(IServiceCollection services);
    }
}