using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    internal sealed class SimpleServiceDescriptorBuilder : ServiceDescriptorBuilder
    {
        public SimpleServiceDescriptorBuilder(Type serviceType) : base(serviceType)
        {
        }

        public override void AddTo(IServiceCollection services)
        {
            if (this.ServiceTypes.Count == 0)
            {
                services.Add(ServiceDescriptor.Describe(this.ImplementationType, this.ImplementationType, this.ServiceLifetime));
            }
            else if (this.ServiceTypes.Count == 1)
            {
                services.Add(ServiceDescriptor.Describe(this.ServiceTypes.First(), this.ImplementationType, this.ServiceLifetime));
            }
            else
            {
                services.Add(ServiceDescriptor.Describe(this.ImplementationType, this.ImplementationType, this.ServiceLifetime));
                foreach (var serviceType in this.ServiceTypes)
                {
                    services.Add(ServiceDescriptor.Describe(serviceType, sp => sp.GetRequiredService(this.ImplementationType), this.ServiceLifetime));
                }
            }
        }
    }
}