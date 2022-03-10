namespace InbarBarkai.Extensions.DependencyInjection.Tests.Services
{
    internal class ServiceWithConstructorArguments : ISimpleService1
    {
        public int Integer { get; set; }

        public ServiceWithConstructorArguments(int integer)
        {
            this.Integer = integer;
        }
    }
}
