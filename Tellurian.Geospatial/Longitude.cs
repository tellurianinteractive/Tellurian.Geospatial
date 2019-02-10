using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{

    [DataContract]
    public struct Longitude : IEquatable<Longitude>
    {
        const double CompareTolerance = 0.00001;

        [DataMember(Name = "Degrees")]
        private double _Dregrees;

        public static Longitude FromDegrees(double degrees)
        {
            return new Longitude(degrees, value => value >=-180 && value <= 180);
        }

        public static Longitude FromRadians(double radians)
        {
            return FromDegrees(radians * 180 / Math.PI);
        }

        private Longitude(double degrees, Func<double, bool> validate)
        {
            if (!validate.Invoke(degrees)) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Dregrees = degrees;
        }

        public double Degrees { get { return _Dregrees; } }
        public double Radians { get { return Degrees * Math.PI / 180; } }
        public bool IsZero { get { return Math.Abs(_Dregrees) < CompareTolerance; } }

        public static bool operator ==(Longitude one, Longitude another) { return one.Equals(another); }
        public static bool operator !=(Longitude one, Longitude another) { return !one.Equals(another); }
        public static bool operator >(Longitude one, Longitude another) { return one.Degrees > another.Degrees; }
        public static bool operator <(Longitude one, Longitude another) { return one.Degrees < another.Degrees; }
        public static bool operator >=(Longitude one, Longitude another) { return one.Degrees >= another.Degrees; }
        public static bool operator <=(Longitude one, Longitude another) { return one.Degrees <= another.Degrees; }

        public bool Equals(Longitude other)
        {
            return Math.Abs(Degrees - other.Degrees) < CompareTolerance;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Longitude)) return false;
            return Equals((Longitude)obj);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _Dregrees.GetHashCode();
        }
    } 
}
