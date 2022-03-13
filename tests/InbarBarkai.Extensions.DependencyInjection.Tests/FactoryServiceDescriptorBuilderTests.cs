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
        public void AddFactoryServiceSuccess(IServiceDescriptorBuilder builder, Type serviceType, string expectedString, int expectedInteger)
        {
            var services = new ServiceCollection()
                .AddSingleton(expectedString);
            builder.WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => expectedInteger)
                .BuildAndAddTo(services);

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

            ((ServiceWithConstructorArguments)instance1).Integer.Should().Be(expectedInteger);
            ((ServiceWithConstructorArguments)instance1).String.Should().Be(expectedString);
        }

        /// <summary>
        /// Gets the data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type, string, int)"/> test.
        /// </summary>
        /// <returns>The data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type, string, int)"/> test.</returns>
        public static IEnumerable<object[]> AddFactoryServiceSuccessData()
        {
            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService1),
                   "something",
                   10
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService2),
                   "something",
                   10
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .As<ISimpleService1>(),
                   typeof(ISimpleService1),
                   "something",
                   10
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest(),
                   typeof(ServiceWithConstructorArguments),
                   "something",
                   10
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService1),
                   "something",
                   0
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService2),
                   "something",
                   0
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .As<ISimpleService1>(),
                   typeof(ISimpleService1),
                   "something",
                   0
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest(),
                   typeof(ServiceWithConstructorArguments),
                   "something",
                   0
            };
        }
    }
}
