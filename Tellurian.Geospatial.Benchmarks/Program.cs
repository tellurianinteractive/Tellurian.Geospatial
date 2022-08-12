using BenchmarkDotNet.Running;
using Tellurian.Geospatial.Benchmarks;

BenchmarkRunner.Run<DistanceBenchmarks>();
BenchmarkRunner.Run<PositionBenchmarks>();
BenchmarkRunner.Run<StretchBenchmarks>();

