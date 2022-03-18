namespace InbarBarkai.Extensions.DependencyInjection.Tests.Services
{
    internal class ServiceWithConstructorArguments : ISimpleService1, ISimpleService2
    {
        public int Integer { get; }

        public string String { get; }

        public string SecondString { get; set; }

        public string ThirdString { get; set; }

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
