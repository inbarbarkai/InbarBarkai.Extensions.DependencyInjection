using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using InbarBarkai.Extensions.DependencyInjection.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace InbarBarkai.Extensions.DependencyInjection
{
    internal class FactoryServiceDescriptorBuilder : ServiceDescriptorBuilder
    {
        private IConstrcutorFinder _constrcutorFinder;
        public IConstrcutorFinder ConstrcutorFinder
        {
            get => _constrcutorFinder;
            set
            {
                MakeSure.NotNull(value, nameof(value));
                _constrcutorFinder = value;
            }
        }

        public ICollection<ParameterResolver> ParameterResolvers { get; } = new HashSet<ParameterResolver>();

        public PropertyResolversMap PropertyResolvers { get; } = new PropertyResolversMap();

        public FactoryServiceDescriptorBuilder(IServiceDescriptorBuilder builder) : base(builder)
        {
            this.ConstrcutorFinder = new DefaultConstructorFinder();
        }

        public override IEnumerable<ServiceDescriptor> Build()
        {
            var factory = CreateFactoryExpression();

            if (this.ServiceTypes.Count == 0)
            {
                yield return ServiceDescriptor.Describe(this.ImplementationType, factory, this.ServiceLifetime);
            }
            else if (this.ServiceTypes.Count == 1)
            {
                yield return ServiceDescriptor.Describe(this.ServiceTypes.First(), factory, this.ServiceLifetime);
            }
            else
            {
                yield return ServiceDescriptor.Describe(this.ImplementationType, factory, this.ServiceLifetime);
                foreach (var serviceType in this.ServiceTypes)
                {
                    yield return ServiceDescriptor.Describe(serviceType, sp => sp.GetRequiredService(this.ImplementationType), this.ServiceLifetime);
                }
            }
        }

        private Func<IServiceProvider, object> CreateFactoryExpression()
        {
            var serviceProvider = Expression.Parameter(typeof(IServiceProvider), "ServiceProvider");
            var arguments = new List<ParameterExpression>();
            var body = new List<Expression>();
            var constructor = this.ConstrcutorFinder.Find(this.ImplementationType);
            foreach (var parameter in constructor.GetParameters())
            {
                CreateResolveParameterExpression(serviceProvider, arguments, body, parameter);
            }

            var initialization = CreateInitializationExpression(serviceProvider, constructor, arguments);
            body.Add(Expression.Convert(initialization, typeof(object)));
            var block = Expression.Block(arguments, body);
            var factory = Expression.Lambda<Func<IServiceProvider, object>>(block, serviceProvider);
            return factory.Compile();
        }

        private void CreateResolveParameterExpression(ParameterExpression serviceProvider, List<ParameterExpression> arguments, List<Expression> body, ParameterInfo parameter)
        {
            var customResolver = this.ParameterResolvers.FirstOrDefault(r => r.IsMatch(parameter));
            Expression value;
            if (customResolver != null)
            {
                value = Expression.Convert(
                    Expression.Invoke(customResolver.Resolve,
                                      serviceProvider,
                                      Expression.Constant(parameter)),
                    parameter.ParameterType);
            }
            else
            {
                var methodInfo = typeof(ServiceProviderServiceExtensions).GetMethod(nameof(ServiceProviderServiceExtensions.GetService), new[] { typeof(IServiceProvider) });
                value = Expression.Call(methodInfo.MakeGenericMethod(parameter.ParameterType), serviceProvider);
            }
            var argument = Expression.Parameter(parameter.ParameterType, parameter.Name);
            arguments.Add(argument);
            body.Add(Expression.Assign(argument, value));
        }

        private Expression CreateInitializationExpression(ParameterExpression serviceProvider, ConstructorInfo constructor, List<ParameterExpression> arguments)
        {
            var @new = Expression.New(constructor, arguments);
            if (this.PropertyResolvers.Count == 0)
            {
                return @new;
            }
            var memberAssignments = this.PropertyResolvers.Select(t => CreateResolvePropertyExperession(serviceProvider, t.Key, t.Value));
            var result = Expression.MemberInit(@new, memberAssignments);
            return result;
        }

        private MemberAssignment CreateResolvePropertyExperession(ParameterExpression serviceProvider, PropertyInfo propertyInfo, PropertyResolver propertyResolver)
        {
            var invoke = Expression.Convert(
                    Expression.Invoke(propertyResolver.Resolve,
                                      serviceProvider,
                                      Expression.Constant(propertyInfo)),
                    propertyInfo.PropertyType);
            return Expression.Bind(propertyInfo, invoke);
        }
    }
}
