using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Speed : IEquatable<Speed>
    {
        const double CompareTolerance = 0.01;

        [DataMember(Name = "MetersPerSecond")]
        readonly double _MetersPerSecond;

        public static Speed Zero { get; } = new Speed(0);

        public static Speed FromMetersPerSecond(double metersPerSecond) => new Speed(metersPerSecond);
        public static Speed FromKilometersPerHour(double kilometersPerHour) => new Speed(kilometersPerHour / 3.6);
        private Speed(in double metersPerSecond)
        {
            if (metersPerSecond < 0) throw new ArgumentOutOfRangeException(nameof(metersPerSecond), "Speed must be zero or positive.");
            _MetersPerSecond = metersPerSecond;
        }

        public double MetersPerSecond => _MetersPerSecond;
        public double KilometersPerHour => MetersPerSecond * 3.6;
        public bool IsZero => MetersPerSecond < CompareTolerance;
        public bool IsBelow(double metersPerSecond) => IsBelow(FromMetersPerSecond(metersPerSecond));
        public bool IsBelow(Speed other) => this < other; 

        public static bool operator == (in Speed one, in Speed another) => one.Equals(another);
        public static bool operator !=(in Speed one, in Speed another) => !one.Equals(another);
        public static bool operator >(in Speed one, in Speed another) => one.MetersPerSecond > another.MetersPerSecond;
        public static bool operator <(in Speed one, in Speed another) => one.MetersPerSecond < another.MetersPerSecond;
        public static bool operator >=(in Speed one, in Speed another) => one.MetersPerSecond >= another.MetersPerSecond;
        public static bool operator <=(in Speed one, in Speed another) => one.MetersPerSecond <= another.MetersPerSecond;
        public static Speed operator +(in Speed one, in Speed another) => Speed.FromMetersPerSecond(one.MetersPerSecond + another.MetersPerSecond);

        public bool Equals(Speed other) => Math.Abs(MetersPerSecond - other.MetersPerSecond) <= CompareTolerance;
        public override bool Equals(object obj) => obj is Speed && Equals((Speed)obj);
        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0:F1}m/s", MetersPerSecond);
        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => MetersPerSecond.GetHashCode();
    }
}
