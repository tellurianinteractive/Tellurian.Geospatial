using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Surfaces;

/// <summary>
/// Represents a circular surface with a reference position and a radius.
/// </summary>
[DataContract]
public sealed class CircularSurface : Surface
{
    /// <summary>
    /// Creates a <see cref="CircularSurface"/>.
    /// </summary>
    /// <param name="referencePosition"></param>
    /// <param name="radius"></param>
    [JsonConstructor]
    public CircularSurface(Position referencePosition, Distance radius) : base(referencePosition) => Radius = radius;
    
    /// <summary>
    /// The radius of the <see cref="CircularSurface"/>.
    /// </summary>
    [DataMember]
    public Distance Radius { get; private set; }

    /// <summary>
    /// Determines whether a <see cref="Position"/> lies within the surface boundary or not.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>True if within bounds or on bound; otherwise false;</returns>
    public override bool Includes(Position position) => Stretch.Between(ReferencePosition, position).Distance <= Radius;

    /// <summary>
    /// Determines if this surface is equal or not to another surface.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if both are <see cref="CircularSurface"/> and properties are equal; otherwise false.</returns>
    public override bool Equals(Surface? other) => other is CircularSurface it && base.Equals(it) && Radius.Equals(it.Radius);
}
