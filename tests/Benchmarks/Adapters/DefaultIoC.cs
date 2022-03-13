using System;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks.Adapters
{
    public class DefaultIoC : IAdapter
    {
        private ServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider => _serviceProvider;

        public void Initialize()
        {
            var services = new ServiceCollection();
            services.AddSingleton("something");

            services.AddSingleton<ISingletonBasicService, SingletonBasicService>();
            services.AddSingleton<ISingletonFactoryService>(sp => new SingletonFactoryService(10, sp.GetRequiredService<string>()));

            services.AddTransient<ITransientBasicService, TransientBasicService>();
            services.AddTransient<ITransientFactoryService>(sp => new TransientFactoryService(10, sp.GetRequiredService<string>()));

            _serviceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }
    }
}
