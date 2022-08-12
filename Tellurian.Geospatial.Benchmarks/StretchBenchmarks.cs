using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Tellurian.Geospatial.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
public class StretchBenchmarks
{

    private readonly Position _position = Position.FromDegrees(58.5, 11.0);
    private readonly Distance _distance = Geospatial.Distance.FromKilometers(1);
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
    public Distance CrossTrackDistance() => Stretch.CrossTrackDistance(_position);

    [Benchmark]
    public Distance OnTrackDistance() => Stretch.OnTrackDistance(_position);

    [Benchmark]
    public bool IsOnTrack() => Stretch.IsOnTrack(_position, _distance);

    [Benchmark]
    public Stretch Inverse() => Stretch.Inverse;
}
