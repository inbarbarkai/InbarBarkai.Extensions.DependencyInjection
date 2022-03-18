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
        public void AddFactoryServiceSuccess(IServiceDescriptorBuilder builder, Type serviceType, object expectedObject)
        {
            var expected = (ServiceWithConstructorArguments)expectedObject;
            var services = new ServiceCollection()
                .AddSingleton(expected.String);
            builder.WithParameter(pi => pi.ParameterType == typeof(int), (sp, pi) => expected.Integer)
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

            instance1.Should().BeEquivalentTo(expected);
        }

        /// <summary>
        /// Gets the data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type, object)"/> test.
        /// </summary>
        /// <returns>The data for the <see cref="AddFactoryServiceSuccess(IServiceDescriptorBuilder, Type, object)"/> test.</returns>
        public static IEnumerable<object[]> AddFactoryServiceSuccessData()
        {
            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(10, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService2),
                   new ServiceWithConstructorArguments(10, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .As<ISimpleService1>(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(10, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest(),
                   typeof(ServiceWithConstructorArguments),
                   new ServiceWithConstructorArguments(10, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(0, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .AsImplementedInterfaces(),
                   typeof(ISimpleService2),
                   new ServiceWithConstructorArguments(0, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest()
                        .As<ISimpleService1>(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(0, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .WithConstructor(ci => ci.GetParameters().Length == 1)
                        .InstancePerRequest(),
                   typeof(ServiceWithConstructorArguments),
                   new ServiceWithConstructorArguments(0, "something")
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces()
                        .AutoWireProperties(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(10, "something")
                   {
                       SecondString = "something",
                       ThirdString = "something"
                   }
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces()
                        .WithProperty(nameof(ServiceWithConstructorArguments.ThirdString), (sp,pi) => "else")
                        .AutoWireProperties(),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(10, "something")
                   {
                       SecondString = "something",
                       ThirdString = "else"
                   }
            };

            yield return new object[]
            {
                   ServiceDescriptorBuilder.Create<ServiceWithConstructorArguments>()
                        .InstancePerRequest()
                        .AsImplementedInterfaces()
                        .WithProperty(nameof(ServiceWithConstructorArguments.ThirdString), (sp,pi) => "else"),
                   typeof(ISimpleService1),
                   new ServiceWithConstructorArguments(10, "something")
                   {
                       ThirdString = "else"
                   }
            };
        }

        /// <summary>
        /// Tests adding service with generated factory method with failure due to missing property.
        /// </summary>
        [Fact]
        public void AddFactoryServiceMissingMember()
        {
            Action action = () => ServiceDescriptorBuilder.Create(typeof(SimpleService)).WithProperty("A", (sp, pi) => 190);

            action.Should().Throw<MissingMemberException>();
        }

        /// <summary>
        /// Tests adding service with generated factory method with failure due to a not writeable property.
        /// </summary>
        [Fact]
        public void AddFactoryServiceNotWriteableProperty()
        {
            Action action = () => ServiceDescriptorBuilder.Create(typeof(ServiceWithConstructorArguments)).WithProperty(nameof(ServiceWithConstructorArguments.Integer), (sp, pi) => 190);

            action.Should().Throw<InvalidOperationException>();
        }
    }
}
