using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial;

[DataContract]
public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
{
    public const double DefaultCompareTolerance = 0.00001;
    public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

    public static Angle FromDegrees(double degrees) => new (degrees, 0, 360);
    public static Angle FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);
    public static Angle Zero => FromDegrees(0);
    public static Angle Undefined => new (double.NaN, 0, 0);
    public static Angle Right => FromDegrees(90);
    public static Angle Straight => FromDegrees(180);

    private Angle(in double degrees, double min, double max)
    {
        if (double.IsNaN(degrees)) Degrees = double.NaN;
        else if (degrees < min || degrees >= max) throw new ArgumentOutOfRangeException(nameof(degrees));
        Degrees = degrees;
    }

    [DataMember(Name = "Degrees")]
    [JsonPropertyName("degrees")]
    public double Degrees { get; init; }

    [JsonIgnore]
    public double Radians => IsUndefined ? double.NaN : Degrees * Math.PI / 180;
    [JsonIgnore]
    public Angle Complement => IsUndefined ? Undefined : Degrees == 0 ? FromDegrees(0) : FromDegrees(360 - Degrees);
    [JsonIgnore]
    public Angle Reverse => IsUndefined ? Undefined : FromDegrees(Degrees >= 180 ? Degrees - 180 : Degrees + 180);
    [JsonIgnore]
    public bool IsAcute => Degrees < 90.0 && !IsUndefined;
    [JsonIgnore]
    public bool IsObtuse { get { if (IsUndefined) return false; var a = Degrees < 180 ? Degrees : Degrees - 180; return a > 90.0 && a < 180.0; } }
    [JsonIgnore]
    public bool IsRight => (Math.Abs(Degrees - 90.0) < CompareTolerance || Math.Abs(Degrees - 270.0) < CompareTolerance) && !IsUndefined;
    [JsonIgnore]
    public bool IsStraight => Math.Abs(Degrees - 180.0) < CompareTolerance && !IsUndefined;
    [JsonIgnore]
    public bool IsUndefined => double.IsNaN(Degrees);

    /// <summary>
    /// Calculates the smallest angle between this angle and another angle.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>The <see cref="Angle"/> between this and other.
    ///</returns>
    public Angle To(Angle other) => FromDegrees(Math.Abs(Degrees - other.Degrees));

    public static bool operator ==(in Angle one, in Angle another) => one.Equals(another);
    public static bool operator !=(in Angle one, in Angle another) => !one.Equals(another);
    public static bool operator >(in Angle one, in Angle another) => one.CompareTo(another) == 1;
    public static bool operator >=(in Angle one, in Angle another) => one.CompareTo(another) >= 0;
    public static bool operator <(in Angle one, in Angle another) => one.CompareTo(another) == -1;
    public static bool operator <=(in Angle one, in Angle another) => one.CompareTo(another) <= 0; 


    public static bool operator >(in Angle angle, in double degrees) => angle > FromDegrees(degrees);
    public static bool operator <(in Angle angle, in double degrees) => angle < FromDegrees(degrees);
    public static bool operator <=(in Angle angle, in double degrees) => angle <= FromDegrees(degrees);
    public static bool operator >=(in Angle angle, in double degrees) => angle >= FromDegrees(degrees);

    public static Angle operator -(in Angle one, in Angle another)
    {
        if (one.IsUndefined || another.IsUndefined) return Undefined;
        var d = one.Degrees - another.Degrees; return FromDegrees(d >= 0 ? d : d + 360.0);
    }
    public static Angle operator +(in Angle one, in Angle another)
    {
        if (one.IsUndefined || another.IsUndefined) return Undefined;
        var d = one.Degrees + another.Degrees; return FromDegrees(d < 360.0 ? d : d - 360.0);
    }

    public int CompareTo(Angle other) =>
        IsUndefined && other.IsUndefined ? 0 :
        IsUndefined && !other.IsUndefined ? -1 :
        !IsUndefined && other.IsUndefined ? 1 :
        Degrees > other.Degrees && Degrees - other.Degrees > CompareTolerance ? 1 :
        Degrees < other.Degrees && other.Degrees -Degrees > CompareTolerance ? -1 :
        0;

    public static Angle Add(Angle one, Angle another) => one + another;
    public static Angle Subtract(Angle one, Angle another) => one - another;

    /// <summary>
    /// Calculates the mimimum angle of this and other angle.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>The minimum <see cref="Angle"/> of this or other</returns>
    public Angle Min(in Angle other)
    {
        var a1 = this - other;
        var a2 = other - this;
        return a1 < a2 ? a1 : a2;
    }

    public override string ToString() => IsUndefined ? "Undefined" : $"{Degrees:F2}°";
    public override bool Equals(object? obj) => obj is Angle angle && Equals(angle);
    public bool Equals(Angle other) => CompareTo(other) == 0;

    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Degrees.GetHashCode();
}
