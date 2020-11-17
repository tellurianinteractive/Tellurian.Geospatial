﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Longitude : IEquatable<Longitude>, IComparable<Longitude>
    {
        public const double DefaultCompareTolerance = 0.00001;
        public static double CompareTolerance { get; set; } = DefaultCompareTolerance;

        [DataMember(Name = "Degrees")]
        private readonly double _Degrees;

        public static Longitude FromDegrees(double degrees) => new Longitude(degrees, -180 ,180);
        public static Longitude FromRadians(double radians) => FromDegrees(radians * 180 / Math.PI);

        private Longitude(in double degrees, double min, double max)
        {
            if (degrees < min || degrees > max) throw new ArgumentOutOfRangeException(nameof(degrees));
            _Degrees = degrees;
        }

        public double Degrees => _Degrees;
        public double Radians => Degrees * Math.PI / 180;
        public bool IsZero => Math.Abs(_Degrees) < CompareTolerance;

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
