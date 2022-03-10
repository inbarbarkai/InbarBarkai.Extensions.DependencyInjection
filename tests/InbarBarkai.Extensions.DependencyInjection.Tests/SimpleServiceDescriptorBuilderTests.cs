using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;

namespace InbarBarkai.Extensions.DependencyInjection.Tests
{
    public class SimpleServiceDescriptorBuilderTests
    {
        [Fact]
        public void AddSimpleServiceTransientSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .InstancePerRequest()
                .AddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            var instance1 = serviceProvider.GetService<SimpleService>();
            var instance2 = serviceProvider.GetService<SimpleService>();

            instance1.Should()
                .NotBeNull();

            instance2.Should()
                .NotBeNull();

            instance1.Should().NotBe(instance2);
        }

        [Fact]
        public void AddSimpleServiceScopedSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .InstancePerScope()
                .AddTo(services);

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

        [Fact]
        public void AddSimpleServiceSingletonSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .SingleInstance()
                .AddTo(services);

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

        [Fact]
        public void AddSimpleServiceAsInterfaceSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .As<ISimpleService>()
                .AddTo(services);

            using var serviceProvider = services.BuildServiceProvider();
                      
            serviceProvider.GetService<SimpleService>()
                .Should()
                .BeNull();

            var instance = serviceProvider.GetService<ISimpleService>();

            instance.Should()
                .NotBeNull();

            instance.Should()
                .BeOfType<SimpleService>();
        }

        [MemberData(nameof(AddSimpleServiceAsImplementedInterfacesSuccessData))]
        [Theory]
        public void AddSimpleServiceAsImplementedInterfacesSuccess(IServiceDescriptorBuilder builder)
        {
            var services = new ServiceCollection();
            
            builder.ServiceTypes.Count.Should().Be(1);
            builder.AddTo(services);

            using var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<SimpleService>()
                .Should()
                .BeNull();

            var instance = serviceProvider.GetService<ISimpleService>();

            instance.Should()
                .NotBeNull();

            instance.Should()
                .BeOfType<SimpleService>();
        }

        public static IEnumerable<object[]> AddSimpleServiceAsImplementedInterfacesSuccessData()
        {
            yield return new object[] 
            { 
                ServiceDescriptorBuilder.Create<SimpleService>()
                    .AsImplementedInterfaces()
            };

            yield return new object[] 
            {
                ServiceDescriptorBuilder.Create<SimpleService>()
                    .As<ISimpleService>()
                    .AsImplementedInterfaces()
            };
        }
    }
}