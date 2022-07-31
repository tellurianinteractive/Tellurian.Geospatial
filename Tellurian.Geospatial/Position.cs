using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static System.Math;
using static Tellurian.Geospatial.Constants;

namespace Tellurian.Geospatial;

/// <summary>
/// Reporesents a coordintate using <see cref="Latitude"/> and <see cref="Longitude"/>.
/// </summary>
/// <remarks>
/// This implementation uses part of 'Latitude/longitude spherical geodesy tools'
/// MIT Licence (c) Chris Veness 2002-2019
/// https://www.movable-type.co.uk/scripts/latlong.html                      
/// https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html 
/// </remarks>
[DataContract]
public readonly struct Position : IEquatable<Position>
{
    private static readonly Position _Origo = FromDegrees(0, 0);
    public static Position Origo => _Origo;
    public static Position FromDegrees(double latitude, double longitude) => new(latitude, longitude);
    public static Position FromRadians(double latitude, double longitude) => new(latitude * 180 / Math.PI, longitude * 180 / Math.PI);

    private Position(in double latitude, in double longitude)
    {
        Latitude = Latitude.FromDegrees(latitude);
        Longitude = Longitude.FromDegrees(longitude);
    }

    [JsonConstructor]
    public Position(Latitude latitude, Longitude longitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    [DataMember(Name = "Latitude")]
    [JsonPropertyName("latitude")]
    public Latitude Latitude { get; init; }

    [DataMember(Name = "Longitude")]
    [JsonPropertyName("longitude")]
    public Longitude Longitude { get; init; }
    [JsonIgnore]
    public bool IsOrigo => Latitude.IsZero && Longitude.IsZero;

    public (double φ, double λ) RadianCoordinates => (Latitude.Radians, Longitude.Radians);

    /// <summary>
    /// Calculate the destina­tion point travelling along a (shortest distance) great circle arc.
    /// </summary>
    /// <param name="initialBearing"><see cref="Angle"/>Angle</param> of initial bearing.
    /// <param name="distance"><see cref="Distance"/></param> to destination.
    /// <returns><see cref="Position"/> for destination.</returns>
    /// <remarks>
    /// Formula from https://www.movable-type.co.uk/scripts/latlong.html
    /// </remarks>
    public Position Destination(in Angle initialBearing, in Distance distance)
    {
        const double R = EarthMeanRadiusMeters;
        var (φ, λ) = RadianCoordinates;
        var δ = initialBearing.Radians;
        var d = distance.Meters;

        var φ2 = Asin((Sin(φ) * Cos(d / R)) + (Cos(φ) * Sin(d / R) * Cos(δ)));
        var λ2 = λ + Atan2(Sin(δ) * Sin(d / R) * Cos(φ), Cos(d / R) - (Sin(φ) * Sin(φ2)));
        return FromRadians(φ2, λ2);
    }

    public bool IsBetween(Position before, Position after)
    {
        var s = Stretch.Between(before, after);
        var s1 = Stretch.Between(before, this);
        var s2 = Stretch.Between(after, this);
        var a1 = (s.Direction.To(s1.Direction));
        var a2 = (s.Direction.Reverse.To(s2.Direction));
        return a1.IsAcute && !a1.IsRight && a2.IsAcute && !a2.IsRight && !s1.Distance.IsZero && !s2.Distance.IsZero;
    }

    public static bool operator ==(in Position one, in Position another) => one.Equals(another);
    public static bool operator !=(in Position one, in Position another) => !one.Equals(another);
    public bool Equals(Position other) => Latitude == other.Latitude && Longitude == other.Longitude;
    public override bool Equals(object? obj) => obj is Position position && Equals(position);

    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0},{1}", Latitude.Degrees, Longitude.Degrees);

    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    internal (double latitude, double longitude) Radians => (Latitude.Radians, Longitude.Radians);
}
