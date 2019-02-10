using static System.Math;

namespace Tellurian.Geospatial.DistanceCalculators
{
    /// <summary>
    /// Calculates distance using the <see cref="https://en.wikipedia.org/wiki/Haversine_formula">Haversine formula</see>
    /// </summary>
    /// <remarks>
    /// This  formula remains particularly well-conditioned for numerical computa­tion even at small distances.
    ///
    /// This implementation uses formulas from 'Latitude/longitude spherical geodesy tools'
    /// MIT Licence (c) Chris Veness 2002-2019
    /// https://www.movable-type.co.uk/scripts/latlong.html                      
    /// https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html 
    /// </remarks>
    public class HaversineDistanceCalculator : IDistanceCalculator
    {
        public Distance GetDistance(Position from, Position to)
        {
            const double r = Constants.EarthMeanRadiusMeters;
            var lat1 = from.Latitude.Radians;
            var lon1 = from.Longitude.Radians;
            var lat2 = to.Latitude.Radians;
            var lon2 = to.Longitude.Radians;
            var dLat = lat2 - lat1;
            var dLon = lon2 - lon1;
            var a = Sin(dLat / 2) * Sin(dLat / 2) + Cos(lat1) * Cos(lat2) * Sin(dLon / 2) * Sin(dLon / 2);
            var c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));
            return Distance.FromMeters(r * c);
        }
    }
}

