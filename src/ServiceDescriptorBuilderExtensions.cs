using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods for <see cref="IServiceDescriptorBuilder"/>.
    /// </summary>
    public static class ServiceDescriptorBuilderExtensions
    {
        /// <summary>
        /// Sets the lifetime of the service as <see cref="ServiceLifetime.Transient"/>.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>The modified service descriptor builder.</returns>
        public static IServiceDescriptorBuilder InstancePerRequest(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Transient;
            return builder;
        }

        /// <summary>
        /// Sets the lifetime of the service as <see cref="ServiceLifetime.Scoped"/>.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>The modified service descriptor builder.</returns>
        public static IServiceDescriptorBuilder InstancePerScope(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Scoped;
            return builder;
        }

        /// <summary>
        /// Sets the lifetime of the service as <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>The modified service descriptor builder.</returns>
        public static IServiceDescriptorBuilder SingleInstance(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            builder.ServiceLifetime = ServiceLifetime.Singleton;
            return builder;
        }

        /// <summary>
        /// Configures the service to be registered as the provided type.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="serviceType">The type of the service to register as.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder As(this IServiceDescriptorBuilder builder, Type serviceType)
        {
            MakeSure.NotNull(builder, nameof(builder));
            MakeSure.NotNull(serviceType, nameof(serviceType));

            builder.ServiceTypes.Add(serviceType);

            return builder;
        }

        /// <summary>
        /// Configures the service to be registered as the provided type.
        /// </summary>
        /// <typeparam name="TServiceType">The type of the service to register as.</typeparam>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder As<TServiceType>(this IServiceDescriptorBuilder builder)
        {
            return builder.As(typeof(TServiceType));
        }

        /// <summary>
        /// Configures the service to be registered as its implemented interfaces.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder AsImplementedInterfaces(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            foreach (var @interface in builder.ImplementationType.GetInterfaces())
            {
                builder.ServiceTypes.Add(@interface);
            }
            return builder;
        }

        /// <summary>
        /// Configures the builder to resolve a constructor argument.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="isMatch">A predicate to define if the argument should be resolved.</param>
        /// <param name="resolve">A callback to resolve the value of the argument.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder WithParameter(this IServiceDescriptorBuilder builder, Predicate<ParameterInfo> isMatch, Expression<Func<IServiceProvider, ParameterInfo, object>> resolve)
        {
            MakeSure.NotNull(builder, nameof(builder));
            var factoryBuilder = builder as FactoryServiceDescriptorBuilder;

            if (factoryBuilder == null)
            {
                factoryBuilder = new FactoryServiceDescriptorBuilder(builder);
            }

            factoryBuilder.ParameterResolvers.Add(new ParameterResolver(isMatch, resolve));

            return factoryBuilder;
        }

        /// <summary>
        /// Configures the builder to resolve a constructor.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="isMatch">A predicate to define if the constructor should be used.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder WithConstructor(this IServiceDescriptorBuilder builder, Predicate<ConstructorInfo> isMatch)
        {
            MakeSure.NotNull(builder, nameof(builder));
            var factoryBuilder = builder as FactoryServiceDescriptorBuilder;

            if (factoryBuilder == null)
            {
                factoryBuilder = new FactoryServiceDescriptorBuilder(builder);
            }

            factoryBuilder.ConstrcutorFinder = new DelegateConstructorFinder(isMatch);

            return factoryBuilder;
        }

        /// <summary>
        /// Configures the builder to resolve a property.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="propertyName">The name of the property to resolve.</param>
        /// <param name="resolve">A callback to resolve the value of the argument.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        /// <exception cref="System.MissingMemberException">Could not find propery with name '{propertyName}'.</exception>
        public static IServiceDescriptorBuilder WithProperty(this IServiceDescriptorBuilder builder, string propertyName, Expression<Func<IServiceProvider, PropertyInfo, object>> resolve)
        {
            MakeSure.NotNull(builder, nameof(builder));
            MakeSure.NotNullOrWhiteSpace(propertyName, nameof(propertyName));

            var propertyInfo = builder.ImplementationType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new MissingMemberException($"Could not find propery with name '{propertyName}'.");
            }

            return builder.WithProperty(propertyInfo, resolve);
        }

        /// <summary>
        /// Configures the builder to resolve a property.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="propertyInfo">The the property to resolve.</param>
        /// <param name="resolve">A callback to resolve the value of the argument.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder WithProperty(this IServiceDescriptorBuilder builder, PropertyInfo propertyInfo, Expression<Func<IServiceProvider, PropertyInfo, object>> resolve)
        {
            MakeSure.NotNull(propertyInfo, nameof(propertyInfo));
            MakeSure.NotNull(resolve, nameof(resolve));

            var factoryBuilder = builder as FactoryServiceDescriptorBuilder;

            if (factoryBuilder == null)
            {
                factoryBuilder = new FactoryServiceDescriptorBuilder(builder);
            }

            factoryBuilder.PropertyResolvers.Add(propertyInfo, new PropertyResolver(resolve));

            return factoryBuilder;
        }

        /// <summary>
        /// Configures the builder to resolve all writeable properties.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <returns>
        /// The modified service descriptor builder.
        /// </returns>
        public static IServiceDescriptorBuilder AutoWireProperties(this IServiceDescriptorBuilder builder)
        {
            MakeSure.NotNull(builder, nameof(builder));
            var factoryBuilder = builder as FactoryServiceDescriptorBuilder;

            if (factoryBuilder == null)
            {
                factoryBuilder = new FactoryServiceDescriptorBuilder(builder);
            }

            foreach (var propertyInfo in factoryBuilder.ImplementationType.GetProperties().Where(p => p.CanWrite))
            {
                if (!factoryBuilder.PropertyResolvers.ContainsKey(propertyInfo))
                {
                    factoryBuilder.PropertyResolvers.Add(propertyInfo, new PropertyResolver((sp, pi) => sp.GetService(pi.PropertyType)));
                }
            }

            return factoryBuilder;
        }

        /// <summary>
        /// Builds the service descriptors and add them to to the specified service collection.
        /// </summary>
        /// <param name="builder">The service descriptor builder.</param>
        /// <param name="services">The service collection to add the descriptors to.</param>
        public static void BuildAndAddTo(this IServiceDescriptorBuilder builder, IServiceCollection services)
        {
            MakeSure.NotNull(builder, nameof(builder));
            MakeSure.NotNull(services, nameof(services));
            foreach (var item in builder.Build())
            {
                services.Add(item);
            }
        }
    }
}