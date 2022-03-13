using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    internal sealed class SimpleServiceDescriptorBuilder : ServiceDescriptorBuilder
    {
        public SimpleServiceDescriptorBuilder(Type serviceType) : base(serviceType)
        {
        }

        public override IEnumerable<ServiceDescriptor> Build()
        {
            if (this.ServiceTypes.Count == 0)
            {
                yield return ServiceDescriptor.Describe(this.ImplementationType, this.ImplementationType, this.ServiceLifetime);
            }
            else if (this.ServiceTypes.Count == 1)
            {
                yield return ServiceDescriptor.Describe(this.ServiceTypes.First(), this.ImplementationType, this.ServiceLifetime);
            }
            else
            {
                yield return ServiceDescriptor.Describe(this.ImplementationType, this.ImplementationType, this.ServiceLifetime);
                foreach (var serviceType in this.ServiceTypes)
                {
                    yield return ServiceDescriptor.Describe(serviceType, sp => sp.GetRequiredService(this.ImplementationType), this.ServiceLifetime);
                }
            }
        }
    }
}