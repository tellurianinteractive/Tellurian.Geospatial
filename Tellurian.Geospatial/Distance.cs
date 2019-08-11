using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        const double CompareTolerance = 0.001;

        public static Distance Zero => FromMeters(0);

        [DataMember(Name = "Meters")]
        readonly double _Meters;

        public static Distance FromMeters(double meters) => new Distance(meters);
        public static Distance FromKilometers(double kilometers) => new Distance(kilometers * 1000);

        private Distance(in double meters)
        {
            if (meters < 0) throw new ArgumentOutOfRangeException(nameof(meters), "A distance must be zero or positive.");
            _Meters = meters;
        }

        public double Meters => _Meters;
        public double Kilometers => Meters / 1000;
        public bool IsZero => Meters < CompareTolerance; 

        public static bool operator == (in Distance one, in Distance another) => one.Equals(another); 
        public static bool operator !=(in Distance one, in Distance another) => ! one.Equals(another); 
        public static bool operator >(in Distance one, in Distance another) => one.CompareTo(another) == 1;  
        public static bool operator >=(in Distance one, in Distance another) => one.CompareTo(another) >= 0; 
        public static bool operator <(in Distance one, in Distance another) => one.CompareTo(another) == -1; 
        public static bool operator <=(in Distance one, in Distance another) => one.CompareTo(another) <= 0;
        public static Distance operator + (in Distance one, in Distance another) => FromMeters(one.Meters + another.Meters);
        public static Distance operator +(in Distance one, in double value) => FromMeters(one.Meters + value > 0 ? value : 0);

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
    }
}
