using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public struct Latitude : IEquatable<Latitude>
    {
        const double CompareTolerance = 0.00001;

        [DataMember(Name = "Degrees")]
        readonly double _Degrees;

        public static Latitude FromDegrees(double degrees)
        {
            return new Latitude(degrees, value => value >=-90 && value <= 90);
        }

        public static Latitude FromRadians(double radians)
        {
            return FromDegrees(radians * 180 / Math.PI);
        }

        private Latitude(double degrees, Func<double, bool> validate)
        {
            if (!validate.Invoke(degrees)) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees => _Degrees;
        public double Radians { get { return Degrees * Math.PI / 180; } }
        public bool IsZero { get { return Math.Abs(_Degrees) < CompareTolerance; } }

        public static bool operator == (Latitude one, Latitude another) { return one.Equals(another); }
        public static bool operator !=(Latitude one, Latitude another) { return ! one.Equals(another); }
        public static bool operator >(Latitude one, Latitude another) { return one.Degrees > another.Degrees; }
        public static bool operator <(Latitude one, Latitude another) { return one.Degrees < another.Degrees; }
        public static bool operator >=(Latitude one, Latitude another) { return one.Degrees >= another.Degrees; }
        public static bool operator <=(Latitude one, Latitude another) { return one.Degrees <= another.Degrees; }

        public override bool Equals(object obj)
        {
            if (!(obj is Latitude)) return false;
            Latitude other = (Latitude)obj;
            return Equals(other);
        }

        public bool Equals(Latitude other)
        {
            return Math.Abs(Degrees - other.Degrees) < CompareTolerance;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _Degrees.GetHashCode();
        }
    }
}
