using System;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    public static class ServiceDescriptorBuilderExtensions
    {
        public static IServiceDescriptorBuilder InstancePerRequest(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Transient;
            return builder;
        }

        public static IServiceDescriptorBuilder InstancePerScope(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Scoped;
            return builder;
        }

        public static IServiceDescriptorBuilder SingleInstance(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Singleton;
            return builder;
        }
    }
}