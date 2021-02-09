using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Transformation
{
    [DataContract]
    public readonly struct GridCoordinate : IEquatable<GridCoordinate>
    {
        [JsonConstructor]
        public GridCoordinate(double northing, double easting)
        {
            _Northing = northing;
            _Easting = easting;
        }
        [DataMember]
        private readonly double _Easting;
        [DataMember]
        private readonly double _Northing;
        [JsonPropertyName("easting")]
        public double Easting => _Easting;
        [JsonPropertyName("northing")]
        public double Northing => _Northing;

        public (double X, double Y) Coordinates => (Northing, Easting);

        public bool Equals(GridCoordinate other) => Northing == other.Northing && Easting == other.Easting;
        public override bool Equals(object? obj) => obj is GridCoordinate other && Equals(other);


        public static bool operator ==(in GridCoordinate one, in GridCoordinate another) => one.Equals(another);
        public static bool operator !=(GridCoordinate one, GridCoordinate another) => !one.Equals(another);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => HashCode.Combine(Northing, Easting);
    }
}
