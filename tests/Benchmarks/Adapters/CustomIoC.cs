using System;
using InbarBarkai.Extensions.DependencyInjection;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks.Adapters
{
    public sealed class CustomIoC : IAdapter
    {
        private ServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider => _serviceProvider;

        public void Initialize()
        {
            var services = new ServiceCollection();
            services.AddSingleton("something");

            ServiceDescriptorBuilder.Create<SingletonBasicService>()
                .SingleInstance()
                .AsImplementedInterfaces()
                .BuildAndAddTo(services);

            ServiceDescriptorBuilder.Create<SingletonFactoryService>()
                .SingleInstance()
                .AsImplementedInterfaces()
                .WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => 10)
                .BuildAndAddTo(services);

            ServiceDescriptorBuilder.Create<TransientBasicService>()
                .InstancePerRequest()
                .AsImplementedInterfaces()
                .BuildAndAddTo(services);

            ServiceDescriptorBuilder.Create<TransientFactoryService>()
                .InstancePerRequest()
                .AsImplementedInterfaces()
                .WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => 10)
                .BuildAndAddTo(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }
    }
}
