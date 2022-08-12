using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial;

[DataContract]
public readonly struct Speed : IEquatable<Speed>, IComparable<Speed>
{
    public const double DefaultCompareTolerance = 0.01;
    public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

    public static Speed Zero { get; } = new Speed(0);
    public static Speed FromMetersPerSecond(double metersPerSecond) => new (metersPerSecond);
    public static Speed FromKilometersPerHour(double kilometersPerHour) => new (kilometersPerHour / 3.6);
    public static Speed FromDistanceAndDuration(Distance distance, TimeSpan duration) =>
        duration.TotalSeconds > 0 ? new Speed(distance.Meters / duration.TotalSeconds) : Zero;

    private Speed(double metersPerSecond)
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

    public static bool operator ==(in Speed one, in Speed another) => one.Equals(another);
    public static bool operator !=(in Speed one, in Speed another) => !one.Equals(another);
    public static bool operator >(in Speed one, in Speed another) => one.CompareTo(another) == 1;
    public static bool operator <(in Speed one, in Speed another) => one.CompareTo(another) == -1;
    public static bool operator >=(in Speed one, in Speed another) => one.CompareTo(another) >= 0;
    public static bool operator <=(in Speed one, in Speed another) => one.CompareTo(another) <= 0;
    public static Speed operator +(in Speed one, in Speed another) => FromMetersPerSecond(one.MetersPerSecond + another.MetersPerSecond);

    public static bool operator >(in Speed speed, in double metersPerSecond) => speed > FromMetersPerSecond(metersPerSecond);
    public static bool operator <(in Speed speed, in double metersPerSecond) => speed < FromMetersPerSecond(metersPerSecond);
    public static bool operator >=(in Speed speed, in double metersPerSecond) => speed >= FromMetersPerSecond(metersPerSecond);
    public static bool operator <=(in Speed speed, in double metersPerSecond) => speed <= FromMetersPerSecond(metersPerSecond);


    public int CompareTo(Speed other) => 
        MetersPerSecond > other.MetersPerSecond && MetersPerSecond - other.MetersPerSecond > CompareTolerance ? 1 :
        other.MetersPerSecond > MetersPerSecond && other.MetersPerSecond - MetersPerSecond > CompareTolerance ? -1 :
        0;

    [Obsolete("Use + operator.")]
    public static Speed Add(Speed one, Speed another) => one + another;

    [Obsolete("Use < or <= operators.")] 
    public bool IsBelow(double metersPerSecond) => IsBelow(FromMetersPerSecond(metersPerSecond));

    [Obsolete("Use < or <= operators.")]
    public bool IsBelow(Speed other) => CompareTo(other)==-1 ;


    public bool Equals(Speed other) => CompareTo(other) == 0;
    public override bool Equals(object? obj) => obj is Speed speed && Equals(speed);
    public override string ToString() => $"{MetersPerSecond:F1}m/s";

    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => MetersPerSecond.GetHashCode();
}
