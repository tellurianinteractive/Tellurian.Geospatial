using BenchmarkDotNet.Attributes;

namespace Tellurian.Geospatial.Benchmarks;

public class StretchBenchmarks
{
    private readonly Stretch Stretch = Stretch.Between(Position.FromDegrees(58.1, 10.9), Position.FromDegrees(59.1, 9.9));

    [Benchmark]
    public Distance Distance() => Stretch.Distance;

    [Benchmark]
    public Angle Bearing() => Stretch.Direction;

    [Benchmark]
    public Angle InitialBearing() => Stretch.InitialBearing;

    [Benchmark]
    public Angle FinalBearing() => Stretch.FinalBearing;

    [Benchmark]
    public Distance CrossTrackDistance() => Stretch.CrossTrackDistance(Position.FromDegrees(58.5, 11.0));

    [Benchmark]
    public Distance OnTrackDistance() => Stretch.OnTrackDistance(Position.FromDegrees(58.5, 11.0));

    [Benchmark]
    public bool IsOnTrack() => Stretch.IsOnTrack(Position.FromDegrees(58.5, 11.0), Geospatial.Distance.FromKilometers(1));

    [Benchmark]
    public Stretch Inverse() => Stretch.Inverse;
}
