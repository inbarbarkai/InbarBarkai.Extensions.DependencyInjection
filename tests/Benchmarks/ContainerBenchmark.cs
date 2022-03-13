using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Benchmarks.Adapters;
using InbarBarkai.Extensions.DependencyInjection.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net60)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MemoryDiagnoser]
    [KeepBenchmarkFiles]
    public class ContainerBenchmark
    {
        public IEnumerable<IAdapter> Adapters()
        {
            yield return new CustomIoC();
            yield return new DefaultIoC();
        }

        [ParamsSource(nameof(Adapters))]
        public IAdapter CurrentAdapter { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            CurrentAdapter.Initialize();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            CurrentAdapter.Dispose();
        }

        [Benchmark]
        public ISingletonBasicService SingletonBasic()
            => CurrentAdapter.ServiceProvider.GetRequiredService<ISingletonBasicService>();

        [Benchmark]
        public ISingletonFactoryService SingletonFactory()
            => CurrentAdapter.ServiceProvider.GetRequiredService<ISingletonFactoryService>();

        [Benchmark]
        public ITransientBasicService TransientBasic()
            => CurrentAdapter.ServiceProvider.GetRequiredService<ITransientBasicService>();

        [Benchmark]
        public ITransientFactoryService TransientFactory()
            => CurrentAdapter.ServiceProvider.GetRequiredService<ITransientFactoryService>();
    }
}
