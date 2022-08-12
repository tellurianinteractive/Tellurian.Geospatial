using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Tellurian.Geospatial.Benchmarks;

#pragma warning disable CA1822 // Mark members as static does not work with benchmarks

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
public class DistanceBenchmarks
{
    private readonly Distance _distance1 = Distance.FromMeters(123.4);

    private readonly Distance _distance2 = Distance.FromMeters(234.5);
    [Benchmark]
    public Distance GetFromMeters() => Distance.FromMeters(123.4);

    [Benchmark]
    public Distance GetFromKilometers() => Distance.FromKilometers(0.1234);

    [Benchmark]
    public bool DistancesEqualsOther() => _distance1 == _distance2;

    [Benchmark]
    public Distance DistanceAddition() => _distance2 + _distance1;

    [Benchmark]
    public Distance DistanceSubtraction() => _distance2 - _distance1;

    [Benchmark]
    public string DistanceAsString() => _distance1.ToString();
}
