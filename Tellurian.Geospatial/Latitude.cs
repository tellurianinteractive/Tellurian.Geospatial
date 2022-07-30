using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial;

[DataContract]
public readonly struct Latitude : IEquatable<Latitude>, IComparable<Latitude>
{
    public const double DefaultCompareTolerance = 0.00001;
    public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

    public static Latitude FromDegrees(double degrees) => new (degrees, -90, 90);
    public static Latitude FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

    private Latitude(in double degrees, double min, double max)
    {
        if (degrees < min || degrees > max) throw new ArgumentOutOfRangeException(nameof(degrees));
        Degrees = degrees;
    }
    [JsonConstructor]
    public Latitude(double degrees) : this(degrees, -90, 90) { }

    [DataMember(Name = "Degrees")]
    [JsonPropertyName("degrees")]
    public double Degrees { get; init; }
    [JsonIgnore]
    public double Radians => Degrees * Math.PI / 180;
    [JsonIgnore]
    public bool IsZero => Math.Abs(Degrees) < CompareTolerance;

    public static bool operator ==(in Latitude one, in Latitude another) => one.Equals(another);
    public static bool operator !=(in Latitude one, in Latitude another) => !one.Equals(another);
    public static bool operator >(in Latitude one, in Latitude another) => one.CompareTo(another) == 1;
    public static bool operator <(in Latitude one, in Latitude another) => one.CompareTo(another) == -1;
    public static bool operator >=(in Latitude one, in Latitude another) => one.CompareTo(another) >= 0;
    public static bool operator <=(in Latitude one, in Latitude another) => one.CompareTo(another) <= 0;

    public int CompareTo(Latitude other) =>
        (Degrees < other.Degrees) && other.Degrees - Degrees > CompareTolerance ? -1 :
        ((Degrees > other.Degrees) && Degrees - other.Degrees > CompareTolerance ? 1 : 0);

    public bool Equals(Latitude other) => CompareTo(other) == 0;
    public override bool Equals(object? obj) => obj is Latitude longitude && Equals(longitude);

    public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);

    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Degrees.GetHashCode();
}
