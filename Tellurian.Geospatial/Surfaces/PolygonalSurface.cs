using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Surfaces;

/// <summary>
/// Represents a spatial area made up of a set od lines defining its border.
/// </summary>
[DataContract]
public class PolygonalSurface : Surface
{
    /// <summary>
    /// Creates a <see cref="PolygonalSurface"/>.
    /// </summary>
    /// <param name="borderPositions"></param>
    /// <param name="referencePosition"></param>
    [JsonConstructor]
    public PolygonalSurface(IEnumerable<Position> borderPositions, Position referencePosition) : base(referencePosition) => _BorderPositions = borderPositions.ToArray();

    [DataMember(Name = "BorderPositions")]
    [JsonPropertyName("borderPositions")]
    private readonly Position[] _BorderPositions;

    /// <summary>
    /// The <see cref="Position">positions</see> that makes up the lines that forms the polygonal area.
    /// </summary>
    public IEnumerable<Position> BorderPositions => _BorderPositions;

    /// <summary>
    /// Determines whether a <see cref="Position"/> lies within the surface boundary or not.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>True if within bounds or on bound; otherwise false;</returns>

    public override bool Includes(Position position)
    {
        if (IsInsideEnclosingRectangle(position))
        {
            var nvert = _BorderPositions.Length;
            var j = nvert - 1;
            bool result = false;
            for (var i = 0; i < nvert; j = i++)
            {
                var vertxi = _BorderPositions[i].Longitude.Degrees;
                var vertyi = _BorderPositions[i].Latitude.Degrees;
                var vertxj = _BorderPositions[j].Longitude.Degrees;
                var vertyj = _BorderPositions[j].Latitude.Degrees;
                var py = position.Latitude.Degrees;
                var px = position.Longitude.Degrees;
                if (((vertyi > py) != (vertyj > py)) && (px < ((vertxj - vertxi) * (py - vertyi) / (vertyj - vertyi)) + vertxi)) result = !result;
            }
            return result ? result : BorderPositions.Any(p => p.Equals(position));
        }
        return false;
    }

    private bool IsInsideEnclosingRectangle(Position position) =>
        position.Longitude <= _BorderPositions.MostEastern() ||
        position.Longitude >= _BorderPositions.MostWestern() ||
        position.Latitude <= _BorderPositions.MostNorthern() ||
        position.Latitude >= _BorderPositions.MostSouthern();

    /// <summary>
    /// Determines if two surfaces are equal;
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if both are <see cref="CircularSurface"/> and properties are equal; otherwise false.</returns>
    public override bool Equals(Surface? other) => other is PolygonalSurface it && base.Equals(it) && Enumerable.SequenceEqual(BorderPositions, it.BorderPositions);
}
