using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public struct Distance : IEquatable<Distance>
    {
        const double CompareTolerance = 0.001;

        public static Distance Zero => FromMeters(0);

        [DataMember(Name = "Meters")]
        readonly double _Meters;

        public static Distance FromMeters(double meters)
        {
            return new Distance(meters);
        }

        public static Distance FromKilometers(double kilometers)
        {
            return new Distance(kilometers * 1000);
        }

        private Distance(double meters)
        {
            if (meters < 0) throw new ArgumentOutOfRangeException(nameof(meters), "A distance must be zero or positive.");
            _Meters = meters;
        }

        public double Meters => _Meters;
        public double Kilometers => Meters / 1000;

        public bool IsZero { get { return Meters < CompareTolerance; } }


        public static bool operator == (Distance one, Distance another) { return one.Equals(another); }
        public static bool operator !=(Distance one, Distance another) { return ! one.Equals(another); }
        public static bool operator >(Distance one, Distance another) { return one.Meters > another.Meters;  }
        public static bool operator >=(Distance one, Distance another) { return one.Meters >= another.Meters; }
        public static bool operator <(Distance one, Distance another) { return one.Meters < another.Meters; }
        public static bool operator <=(Distance one, Distance another) { return one.Meters <= another.Meters; }

        public bool Equals(Distance other)
        {
            return Math.Abs(other.Meters - Meters) < CompareTolerance;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Distance)) return false;
            return Equals((Distance)obj);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}m", Meters);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Meters.GetHashCode();
        }
    }
}
