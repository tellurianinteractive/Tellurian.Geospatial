using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial;

[DataContract]
public readonly struct Distance : IEquatable<Distance>, IComparable<Distance>
{
    public const double DefaultCompareTolerance = 0.001;
    public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

    public static Distance Zero => FromMeters(0);
    public static Distance FromMeters(double meters) => new(meters);
    public static Distance FromKilometers(double kilometers) => new(kilometers * 1000);

    private Distance(double meters)
    {
        if (meters < 0) throw new ArgumentOutOfRangeException(nameof(meters), "A distance must be zero or positive.");
        Meters = meters;
    }

    [DataMember(Name = "Meters")]
    [JsonPropertyName("meters")]
    public double Meters { get; init; }
    [JsonIgnore]
    public double Kilometers => Meters / 1000;
    [JsonIgnore]
    public bool IsZero => Meters < CompareTolerance;

    public static bool operator ==(in Distance one, in Distance another) => one.Equals(another);
    public static bool operator !=(in Distance one, in Distance another) => !one.Equals(another);
    public static bool operator >(in Distance one, in Distance another) => one.CompareTo(another) == 1;
    public static bool operator >=(in Distance one, in Distance another) => one.CompareTo(another) >= 0;
    public static bool operator <(in Distance one, in Distance another) => one.CompareTo(another) == -1;
    public static bool operator <=(in Distance one, in Distance another) => one.CompareTo(another) <= 0;

    public static Distance operator +(in Distance one, in Distance another) => Add(one, another);
    public static Distance operator -(in Distance one, in Distance another) => Subtract(one, another);
    public static Distance operator -(in Distance one, in double value) => FromMeters(one.Meters - value > 0 ? value : 0);

    public bool Equals(Distance other) => CompareTo(other) == 0;
    public override bool Equals(object? obj) => obj is Distance d && Equals(d);
    public int CompareTo(Distance other) =>
            Meters > other.Meters && Meters - other.Meters > CompareTolerance ? 1 :
            other.Meters > Meters && other.Meters - Meters > CompareTolerance ? -1 :
            0;

    public override string ToString() => $"{Meters}m";


    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Meters.GetHashCode();

    public static Distance Add(Distance left, Distance right) => FromMeters(left.Meters + right.Meters);
    public static Distance Subtract(Distance left, Distance right) { var d = left.Meters - right.Meters; return FromMeters(d > 0 ? d : 0); }
}