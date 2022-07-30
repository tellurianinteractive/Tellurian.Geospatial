using static System.Math;
using static Tellurian.Geospatial.Constants;

namespace Tellurian.Geospatial.Transformation;

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
        var f = projection.Ellipsoid.Flattening;
        var (φ, λ) = geodeticPosition.RadianCoordinates;
        // Prepare ellipsoid-based stuff.
        var e2 = f * (2.0 - f);
        var n = f / (2.0 - f);
        var A = projection.Ellipsoid.SemiMajorAxis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
        var b = (5.0 * e2 * e2 - e2 * e2 * e2) / 6.0;
        var c = (104.0 * e2 * e2 * e2 - 45.0 * e2 * e2 * e2 * e2) / 120.0;
        var d = (1237.0 * e2 * e2 * e2 * e2) / 1260.0;
        var β1 = n / 2.0 - 2.0 * n * n / 3.0 + 5.0 * n * n * n / 16.0 + 41.0 * n * n * n * n / 180.0;
        var β2 = 13.0 * n * n / 48.0 - 3.0 * n * n * n / 5.0 + 557.0 * n * n * n * n / 1440.0;
        var β3 = 61.0 * n * n * n / 240.0 - 103.0 * n * n * n * n / 140.0;
        var β4 = 49561.0 * n * n * n * n / 161280.0;
        // Convert
        var λ0 = projection.CentralMeridian * (Π / 180.0);
        var ΦS = φ - Sin(φ) * Cos(φ) * (e2 +
                            b * Pow(Sin(φ), 2) +
                            c * Pow(Sin(φ), 4) +
                            d * Pow(Sin(φ), 6));
        var Δλ = λ - λ0;
        var Ξ = Atan(Tan(ΦS) / Cos(Δλ));
        var Η = Atanh(Cos(ΦS) * Sin(Δλ));
        var x = projection.Scale * A * (Ξ +
                            β1 * Sin(2.0 * Ξ) * Cosh(2.0 * Η) +
                            β2 * Sin(4.0 * Ξ) * Cosh(4.0 * Η) +
                            β3 * Sin(6.0 * Ξ) * Cosh(6.0 * Η) +
                            β4 * Sin(8.0 * Ξ) * Cosh(8.0 * Η)) +
                            projection.FalseNorthing;
        var y = projection.Scale * A * (Η +
                            β1 * Cos(2.0 * Ξ) * Sinh(2.0 * Η) +
                            β2 * Cos(4.0 * Ξ) * Sinh(4.0 * Η) +
                            β3 * Cos(6.0 * Ξ) * Sinh(6.0 * Η) +
                            β4 * Cos(8.0 * Ξ) * Sinh(8.0 * Η)) +
                            projection.FalseEasting;
        return new GridCoordinate(Round(x, 3), Round(y, 3));
    }

    public static Position ToPosition(in GridCoordinate gridCoordinate, in MapProjection projection)
    {
        var (X, Y) = gridCoordinate.Coordinates;
        var f = projection.Ellipsoid.Flattening;
        if (projection.CentralMeridian == double.MinValue) return Position.Origo;

        var e2 = f * (2.0 - f);
        var n = f / (2.0 - f);
        var A = projection.Ellipsoid.SemiMajorAxis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
        var Δ1 = n / 2.0 - 2.0 * n * n / 3.0 + 37.0 * n * n * n / 96.0 - n * n * n * n / 360.0;
        var Δ2 = n * n / 48.0 + n * n * n / 15.0 - 437.0 * n * n * n * n / 1440.0;
        var Δ3 = 17.0 * n * n * n / 480.0 - 37 * n * n * n * n / 840.0;
        var Δ4 = 4397.0 * n * n * n * n / 161280.0;
        var a = e2 + e2 * e2 + e2 * e2 * e2 + e2 * e2 * e2 * e2;
        var b = -(7.0 * e2 * e2 + 17.0 * e2 * e2 * e2 + 30.0 * e2 * e2 * e2 * e2) / 6.0;
        var c = (224.0 * e2 * e2 * e2 + 889.0 * e2 * e2 * e2 * e2) / 120.0;
        var d = -(4279.0 * e2 * e2 * e2 * e2) / 1260.0;

        var λ0 = projection.CentralMeridian * (PI / 180);
        var ξ = (X - projection.FalseNorthing) / (projection.Scale * A);
        var η = (Y - projection.FalseEasting) / (projection.Scale * A);
        var Ξ = ξ -
            Δ1 * Sin(2.0 * ξ) * Cosh(2.0 * η) -
            Δ2 * Sin(4.0 * ξ) * Cosh(4.0 * η) -
            Δ3 * Sin(6.0 * ξ) * Cosh(6.0 * η) -
            Δ4 * Sin(8.0 * ξ) * Cosh(8.0 * η);
        var Η = η -
            Δ1 * Cos(2.0 * ξ) * Sinh(2.0 * η) -
            Δ2 * Cos(4.0 * ξ) * Sinh(4.0 * η) -
            Δ3 * Cos(6.0 * ξ) * Sinh(6.0 * η) -
            Δ4 * Cos(8.0 * ξ) * Sinh(8.0 * η);
        var Φ = Asin(Sin(Ξ) / Cosh(Η));
        var Δλ = Atan(Sinh(Η) / Cos(Ξ));
        var λ = λ0 + Δλ;
        var φ = Φ + Sin(Φ) * Cos(Φ) * (a + b * Pow(Sin(Φ), 2) + c * Pow(Sin(Φ), 4) + d * Pow(Sin(Φ), 6));
        return Position.FromRadians(φ, λ);
    }
}
