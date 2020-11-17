using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Latitude : IEquatable<Latitude>, IComparable<Latitude>
    {
        public const double DefaultCompareTolerance = 0.00001;
        public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

        [DataMember(Name = "Degrees")]
        private readonly double _Degrees;

        public static Latitude FromDegrees(double degrees) => new Latitude(degrees, -90 , 90);
        public static Latitude FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

        private Latitude(in double degrees, double min, double max)
        {
            if (degrees < min || degrees > max) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees => _Degrees;
        public double Radians => Degrees * Math.PI / 180;
        public bool IsZero => Math.Abs(_Degrees) < CompareTolerance;

        public static bool operator ==(in Latitude one, in Latitude another) => one.Equals(another);
        public static bool operator !=(in Latitude one, in Latitude another) => !one.Equals(another);
        public static bool operator >(in Latitude one, in Latitude another) => one.CompareTo(another) == 1;
        public static bool operator <(in Latitude one, in Latitude another) => one.CompareTo(another) == -1;
        public static bool operator >=(in Latitude one, in Latitude another) => one.CompareTo(another) >=0;
        public static bool operator <=(in Latitude one, in Latitude another) => one.CompareTo(another) <=0;

        public int CompareTo(Latitude other) =>
            (Degrees < other.Degrees) && other.Degrees - Degrees > CompareTolerance ? -1 :
            ((Degrees > other.Degrees) && Degrees - other.Degrees > CompareTolerance ? 1 : 0);

        public override bool Equals(object obj)
        {
            if (!(obj is Latitude)) return false;
            Latitude other = (Latitude)obj;
            return Equals(other);
        }

        public bool Equals(Latitude other) => CompareTo(other) == 0;

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => _Degrees.GetHashCode();
    }
}
