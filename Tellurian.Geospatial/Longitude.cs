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
        private readonly double _Degrees;

        public static Longitude FromDegrees(double degrees) => new Longitude(degrees, value => value >=-180 && value <= 180);
        public static Longitude FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

        private Longitude(in double degrees, Func<double, bool> validate)
        {
            if (!validate.Invoke(degrees)) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees => _Degrees;
        public double Radians => Degrees * Math.PI / 180;
        public bool IsZero => Math.Abs(_Degrees) < CompareTolerance;

        public static bool operator ==(in Longitude one, in Longitude another) =>one.Equals(another);
        public static bool operator !=(in Longitude one, in Longitude another) =>!one.Equals(another);
        public static bool operator >(in Longitude one, in Longitude another) =>one.Degrees > another.Degrees;
        public static bool operator <(in Longitude one, in Longitude another) =>one.Degrees < another.Degrees;
        public static bool operator >=(in Longitude one, in Longitude another) =>one.Degrees >= another.Degrees;
        public static bool operator <=(in Longitude one, in Longitude another) =>one.Degrees <= another.Degrees;

        public bool Equals(Longitude other) => Math.Abs(Degrees - other.Degrees) < CompareTolerance;

        public override bool Equals(object obj)
        {
            if (!(obj is Longitude)) return false;
            return Equals((Longitude)obj);
        }

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0:F1}", Degrees);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => _Degrees.GetHashCode();
    } 
}
