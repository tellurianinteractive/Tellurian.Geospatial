using System;
using System.Runtime.Serialization;

namespace Tellurian.Geospatial.Surfaces;

[DataContract]
[KnownType(typeof(CircularSurface))]
[KnownType(typeof(PolygonalSurface))]
public abstract class Surface : IEquatable<Surface>
{
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
    /// <returns><see cref="true"/> if within border or on border of the <see cref="Surface"/></returns>
    public abstract bool Includes(Position position);
    public override bool Equals(object? obj) => obj is Surface other && Equals(other);
    public virtual bool Equals(Surface? other) => other is not null && other.ReferencePosition.Equals(ReferencePosition);
    public override int GetHashCode() => ReferencePosition.GetHashCode();


}
