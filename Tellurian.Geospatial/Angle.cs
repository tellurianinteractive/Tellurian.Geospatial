using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static Tellurian.Geospatial.Constants;

namespace Tellurian.Geospatial;

/// <summary>
/// Represents an angle equal to or greater than 0 and less than 360 degrees.
/// </summary>
[DataContract]
public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
{
    /// <summary>
    /// The default value for <see cref="CompareTolerance"/>.
    /// </summary>
    public const double DefaultCompareTolerance = 0.00001;

    /// <summary>
    /// If the differens between the <see cref="Degrees"/> are less than this value, 
    /// the angles are considered equal; otherwise not.
    /// </summary>
    public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

    /// <summary>
    /// Creates an <see cref="Angle"/> from a degrees value.
    /// </summary>
    /// <param name="degrees">A value greater or equal to 0 and less than 360.</param>
    /// <returns></returns>
    public static Angle FromDegrees(double degrees) => new(degrees, 0, 360);

    /// <summary>
    /// Creates an <see cref="Angle"/> from a radians value.
    /// </summary>
    /// <param name="radians"> A value equal or greater than 0 and and less than 2Π.</param>
    /// <returns></returns>
    public static Angle FromRadians(double radians) => FromDegrees(radians / Πover180);

    /// <summary>
    /// Creates an <see cref="Angle"/> of zero degrees/radians.
    /// </summary>
    public static Angle Zero => FromDegrees(0);

    /// <summary>
    /// Creates an <see cref="Angle"/> where <see cref="Degrees"/> and <see cref="Radians"/> are <see cref="double.NaN"/>.
    /// </summary>
    public static Angle Undefined => new(double.NaN, 0, 0);

    /// <summary>
    /// Creates an <see cref="Angle"/> where <see cref="Degrees"/> is 90º and <see cref="Radians"/> Π/2.
    /// </summary>
    public static Angle Right => FromDegrees(90);

    /// <summary>
    /// Creates an <see cref="Angle"/> where <see cref="Degrees"/> is 180º and <see cref="Radians"/> Π.
    /// </summary>
    public static Angle Straight => FromDegrees(180);

    private Angle(in double degrees, double min, double max)
    {
        if (double.IsNaN(degrees)) Degrees = double.NaN;
        else if (degrees < min || degrees >= max) throw new ArgumentOutOfRangeException(nameof(degrees));
        Degrees = degrees;
    }

    /// <summary>
    /// The <see cref="Angle"/> as degrees or undefined as <see cref="double.NaN"/>.
    /// </summary>
    [DataMember(Name = "Degrees")]
    [JsonPropertyName("degrees")]
    public double Degrees { get; init; }

    /// <summary>
    /// The <see cref="Angle"/> as radians or undefined as <see cref="double.NaN"/>.
    /// </summary>
    [JsonIgnore]
    public double Radians => IsUndefined ? double.NaN : Degrees * Πover180;

    /// <summary>
    /// A new <see cref="Angle"/> that is complement to this.
    /// A complement to an <see cref="Undefined"/> angle is undefined.
    /// </summary>
    [JsonIgnore]
    public Angle Complement => IsUndefined ? Undefined : Degrees == 0 ? FromDegrees(0) : FromDegrees(360 - Degrees);

    /// <summary>
    /// A new <see cref="Angle"/> that is reverse to this.
    /// A reverse to an <see cref="Undefined"/> angle is undefined.
    /// </summary>
    [JsonIgnore]
    public Angle Reverse => IsUndefined ? Undefined : FromDegrees(Degrees >= 180 ? Degrees - 180 : Degrees + 180);


    /// <summary>
    /// True if the <see cref="Angle"/> is less than 90º degrees or Π/2 radians; otherwise false.
    /// </summary>
    [JsonIgnore]
    public bool IsAcute => Degrees < 90.0;

    /// <summary>
    /// True if the <see cref="Angle"/> is greater than 90º degrees or Π/2 radians buth less than 180º degrees or Π radians; otherwise false.
    /// </summary>
    [JsonIgnore]
    public bool IsObtuse { get { if (IsUndefined) return false; var a = Degrees < 180 ? Degrees : Degrees - 180; return a > 90.0 && a < 180.0; } }


    /// <summary>
    /// True if the <see cref="Angle"/> is 90º degrees or Π/2 radians or 270º degrees or 3Π/4 radians; otherwise false.
    /// </summary>
    [JsonIgnore]
    public bool IsRight => Degrees == 90.0 || Degrees == 270.0;

    /// <summary>
    /// True if the <see cref="Angle"/> is 180º degrees or Π radians; otherwise false.
    /// </summary>
    [JsonIgnore]
    public bool IsStraight => Degrees == 180.0;

    /// <summary>
    /// True if the <see cref="Angle"/> <see cref="Degrees"/>/<see cref="Radians"/>is <see cref="double.NaN"/>; otherwise false.
    /// </summary>

    [JsonIgnore]
    public bool IsUndefined => double.IsNaN(Degrees);

    /// <summary>
    /// Calculates the smallest angle between this angle and another angle.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>The <see cref="Angle"/> between this and other.</returns>
    public Angle To(Angle other) => FromDegrees(Math.Abs(Degrees - other.Degrees));

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for equality.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if both angles differ less than <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator ==(in Angle one, in Angle another) => one.Equals(another);

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for inequality.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if both angles differ equal or greather than <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator !=(in Angle one, in Angle another) => !one.Equals(another);

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for one is greater than another.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if one is greater than another at least with <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator >(in Angle one, in Angle another) => one.CompareTo(another) == 1;

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for one is equal or greater than another.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if one is equal or greater than another; otherwise false;</returns>
    public static bool operator >=(in Angle one, in Angle another) => one.CompareTo(another) >= 0;

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for one is lesser than another.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if one is less than another at least with <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator <(in Angle one, in Angle another) => one.CompareTo(another) == -1;

    /// <summary>
    /// Compares two <see cref="Angle">angles</see> for one is less than or equal with another.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>True if one is less than or equal to another; otherwise false;</returns>
    public static bool operator <=(in Angle one, in Angle another) => one.CompareTo(another) <= 0;

    /// <summary>
    /// Compares if an <see cref="Angle"/> is less than given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>True if the angle is less than the degrees with at least <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator <(in Angle angle, in double degrees) => angle < FromDegrees(degrees);

    /// <summary>
    /// Compares if an <see cref="Angle">angles</see> is equal or greater than given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>True if the angle less than or is equal to the degrees; otherwise false;</returns>
    public static bool operator <=(in Angle angle, in double degrees) => angle <= FromDegrees(degrees);

    /// <summary>
    /// Compares if an <see cref="Angle">angles</see> greater than given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>True if the angle is greather than the degrees with at least <see cref="CompareTolerance"/>; otherwise false;</returns>
    public static bool operator >(in Angle angle, in double degrees) => angle > FromDegrees(degrees);

    /// <summary>
    /// Compares if an <see cref="Angle">angles</see> is equal or greater than given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>True if the angle is equal to or greather than the degrees; otherwise false;</returns>
    public static bool operator >=(in Angle angle, in double degrees) => angle >= FromDegrees(degrees);

    /// <summary>
    /// Creates a new <see cref="Angle"/> that is the sum of this angle and the given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>Results in a valid <see cref="Angle"/>, unless the angle is undefined.</returns>
    public static Angle operator +(in Angle angle, double degrees) => angle + FromDegrees(degrees);

    /// <summary>
    /// Creates a new <see cref="Angle"/> that is the sum of this angle and the given degrees.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>Results in a valid <see cref="Angle"/>, unless any of the angles is indefined.</returns>
    public static Angle operator +(in Angle one, in Angle another)
    {
        if (one.IsUndefined || another.IsUndefined) return Undefined;
        var d = one.Degrees + another.Degrees; return FromDegrees(d < 360.0 ? d : d - 360.0);
    }

    /// <summary>
    /// Creates a new <see cref="Angle"/> that is this angle minus the given degrees.
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="degrees"></param>
    /// <returns>Results in a valid <see cref="Angle"/>, unless the angle is undefined.</returns>
    public static Angle operator -(in Angle angle, in double degrees) => angle - FromDegrees(degrees);

    /// <summary>
    /// Creates a new <see cref="Angle"/> that is one minus another angle.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns>Results in a valid <see cref="Angle"/>, unless any of the angles is indefined.</returns>
    public static Angle operator -(in Angle one, in Angle another)
    {
        if (one.IsUndefined || another.IsUndefined) return Undefined;
        var d = one.Degrees - another.Degrees; return FromDegrees(d >= 0 ? d : d + 360.0);
    }

    /// <summary>
    /// Implementation of the <see cref="IComparable{T}"/>
    /// </summary>
    /// <param name="other"></param>
    /// <returns>
    /// If both angles are undefined: 0,
    /// If the differerence between this angle and another are less than <see cref="CompareTolerance"/>: 0,
    /// If this is greather than the other: 1,
    /// If this is less than the other: -1.
    /// </returns>
    public int CompareTo(Angle other) =>
        IsUndefined && other.IsUndefined ? 0 :
        IsUndefined && !other.IsUndefined ? -1 :
        !IsUndefined && other.IsUndefined ? 1 :
        Degrees > other.Degrees && Degrees - other.Degrees > CompareTolerance ? 1 :
        Degrees < other.Degrees && other.Degrees - Degrees > CompareTolerance ? -1 :
        0;

    /// <summary>
    /// OBSOLETE!
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns></returns>
    [Obsolete("Use + operator instead.")]
    public static Angle Add(Angle one, Angle another) => one + another;

    /// <summary>
    /// OBSOLETE!
    /// </summary>
    /// <param name="one"></param>
    /// <param name="another"></param>
    /// <returns></returns>
    [Obsolete("Use - operator instead.")]
    public static Angle Subtract(Angle one, Angle another) => one - another;

    /// <summary>
    /// Determines the minimum <see cref="Angle"/> between this and other angle.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>Returns the minimum <see cref="Angle"/> between this and other angle.</returns>
    public Angle Min(in Angle other)
    {
        var a1 = this - other;
        var a2 = other - this;
        return a1 < a2 ? a1 : a2;
    }

    /// <summary>
    /// Determines if this angle is between two other angles, handling wrap-around at 0°/360°.
    /// </summary>
    /// <param name="lower">The lower bound of the range.</param>
    /// <param name="upper">The upper bound of the range.</param>
    /// <returns>
    /// True if this angle is within the range [lower, upper], handling the case where
    /// the range wraps around 0° (e.g., 315° to 45° includes 350°, 0°, and 30°).
    /// Returns false if any angle is undefined.
    /// </returns>
    public bool IsBetween(in Angle lower, in Angle upper)
    {
        if (IsUndefined || lower.IsUndefined || upper.IsUndefined) return false;
        if (lower <= upper)
        {
            // Normal case: range doesn't wrap (e.g., 60° to 120°)
            return this >= lower && this <= upper;
        }
        else
        {
            // Wrap-around case: range crosses 0° (e.g., 315° to 45°)
            return this >= lower || this <= upper;
        }
    }

    /// <summary>
    /// Default formatted value for an <see cref="Angle"/>.
    /// </summary>
    /// <returns>"Undefined" or degrees with two decimals and unit symbol °.</returns>
    public override string ToString() => IsUndefined ? "Undefined" : $"{Degrees:F2}°";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>
    /// Returns true if <paramref name="obj"/> is of type <see cref="Angle"/> and
    /// the difference between the angles degrees difference between this and other <see cref="Angle"/>  
    /// is less than <see cref="CompareTolerance"/>.
    /// </returns>
    public override bool Equals(object? obj) => obj is Angle angle && Equals(angle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param> 
    /// <returns>True if the degrees difference between this and other <see cref="Angle"/>  
    /// is less than <see cref="CompareTolerance"/>.
    /// </returns>
    public bool Equals(Angle other) => CompareTo(other) == 0;

    /// <summary>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode() => Degrees.GetHashCode();
}
