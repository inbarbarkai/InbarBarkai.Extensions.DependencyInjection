using System;
using System.Linq.Expressions;
using System.Reflection;
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

        public static IServiceDescriptorBuilder As(this IServiceDescriptorBuilder builder, Type serviceType)
        {
            MakeSure.NotNull(builder, nameof(builder));
            MakeSure.NotNull(serviceType, nameof(serviceType));

            if (serviceType.IsAssignableFrom(builder.ImplementationType))
            {
                builder.ServiceTypes.Add(serviceType);

                return builder;
            }

            throw new InvalidOperationException($"The type '{builder.ImplementationType.FullName}' cannot be assigned to '{serviceType.FullName}'");
        }

        public static IServiceDescriptorBuilder As<TServiceType>(this IServiceDescriptorBuilder builder)
        {
            return builder.As(typeof(TServiceType));
        }

        public static IServiceDescriptorBuilder AsImplementedInterfaces(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            foreach (var @interface in builder.ImplementationType.GetInterfaces())
            {
                builder.ServiceTypes.Add(@interface);
            }
            return builder;
        }

        public static IServiceDescriptorBuilder WithParameter(this IServiceDescriptorBuilder builder, Func<ParameterInfo, bool> isMatch, Expression<Func<IServiceProvider, ParameterInfo, object>> resolve)
        {
            var factoryBuilder = builder as FactoryServiceDescriptorBuilder;

            if (factoryBuilder == null)
            {
                factoryBuilder = new FactoryServiceDescriptorBuilder(builder);
            }

            factoryBuilder.ParameterResolvers.Add(new ParameterResolver(isMatch, resolve));

            return factoryBuilder;
        }
    }
}