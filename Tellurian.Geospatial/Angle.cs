using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Angle : IEquatable<Angle>, IComparable<Angle>
    {
        public const double DefaultCompareTolerance = 0.00001;
        public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

        [DataMember(Name = "Degrees")]
        private readonly double _Degrees;

        public static Angle FromDegrees(double degrees) => new Angle(degrees, 0 , 360);

        public static Angle FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

        public static Angle Zero => FromDegrees(0);
        public static Angle Right => FromDegrees(90);
        public static Angle Straight => FromDegrees(180);

        private Angle(in double degrees, double min, double max)
        {
            if (degrees < min || degrees >= max) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees => _Degrees;

        public double Radians => Degrees * Math.PI / 180;
        public Angle Complement => Degrees == 0 ? FromDegrees(0) : FromDegrees(360 - Degrees);
        public Angle Reverse => FromDegrees(Degrees >= 180 ? Degrees - 180 : Degrees + 180);
        public bool IsAcute => Degrees < 90.0;
        public bool IsObtuse { get { var a = Degrees < 180 ? Degrees : Degrees - 180; return a > 90.0 && a < 180.0; } }
        public bool IsRight => Math.Abs(Degrees - 90.0) < CompareTolerance || Math.Abs(Degrees - 270.0) < CompareTolerance;
        public bool IsStraight => Math.Abs(Degrees - 180.0) < CompareTolerance;

        /// <summary>
        /// Calculates the angle between this angle and another angle.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>The <see cref="Angle"/> between this and other.
        ///</returns>
        public  Angle To(Angle other) => FromDegrees(Math.Abs(Degrees - other.Degrees));

        public static bool operator ==(in Angle one, in Angle another) => one.Equals(another);
        public static bool operator !=(in Angle one, in Angle another) => !one.Equals(another);
        public static bool operator >(in Angle one, in Angle another) => one.CompareTo(another) == 1;
        public static bool operator >=(in Angle one, in Angle another) => one.CompareTo(another) >= 0;
        public static bool operator <(in Angle one, in Angle another) => one.CompareTo(another) == -1;
        public static bool operator <=(in Angle one, in Angle another) => one.CompareTo(another) <= 0;
        public static Angle operator -(in Angle one, in Angle another) { var d = one.Degrees - another.Degrees; return FromDegrees(d >= 0 ? d : d + 360.0); }
        public static Angle operator +(in Angle one, in Angle another) { var d = one.Degrees + another.Degrees; return FromDegrees(d < 360.0 ? d : d - 360.0); }

        public int CompareTo(Angle other) =>
            (Degrees < other.Degrees) && other.Degrees-Degrees > CompareTolerance ? -1 :
            ((Degrees > other.Degrees) && Degrees-other.Degrees > CompareTolerance ? 1 : 0);

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

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0:F2}°", Degrees);
        public override bool Equals(object obj) => obj is Angle angle && Equals(angle);
        public bool Equals(Angle other) => CompareTo(other) == 0;

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => _Degrees.GetHashCode();
    }
}
