using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Tellurian.Geospatial.Benchmarks;

#pragma warning disable CA1822 // Mark members as static does not work with benchmarks

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
public class PositionBenchmarks
{

    private readonly Position _position = Position.FromDegrees(58.0, -22.1);
    private readonly Position _before = Position.FromDegrees(57.0, -22.1);
    private readonly Position _after = Position.FromDegrees(59.0, -22.1);
    private readonly Angle _bearing = Angle.FromDegrees(15.0);
    private readonly Distance _distance = Geospatial.Distance.FromKilometers(1);

    [Benchmark]
    public Position GetPositionFromDegrees() => Position.FromDegrees(58.0, -22.1);

    [Benchmark]
    public Position GetPositionFromRadians() => Position.FromRadians(0.75, -0.55);

    [Benchmark]
    public (double, double) PositionAsRadianCoordinates() => _position.RadianCoordinates;

    [Benchmark]
    public Position GetDestination() => _position.Destination(_bearing, _distance);

    [Benchmark]
    public bool PositionIsBetween() => _position.IsBetween(_before, _after);

    [Benchmark]
    public bool PositionsAreEqual() => _position == _before;
}
