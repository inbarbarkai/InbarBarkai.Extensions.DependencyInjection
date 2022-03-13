using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System;

namespace InbarBarkai.Extensions.DependencyInjection.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="SimpleServiceDescriptorBuilder"/>.
    /// </summary>
    public class SimpleServiceDescriptorBuilderTests
    {
        /// <summary>
        /// Tests adding transient service with successful outcome.
        /// </summary>
        [Fact]
        public void AddSimpleServiceTransientSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .InstancePerRequest()
                .BuildAndAddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            var instance1 = serviceProvider.GetService<SimpleService>();
            var instance2 = serviceProvider.GetService<SimpleService>();

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().NotBe(instance2);
        }

        /// <summary>
        /// Tests adding scoped service with successful outcome.
        /// </summary>
        [Fact]
        public void AddSimpleServiceScopedSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .InstancePerScope()
                .BuildAndAddTo(services);

            using var serviceProvider = services.BuildServiceProvider();
            using var scope1 = serviceProvider.CreateScope();
            using var scope2 = serviceProvider.CreateScope();

            var instance1 = scope1.ServiceProvider.GetService<SimpleService>();
            var instance2 = scope2.ServiceProvider.GetService<SimpleService>();

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().NotBe(instance2);
            scope1.ServiceProvider
                .GetService<SimpleService>()
                .Should()
                .Be(instance1);
        }

        /// <summary>
        /// Tests adding singleton service with successful outcome.
        /// </summary>
        [Fact]
        public void AddSimpleServiceSingletonSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .SingleInstance()
                .BuildAndAddTo(services);

            using var serviceProvider = services.BuildServiceProvider();
            using var scope1 = serviceProvider.CreateScope();
            using var scope2 = serviceProvider.CreateScope();

            var instance1 = scope1.ServiceProvider.GetService<SimpleService>();
            var instance2 = scope2.ServiceProvider.GetService<SimpleService>();

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().Be(instance2);
            scope1.ServiceProvider
                .GetService<SimpleService>()
                .Should()
                .Be(instance1);
        }

        /// <summary>
        /// Tests adding service as an interface with successful outcome.
        /// </summary>
        [Fact]
        public void AddSimpleServiceAsInterfaceSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .As<ISimpleService1>()
                .BuildAndAddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<SimpleService>()
                .Should()
                .BeNull();

            var instance = serviceProvider.GetService<ISimpleService1>();

            instance.Should()
                .NotBeNull();

            instance.Should()
                .BeOfType<SimpleService>();
        }


        /// <summary>
        /// Tests adding service as an interface with exception due to incompatibility.
        /// </summary>
        [Fact]
        public void AddSimpleServiceAsInterfaceInvalidOperationExcetpion()
        {
            Action action = () => ServiceDescriptorBuilder.Create<SimpleService>()
                .As<ServiceWithConstructorArguments>();

            action.Should().Throw<InvalidOperationException>();
        }

        /// <summary>
        /// Tests adding service as its implemented interfaces with successful outcome.
        /// </summary>
        [MemberData(nameof(AddSimpleServiceAsImplementedInterfacesSuccessData))]
        [Theory]
        public void AddSimpleServiceAsImplementedInterfacesSuccess(IServiceDescriptorBuilder builder)
        {
            var services = new ServiceCollection();

            builder.ServiceTypes.Count.Should().Be(2);
            builder.BuildAndAddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            var instance1 = serviceProvider.GetService<ISimpleService1>();
            var instance2 = serviceProvider.GetService<ISimpleService2>();

            instance1.Should()
                .NotBeNull();
            instance2.Should()
               .NotBeNull();

            instance1.Should()
                .BeOfType<SimpleService>();
            instance2.Should()
                .BeOfType<SimpleService>();

            instance1.Should().Be(instance2);
        }

        /// <summary>
        /// Gets the data for the <see cref="AddSimpleServiceAsImplementedInterfacesSuccess(IServiceDescriptorBuilder)"/> test.
        /// </summary>
        /// <returns>The data for the <see cref="AddSimpleServiceAsImplementedInterfacesSuccess(IServiceDescriptorBuilder)"/> test.</returns>
        public static IEnumerable<object[]> AddSimpleServiceAsImplementedInterfacesSuccessData()
        {
            yield return new object[]
            {
                ServiceDescriptorBuilder.Create<SimpleService>()
                    .SingleInstance()
                    .AsImplementedInterfaces()
            };

            yield return new object[]
            {
                ServiceDescriptorBuilder.Create<SimpleService>()
                    .SingleInstance()
                    .As<ISimpleService1>()
                    .AsImplementedInterfaces()
            };
        }

        /// <summary>
        /// Tests creating a service descriptor with <see cref="NullReferenceException"/> due to null implementation type.
        /// </summary>
        [Fact]
        public void CreateNullReferenceException()
        {
            Action action = () => ServiceDescriptorBuilder.Create(null);

            action.Should().Throw<ArgumentNullException>();
        }       
    }
}