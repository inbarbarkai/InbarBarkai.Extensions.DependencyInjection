namespace InbarBarkai.Extensions.DependencyInjection.Tests.Services
{
    public class TransientFactoryService : ITransientFactoryService
    {
        public int Integer { get; set; }
        public string String { get; }

        public TransientFactoryService(int integer, string @string)
        {
            this.Integer = integer;
            this.String = @string;
        }
    }
}
