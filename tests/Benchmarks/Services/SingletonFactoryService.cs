namespace InbarBarkai.Extensions.DependencyInjection.Tests.Services
{
    public class SingletonFactoryService : ISingletonFactoryService
    {
        public int Integer { get; set; }
        public string String { get; }

        public SingletonFactoryService(int integer, string @string)
        {
            this.Integer = integer;
            this.String = @string;
        }
    }
}
