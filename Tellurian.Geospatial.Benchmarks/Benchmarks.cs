using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Tellurian.Geospatial.Benchmarks;

#pragma warning disable CA1822 // Mark members as static does not work with benchmarks

[MemoryDiagnoser()]
[SimpleJob(RuntimeMoniker.Net80, baseline: true)]
[SimpleJob(RuntimeMoniker.Net90)]

public class Benchmarks
{
    private readonly Position _position = Position.FromDegrees(58.0, -22.1);
    private readonly Distance _distance = Geospatial.Distance.FromKilometers(1);

    /* Position */
    private readonly Position _before = Position.FromDegrees(57.0, -22.1);
    private readonly Position _after = Position.FromDegrees(59.0, -22.1);
    private readonly Angle _bearing = Angle.FromDegrees(15.0);

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

    /* Distance */

    private readonly Distance _distance1 = Geospatial.Distance.FromMeters(123.4);

    private readonly Distance _distance2 = Geospatial.Distance.FromMeters(234.5);
    [Benchmark]
    public Distance GetFromMeters() => Geospatial.Distance.FromMeters(123.4);

    [Benchmark]
    public Distance GetFromKilometers() => Geospatial.Distance.FromKilometers(0.1234);

    [Benchmark]
    public bool DistancesEqualsOther() => _distance1 == _distance2;

    [Benchmark]
    public Distance DistanceAddition() => _distance2 + _distance1;

    [Benchmark]
    public Distance DistanceSubtraction() => _distance2 - _distance1;

    [Benchmark]
    public string DistanceAsString() => _distance1.ToString();

    /* Stretch */

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
