using System.Runtime.Serialization;

namespace Tellurian.Geospatial.Surfaces;
/// <summary>
/// Base class for all types of spatial surfaces.
/// </summary>
[DataContract]
[KnownType(typeof(CircularSurface))]
[KnownType(typeof(PolygonalSurface))]
public abstract class Surface : IEquatable<Surface>
{
    /// <summary>
    /// Base class constructor.
    /// </summary>
    /// <param name="referencePosition">A surface as at least a reference <see cref="Position"/>.</param>
    protected Surface(Position referencePosition) => ReferencePosition = referencePosition;

    /// <summary>
    /// The <see cref="ReferencePosition"/> is the single point represetation of the <see cref="Surface"/>.
    /// It can be used in large zoom levels and also to calculare distances between <see cref="Surface">surfaces</see>.
    /// </summary>
    [DataMember]
    public Position ReferencePosition { get; private set; }

    /// <summary>
    /// Tells if a <see cref="Position"/> is within a <see cref="Surface"/> or not.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>True if within border or on border of the <see cref="Surface"/>; otherwise false;</returns>
    public abstract bool Includes(Position position);

    /// <summary>
    /// Determines if this surface is equal or not to another surface.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>True if <paramref name="obj"/> is of type <see cref="Surface"/> and properties are equal; otherwise false.</returns>
    public override bool Equals(object? obj) => obj is Surface other && Equals(other);

    /// <summary>
    /// Determines if this surface is equal or not to another surface.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if properties are equal; otherwise false.</returns>
    public virtual bool Equals(Surface? other) => other is not null && other.ReferencePosition.Equals(ReferencePosition);
    
    /// <summary>
    /// </summary>
    public override int GetHashCode() => ReferencePosition.GetHashCode();
}
