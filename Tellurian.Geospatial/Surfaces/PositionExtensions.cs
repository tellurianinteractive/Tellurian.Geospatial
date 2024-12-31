using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Geospatial.Surfaces;

/// <summary>
/// Additional methods operating on a set of <see cref="Position">positions</see>.
/// </summary>
public static class PositionExtensions
{
    public static bool IsWithin(this Position me, Surface area) => area.Includes(me);

    public static IEnumerable<Stretch> EnclosingSides(this Position[] positions) =>
        positions.Select((v, i) => Stretch.Between(v, positions[i > positions.Length - 1 ? 0 : i]));
    public static Latitude MostNorthern(this Position[] positions) => Latitude.FromDegrees(positions.Max(p => p.Latitude.Degrees));
    public static Latitude MostSouthern(this Position[] positions) => Latitude.FromDegrees(positions.Min(p => p.Latitude.Degrees));
    public static Longitude MostEastern(this Position[] positions) => Longitude.FromDegrees(positions.Max(p => p.Longitude.Degrees));
    public static Longitude MostWestern(this Position[] positions) => Longitude.FromDegrees(positions.Min(p => p.Longitude.Degrees));

}
