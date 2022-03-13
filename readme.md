# InbarBarkai.Extensions.DependencyInjection
[![Package](https://github.com/inbarbarkai/InbarBarkai.Extensions.DependencyInjection/actions/workflows/dotnet.yml/badge.svg)](https://github.com/inbarbarkai/InbarBarkai.Extensions.DependencyInjection/actions/workflows/dotnet.yml)
## Description

This repository contains code to simplify complex service registrations with the `Microsoft.Extensions.DependencyInjection` IoC Containers.
The extensions allow a mix of implicit and explicit arguments resolvement for each registered service.
This is done by generating factory methods at runtime.
It also allows automatically register a service as all of its implemented interfaces.

## Benchmarks

|           Method |           Job |       Runtime | CurrentAdapter |     Mean |    Error |   StdDev |  Gen 0 | Allocated |
|----------------- |-------------- |-------------- |--------------- |---------:|---------:|---------:|-------:|----------:|
|   SingletonBasic |      .NET 6.0 |      .NET 6.0 |      CustomIoC | 27.49 ns | 0.602 ns | 0.670 ns |      - |         - |
| SingletonFactory |      .NET 6.0 |      .NET 6.0 |      CustomIoC | 26.86 ns | 0.569 ns | 0.532 ns |      - |         - |
|   TransientBasic |      .NET 6.0 |      .NET 6.0 |      CustomIoC | 30.96 ns | 0.565 ns | 0.528 ns | 0.0057 |      24 B |
| TransientFactory |      .NET 6.0 |      .NET 6.0 |      CustomIoC | 66.05 ns | 1.310 ns | 1.456 ns | 0.0134 |      56 B |
|   SingletonBasic | .NET Core 3.1 | .NET Core 3.1 |      CustomIoC | 32.37 ns | 0.582 ns | 0.545 ns |      - |         - |
| SingletonFactory | .NET Core 3.1 | .NET Core 3.1 |      CustomIoC | 31.32 ns | 0.572 ns | 0.535 ns |      - |         - |
|   TransientBasic | .NET Core 3.1 | .NET Core 3.1 |      CustomIoC | 36.28 ns | 0.768 ns | 1.150 ns | 0.0057 |      24 B |
| TransientFactory | .NET Core 3.1 | .NET Core 3.1 |      CustomIoC | 67.77 ns | 1.379 ns | 1.354 ns | 0.0076 |      32 B |
|   SingletonBasic |      .NET 6.0 |      .NET 6.0 |     DefaultIoC | 26.00 ns | 0.569 ns | 0.559 ns |      - |         - |
| SingletonFactory |      .NET 6.0 |      .NET 6.0 |     DefaultIoC | 28.23 ns | 0.442 ns | 0.413 ns |      - |         - |
|   TransientBasic |      .NET 6.0 |      .NET 6.0 |     DefaultIoC | 29.95 ns | 0.634 ns | 0.678 ns | 0.0057 |      24 B |
| TransientFactory |      .NET 6.0 |      .NET 6.0 |     DefaultIoC | 71.56 ns | 1.467 ns | 1.441 ns | 0.0134 |      56 B |
|   SingletonBasic | .NET Core 3.1 | .NET Core 3.1 |     DefaultIoC | 32.09 ns | 0.487 ns | 0.455 ns |      - |         - |
| SingletonFactory | .NET Core 3.1 | .NET Core 3.1 |     DefaultIoC | 33.57 ns | 0.425 ns | 0.398 ns |      - |         - |
|   TransientBasic | .NET Core 3.1 | .NET Core 3.1 |     DefaultIoC | 36.11 ns | 0.680 ns | 0.636 ns | 0.0057 |      24 B |
| TransientFactory | .NET Core 3.1 | .NET Core 3.1 |     DefaultIoC | 75.13 ns | 1.453 ns | 1.359 ns | 0.0076 |      32 B |