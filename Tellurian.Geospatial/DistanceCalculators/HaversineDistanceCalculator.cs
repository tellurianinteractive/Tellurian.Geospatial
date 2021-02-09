using static System.Math;

namespace Tellurian.Geospatial.DistanceCalculators
{
    /// <summary>
    /// Calculates distance using the <see cref="https://en.wikipedia.org/wiki/Haversine_formula">Haversine formula</see>
    /// </summary>
    /// <remarks>
    /// <para>This  formula remains particularly well-conditioned for numerical computa­tion even at small distances.</para>
    /// <para>
    /// This implementation uses formulas from 'Latitude/longitude spherical geodesy tools'
    /// MIT Licence (c) Chris Veness 2002-2019
    /// https://www.movable-type.co.uk/scripts/latlong.html                      
    /// https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html 
    /// </para>
    /// </remarks>
    public sealed class HaversineDistanceCalculator : IDistanceCalculator
    {
        public Distance GetDistance(in Position from, in Position to)
        {
            const double r = Constants.EarthMeanRadiusMeters;
            var (φ1, λ1) = from.RadianCoordinates;
            var (φ2, λ2) = to.RadianCoordinates;
            var Δφ = φ2 - φ1;
            var Δλ = λ2 - λ1;
            var a = (Sin(Δφ / 2) * Sin(Δφ / 2)) + (Cos(φ1) * Cos(φ2) * Sin(Δλ / 2) * Sin(Δλ / 2));
            var c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));
            return Distance.FromMeters(r * c);
        }
    }
}

