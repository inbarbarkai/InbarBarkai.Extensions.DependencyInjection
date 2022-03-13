using System;
using System.Collections.Generic;
using FluentAssertions;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InbarBarkai.Extensions.DependencyInjection.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="FactoryServiceDescriptorBuilder"/>.
    /// </summary>
    public class FactoryServiceDescriptorBuilderTests
    {
        /// <summary>
        /// Tests adding service with generated factory method with successful outcome.
        /// </summary>
        [MemberData(nameof(AddFactoryServiceSuccessData))]
        [Theory]
        public void AddFactoryServiceSuccess(IServiceDescriptorBuilder builder, Type serviceType)
        {
            var services = new ServiceCollection()
                .AddSingleton("something");
            builder.WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => 10)
                .AddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            var instance1 = serviceProvider.GetService(serviceType);
            var instance2 = serviceProvider.GetService(serviceType);

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().NotBe(instance2);
            instance1.Should().BeOfType<ServiceWithConstructorArguments>();
            instance2.Should().BeOfType<ServiceWithConstructorArguments>();

            ((ServiceWithConstructorArguments)instance1).Integer.Should().Be(10);
            ((ServiceWithConstructorArguments)instance1).String.Should().Be("something");
        }

        /// <summary>
        /// Gets the data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type)"/> test.
        /// </summary>
        /// <returns>The data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type)"/> test.</returns>
        public static IEnumerable<object[]> AddFactoryServiceSuccessData()
        {
            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService1)
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService2)
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .As<ISimpleService1>(),
                   typeof(ISimpleService1)
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest(),
                   typeof(ServiceWithConstructorArguments)
            };
        }
    }
}
