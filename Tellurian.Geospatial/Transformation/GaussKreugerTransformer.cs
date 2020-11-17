using static System.Math;

namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// Methods for transforming between cartesian and planar coordinates.
    /// </summary>
    /// <remarks>
    /// References:
    /// Formulas from Swedish Land Survey (Lantmäteriet) formula collection:
    /// https://www.lantmateriet.se/sv/Kartor-och-geografisk-information/GPS-och-geodetisk-matning/Om-geodesi/Formelsamling/
    /// </remarks>
    public static class GaussKreugerTransformer
    {
        public static GridCoordinate ToGridCoordinate(in Position geodeticPosition, in MapProjection projection)
        {
            var axis = projection.Ellipsoid.SemiMajorAxis;
            var centralMeridian = projection.CentralMeridian;
            var falseEasting = projection.FalseEasting;
            var falseNorthing = projection.FalseNorthing;
            var flattening = projection.Ellipsoid.Flattening;
            var latitude = geodeticPosition.Latitude.Radians;
            var longitude = geodeticPosition.Longitude.Radians;
            var scale = projection.Scale;
            // Prepare ellipsoid-based stuff.
            var e2 = flattening * (2.0 - flattening);
            var n = flattening / (2.0 - flattening);
            var aRoof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
            var a = e2;
            var b = (5.0 * e2 * e2 - e2 * e2 * e2) / 6.0;
            var c = (104.0 * e2 * e2 * e2 - 45.0 * e2 * e2 * e2 * e2) / 120.0;
            var d = (1237.0 * e2 * e2 * e2 * e2) / 1260.0;
            var beta1 = n / 2.0 - 2.0 * n * n / 3.0 + 5.0 * n * n * n / 16.0 + 41.0 * n * n * n * n / 180.0;
            var beta2 = 13.0 * n * n / 48.0 - 3.0 * n * n * n / 5.0 + 557.0 * n * n * n * n / 1440.0;
            var beta3 = 61.0 * n * n * n / 240.0 - 103.0 * n * n * n * n / 140.0;
            var beta4 = 49561.0 * n * n * n * n / 161280.0;
            // Convert
            const double degToRad = PI / 180.0;
            var phi = latitude;
            var lambda = longitude;
            var lambdaZero = centralMeridian * degToRad;
            var phiStar = phi - Sin(phi) * Cos(phi) * (a +
                                b * Pow(Sin(phi), 2) +
                                c * Pow(Sin(phi), 4) +
                                d * Pow(Sin(phi), 6));
            var deltaLambda = lambda - lambdaZero;
            var xiPrim = Atan(Tan(phiStar) / Cos(deltaLambda));
            var etaPrim = Atanh(Cos(phiStar) * Sin(deltaLambda));
            var x = scale * aRoof * (xiPrim +
                                beta1 * Sin(2.0 * xiPrim) * Cosh(2.0 * etaPrim) +
                                beta2 * Sin(4.0 * xiPrim) * Cosh(4.0 * etaPrim) +
                                beta3 * Sin(6.0 * xiPrim) * Cosh(6.0 * etaPrim) +
                                beta4 * Sin(8.0 * xiPrim) * Cosh(8.0 * etaPrim)) +
                                falseNorthing;
            var y = scale * aRoof * (etaPrim +
                                beta1 * Cos(2.0 * xiPrim) * Sinh(2.0 * etaPrim) +
                                beta2 * Cos(4.0 * xiPrim) * Sinh(4.0 * etaPrim) +
                                beta3 * Cos(6.0 * xiPrim) * Sinh(6.0 * etaPrim) +
                                beta4 * Cos(8.0 * xiPrim) * Sinh(8.0 * etaPrim)) +
                                falseEasting;
            return new GridCoordinate(Round(x, 3), Round(y, 3));
        }

        public static Position ToPosition(in GridCoordinate gridCoordinate, in MapProjection projection)
        {
            var axis = projection.Ellipsoid.SemiMajorAxis;
            var centralMeridian = projection.CentralMeridian;
            var falseEasting = projection.FalseEasting;
            var falseNorthing = projection.FalseNorthing;
            var flattening = projection.Ellipsoid.Flattening;
            var x = gridCoordinate.Northing;
            var y = gridCoordinate.Easting;
            var scale = projection.Scale;
            if (centralMeridian == double.MinValue) return Position.Origo;

            var e2 = flattening * (2.0 - flattening);
            var n = flattening / (2.0 - flattening);
            var aRoof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
            var delta1 = n / 2.0 - 2.0 * n * n / 3.0 + 37.0 * n * n * n / 96.0 - n * n * n * n / 360.0;
            var delta2 = n * n / 48.0 + n * n * n / 15.0 - 437.0 * n * n * n * n / 1440.0;
            var delta3 = 17.0 * n * n * n / 480.0 - 37 * n * n * n * n / 840.0;
            var delta4 = 4397.0 * n * n * n * n / 161280.0;
            var Astar = e2 + e2 * e2 + e2 * e2 * e2 + e2 * e2 * e2 * e2;
            var Bstar = -(7.0 * e2 * e2 + 17.0 * e2 * e2 * e2 + 30.0 * e2 * e2 * e2 * e2) / 6.0;
            var Cstar = (224.0 * e2 * e2 * e2 + 889.0 * e2 * e2 * e2 * e2) / 120.0;
            var Dstar = -(4279.0 * e2 * e2 * e2 * e2) / 1260.0;

            const double degToRad = PI / 180;
            var lambdaZero = centralMeridian * degToRad;
            var xi = (x - falseNorthing) / (scale * aRoof);
            var eta = (y - falseEasting) / (scale * aRoof);
            var xiPrim = xi -
                                delta1 * Sin(2.0 * xi) * Cosh(2.0 * eta) -
                                delta2 * Sin(4.0 * xi) * Cosh(4.0 * eta) -
                                delta3 * Sin(6.0 * xi) * Cosh(6.0 * eta) -
                                delta4 * Sin(8.0 * xi) * Cosh(8.0 * eta);
            var etaPrim = eta -
                                delta1 * Cos(2.0 * xi) * Sinh(2.0 * eta) -
                                delta2 * Cos(4.0 * xi) * Sinh(4.0 * eta) -
                                delta3 * Cos(6.0 * xi) * Sinh(6.0 * eta) -
                                delta4 * Cos(8.0 * xi) * Sinh(8.0 * eta);
            var phiStar = Asin(Sin(xiPrim) / Cosh(etaPrim));
            var deltaLambda = Atan(Sinh(etaPrim) / Cos(xiPrim));
            var lonRadian = lambdaZero + deltaLambda;
            var latRadian = phiStar + Sin(phiStar) * Cos(phiStar) *
                                (Astar +
                                 Bstar * Pow(Sin(phiStar), 2) +
                                 Cstar * Pow(Sin(phiStar), 4) +
                                 Dstar * Pow(Sin(phiStar), 6));
            return Position.FromRadians(latRadian, lonRadian);
        }
    }
}
