using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial
{
    [DataContract]
    public readonly struct Vector : IEquatable<Vector>
    {
        public static Vector From(double degrees, double meters) =>
            new Vector(Angle.FromDegrees(degrees), Distance.FromMeters(meters));

        [JsonConstructor]
        public Vector(Angle direction, Distance distance)
        {
            Direction = direction;
            Distance = distance;
        }
        [DataMember]
        [JsonPropertyName("direction")]
        public Angle Direction { get; init; }

        [DataMember]
        [JsonPropertyName("distance")]
        public Distance Distance { get; init; }

        public bool Equals(Vector other) => other.Direction == Direction && other.Distance == Distance;
        public override bool Equals(object? obj) => obj is Vector vector && Equals(vector);
        public override int GetHashCode() => HashCode.Combine(Direction, Distance);
        public static bool operator ==(Vector left, Vector right) => left.Equals(right);
        public static bool operator !=(Vector left, Vector right) => !(left == right);
    }
}
