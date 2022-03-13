using System;

namespace Benchmarks.Adapters
{
    public interface IAdapter : IDisposable
    {
        IServiceProvider ServiceProvider { get; }

        void Initialize();
    }
}
