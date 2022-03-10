using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests
{
    public class FactoryServiceDescriptorBuilderTests
    {
        [Fact]
        public void AddFactoryServiceSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                .InstancePerRequest()
                .AsImplementedInterfaces()
                .WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => 10)
                .AddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            var instance1 = serviceProvider.GetService<ISimpleService1>();
            var instance2 = serviceProvider.GetService<ISimpleService1>();

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().NotBe(instance2);
            instance1.Should().BeOfType<ServiceWithConstructorArguments>();
            instance2.Should().BeOfType<ServiceWithConstructorArguments>();
        }
    }
}
