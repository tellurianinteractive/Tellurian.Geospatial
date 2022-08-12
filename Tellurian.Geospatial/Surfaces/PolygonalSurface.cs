using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Surfaces;

[DataContract]
public class PolygonalSurface : Surface
{
    [JsonConstructor]
    public PolygonalSurface(IEnumerable<Position> borderPositions, Position referencePosition) : base(referencePosition) => _BorderPositions = borderPositions.ToArray();

    [DataMember(Name = "BorderPositions")]
    [JsonPropertyName("borderPositions")]
    private readonly Position[] _BorderPositions;
    public IEnumerable<Position> BorderPositions => _BorderPositions;

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

    public override bool Equals(Surface? me) => me is PolygonalSurface other && base.Equals(other) && Enumerable.SequenceEqual(BorderPositions, other.BorderPositions);
}
