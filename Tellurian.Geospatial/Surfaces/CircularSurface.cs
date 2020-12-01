using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Surfaces
{
    [DataContract]
    public sealed class CircularSurface : Surface
    {
        [JsonConstructor]
        public CircularSurface(Position referencePosition, Distance radius) : base(referencePosition) => Radius = radius;
        [DataMember]
        public Distance Radius { get; private set; }
        public override bool Includes(Position position) => Stretch.Between(ReferencePosition, position).Distance <= Radius;
        public override bool Equals( Surface? me) => me is CircularSurface other && base.Equals(other) && Radius.Equals(other.Radius);       
    }
}
