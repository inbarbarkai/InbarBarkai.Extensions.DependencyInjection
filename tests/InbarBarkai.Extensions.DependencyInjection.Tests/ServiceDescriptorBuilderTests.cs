using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace InbarBarkai.Extensions.DependencyInjection.Tests
{
    public class ServiceDescriptorBuilderTests
    {
        [Fact]
        public void AddSimpleServiceTransientSuccess()
        {
            var services = new ServiceCollection();
            ServiceDescriptorBuilder.Create<SimpleService>()
                .AddTo(services);

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var instance1 = serviceProvider.GetService<SimpleService>();
                var instance2 = serviceProvider.GetService<SimpleService>(); 
                    
                instance1.Should()
                    .NotBeNull();

                instance2.Should()
                    .NotBeNull();

                instance1.Should().NotBe(instance2);
            }
        }
    }
}