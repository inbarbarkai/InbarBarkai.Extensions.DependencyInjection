namespace InbarBarkai.Extensions.DependencyInjection.Tests.Services
{
    internal class ServiceWithConstructorArguments : ISimpleService1, ISimpleService2
    {
        public int Integer { get; set; }
        public string String { get; }

        public ServiceWithConstructorArguments(int integer, string @string)
        {
            this.Integer = integer;
            this.String = @string;
        }

        public ServiceWithConstructorArguments(string @string)
        {
            this.String = @string;
        }
    }
}
