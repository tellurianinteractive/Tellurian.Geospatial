using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Longitude : IEquatable<Longitude>, IComparable<Longitude>
    {
        public const double DefaultCompareTolerance = 0.00001;
        public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

        public static Longitude FromDegrees(double degrees) => new Longitude(degrees, -180 ,180);
        public static Longitude FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

        private Longitude(in double degrees, double min, double max)
        {
            if (degrees < min || degrees > max) throw new ArgumentOutOfRangeException(nameof(degrees));
            Degrees = degrees;
        }

        [JsonConstructor]
        public Longitude(double degrees) : this(degrees, -180, 180) { }

        [DataMember(Name = "Degrees")]
        [JsonPropertyName("degrees")]
        public double Degrees { get; init; }
        [JsonIgnore]
        public double Radians => Degrees * Math.PI / 180;
        [JsonIgnore]
        public bool IsZero => Math.Abs(Degrees) < CompareTolerance;

        public static bool operator ==(in Longitude one, in Longitude another) => one.Equals(another);
        public static bool operator !=(in Longitude one, in Longitude another) => !one.Equals(another);
        public static bool operator >(in Longitude one, in Longitude another) => one.CompareTo(another) == 1;
        public static bool operator <(in Longitude one, in Longitude another) => one.CompareTo(another) == -1;
        public static bool operator >=(in Longitude one, in Longitude another) => one.CompareTo(another) >= 0;
        public static bool operator <=(in Longitude one, in Longitude another) => one.CompareTo(another) <= 0;

        public int CompareTo(Longitude other) =>
            (Degrees < other.Degrees) && other.Degrees - Degrees > CompareTolerance ? -1 :
            ((Degrees > other.Degrees) && Degrees - other.Degrees > CompareTolerance ? 1 : 0);

        public bool Equals(Longitude other) => CompareTo(other) == 0;

        public override bool Equals(object? obj)
        {
            if (!(obj is Longitude)) return false;
            return Equals((Longitude)obj);
        }

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => Degrees.GetHashCode();
    }
}
