﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial;

[DataContract]
public readonly struct Speed : IEquatable<Speed>, IComparable<Speed>
{
    private const double CompareTolerance = 0.01;
    public static Speed Zero { get; } = new Speed(0);
    public static Speed FromMetersPerSecond(double metersPerSecond) => new (metersPerSecond);
    public static Speed FromKilometersPerHour(double kilometersPerHour) => new (kilometersPerHour / 3.6);
    public static Speed FromDistanceAndDuration(Distance distance, TimeSpan duration) =>
        duration.TotalSeconds > 0 ? new Speed(distance.Meters / duration.TotalSeconds) : Zero;

    [JsonConstructor]
    public Speed(double metersPerSecond)
    {
        if (metersPerSecond < 0) throw new ArgumentOutOfRangeException(nameof(metersPerSecond), "Speed must be zero or positive.");
        MetersPerSecond = metersPerSecond;
    }
    [DataMember(Name = "MetersPerSecond")]
    [JsonPropertyName("metersPerSecond")]
    public double MetersPerSecond { get; init; }
    [JsonIgnore]
    public double KilometersPerHour => MetersPerSecond * 3.6;
    [JsonIgnore]
    public bool IsZero => MetersPerSecond < CompareTolerance;

    public bool IsBelow(double metersPerSecond) => IsBelow(FromMetersPerSecond(metersPerSecond));
    public bool IsBelow(Speed other) => this < other;
    public static bool operator ==(in Speed one, in Speed another) => one.Equals(another);
    public static bool operator !=(in Speed one, in Speed another) => !one.Equals(another);
    public static bool operator >(in Speed one, in Speed another) => one.MetersPerSecond > another.MetersPerSecond;
    public static bool operator <(in Speed one, in Speed another) => one.MetersPerSecond < another.MetersPerSecond;
    public static bool operator >=(in Speed one, in Speed another) => one.MetersPerSecond >= another.MetersPerSecond;
    public static bool operator <=(in Speed one, in Speed another) => one.MetersPerSecond <= another.MetersPerSecond;
    public static Speed operator +(in Speed one, in Speed another) => Speed.FromMetersPerSecond(one.MetersPerSecond + another.MetersPerSecond);
    public int CompareTo(Speed other) => MetersPerSecond < other.MetersPerSecond ? -1 : MetersPerSecond > other.MetersPerSecond ? 1 : 0;
    public static Speed Add(Speed one, Speed another) => one + another;

    public bool Equals(Speed other) => Math.Abs(MetersPerSecond - other.MetersPerSecond) <= CompareTolerance;
    public override bool Equals(object? obj) => obj is Speed speed && Equals(speed);
    public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0:F1}m/s", MetersPerSecond);

    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => MetersPerSecond.GetHashCode();
}
