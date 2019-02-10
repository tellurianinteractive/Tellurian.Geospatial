using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public struct Angle : IEquatable<Angle>
    {
        const double CompareTolerance = 0.01;

        [DataMember(Name = "Degrees")]
        readonly double _Degrees;

        public static Angle FromDegrees(double degrees)
        {
            return new Angle(degrees, value => value >= 0 && value < 360);
        }

        public static Angle FromRadians(double radians)
        {
            return FromDegrees(radians * 180 / Math.PI);
        }

        Angle(double degrees, Func<double, bool> validate)
        {
            if (!validate.Invoke(degrees)) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees { get { return _Degrees; } }
        public double Radians { get { return Degrees * Math.PI / 180; } }
        public Angle Complement { get { return Degrees == 0 ? FromDegrees(0) : FromDegrees(360 - Degrees); } }
        public Angle Reverse { get { return FromDegrees(Degrees >= 180 ? Degrees - 180 : Degrees + 180); } }
        public bool IsAcute { get { return Degrees < 90.0; } }
        public bool IsObtuse { get { var a = Degrees < 180 ? Degrees : Degrees - 180; return a > 90.0 && a < 180.0; } }
        public bool IsRight { get { return Math.Abs(Degrees - 90.0) < CompareTolerance || Math.Abs(Degrees - 270.0) < CompareTolerance; } }
        public bool IsStraight { get { return Math.Abs(Degrees - 180.0) < CompareTolerance; } }

        public static bool operator ==(Angle one, Angle another) { return one.Equals(another); }
        public static bool operator !=(Angle one, Angle another) { return !one.Equals(another); }
        public static bool operator >(Angle one, Angle another) { return one.Degrees > another.Degrees; }
        public static bool operator >=(Angle one, Angle another) { return one.Degrees >= another.Degrees; }
        public static bool operator <(Angle one, Angle another) { return one.Degrees < another.Degrees; }
        public static bool operator <=(Angle one, Angle another) { return one.Degrees <= another.Degrees; }
        public static Angle operator -(Angle one, Angle another) { var d = one.Degrees - another.Degrees; return FromDegrees(d > 0 ? d : d + 360.0); }
        public static Angle operator +(Angle one, Angle another) { var d = one.Degrees + another.Degrees; return FromDegrees(d < 360.0 ? d : d - 360.0); }

        public Angle Min(Angle other)
        {
            var a1 = this - other;
            var a2 = other - this;
            return a1 < a2 ? a1 : a2;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:F2}°", Degrees);
        }

        public override bool Equals(object obj)
        {
            return obj is Angle && Equals((Angle)obj);
        }

        public bool Equals(Angle other)
        {
            return Math.Abs(Degrees - other.Degrees) < CompareTolerance;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return _Degrees.GetHashCode();
        }
    }
}
