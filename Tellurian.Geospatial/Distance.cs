using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        private const double CompareTolerance = 0.001;

        public static Distance Zero => FromMeters(0);

        [DataMember(Name = "Meters")]
        private readonly double _Meters;

        public static Distance FromMeters(double meters) => new Distance(meters);
        public static Distance FromKilometers(double kilometers) => new Distance(kilometers * 1000);

        [JsonConstructor]
        public Distance(double meters)
        {
            if (meters < 0) throw new ArgumentOutOfRangeException(nameof(meters), "A distance must be zero or positive.");
            _Meters = meters;
        }

        [JsonPropertyName("meters")]
        public double Meters => _Meters;
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

        public bool Equals(Distance other) => Math.Abs(other.Meters - Meters) < CompareTolerance;

        public override bool Equals(object obj)
        {
            if (!(obj is Distance)) return false;
            return Equals((Distance)obj);
        }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0}m", Meters);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => Meters.GetHashCode();

        public int CompareTo(Distance other) => Equals(other) ? 0 : Meters.CompareTo(other.Meters);
        public static Distance Add(Distance left, Distance right) => FromMeters(left.Meters + right.Meters);
        public static Distance Subtract(Distance left, Distance right) { var d = left.Meters - right.Meters; return FromMeters(d > 0 ? d : 0); }
    }
}
