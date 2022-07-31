using Tellurian.Geospatial.Transformation;

namespace Tellurian.Geospatial.NVectors;
public struct NVector
{
    internal const int Length = 3;

    public NVector(double x, double y, double z, double depth = 0.0)
    {
        X = x;
        Y = y;
        Z = z;
        Depth = depth;
    }
    internal NVector(double[] values, double depth = 0.0)
    {
        X = values[0];
        Y = values[1];
        Z = values[2];
        Depth = depth;
    }
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
    public double Depth { get; init; }

    internal double[] Values => new double[] { X, Y, Z };
}

public static class NVectorExtensions
{
    private static Matrix EarthAxisNorthPoleZ => new(new double[3, 3]
    {
        { 0, 0, 1 },
        { 0, 1, 0 },
        { -1, 0, 0 }
    });

    private static readonly Matrix DefaultEarthAxis = EarthAxisNorthPoleZ;

    public static NVector AsNVector(this Position position)
    {
        var (latitude, longitude) = position.Radians;
        return DefaultEarthAxis.Transposed.MultiplyWith(new NVector(Math.Sin(latitude), Math.Sin(longitude) * Math.Cos(latitude), -Math.Cos(longitude) * Math.Cos(latitude)));
    }
    /// <summary>
    /// Converts <see cref="NVector"/> to <see cref="Position"/> with latitude and longitude.
    /// </summary>
    /// <param name="vector">The <see cref="NVector"/> to convert.</param>
    /// <returns></returns>
    public static Position AsPosition(this NVector vector)
    {
        var correctedVector = DefaultEarthAxis.MultiplyWith(vector);
        var longitude = Math.Atan2(correctedVector.Y, -correctedVector.Z);
        var equatorialEomponent = Math.Sqrt(Math.Pow(correctedVector.Y, 2) + Math.Pow(correctedVector.Z, 2));
        var latitude = Math.Atan2(correctedVector.X, equatorialEomponent);
        return Position.FromRadians(latitude, longitude);
    }

    public static NVector To(this NVector start, NVector end, EarthEllipsoid? ellipsoid = null)
    {
        var ee = ellipsoid ?? Ellipsoids.Wgs84;
        var s = start.ToCartesianVector(ee);
        var e = end.ToCartesianVector(ee);
        return new(
            new double[NVector.Length] {
                -s.X + e.X,
                -s.Y + e.Y,
                -s.Z + e.Z }
            );
    }

    internal static NVector ToCartesianVector(this NVector vector, EarthEllipsoid ellipsoid)
    {
        var bodyEarthPosition = Unit(DefaultEarthAxis.MultiplyWith(vector));
        var tmp = Math.Pow(1.0 - ellipsoid.Flattening, 2);
        var denominator = Math.Sqrt(Math.Pow(bodyEarthPosition.X, 2) + Math.Pow(bodyEarthPosition.Y, 2) / tmp + Math.Pow(bodyEarthPosition.Z, 2) / tmp);
        var originPosition = new NVector(
            new double[3] {
                ellipsoid.SemiMinorAxis / denominator * bodyEarthPosition.X,
                ellipsoid.SemiMinorAxis / denominator * bodyEarthPosition.Y / tmp,
                ellipsoid.SemiMinorAxis / denominator * bodyEarthPosition.Z / tmp });
        var tempVector = new NVector(
            new double[3] {
                originPosition.X - bodyEarthPosition.X * vector.Depth,
                originPosition.Y - bodyEarthPosition.Y * vector.Depth,
                originPosition.Z - bodyEarthPosition.Z * vector.Depth, });
        return DefaultEarthAxis.Transposed.MultiplyWith(tempVector);
    }

    internal static NVector FromCartesianVector(this NVector vector, EarthEllipsoid ellipsoid)
    {
        var correctedVector = DefaultEarthAxis.MultiplyWith(vector);
        var e2 = 2 * ellipsoid.Flattening - Math.Pow(ellipsoid.Flattening, 2);
        var R2 = Math.Pow(correctedVector.Y, 2) + Math.Pow(correctedVector.Z, 2);
        var R = Math.Sqrt(R2);
        var p = R2 / Math.Pow(ellipsoid.SemiMajorAxis, 2);
        var q = (1 - e2) / Math.Pow(ellipsoid.SemiMajorAxis, 2) * Math.Pow(correctedVector.X, 2);
        var r = (p + q - Math.Pow(e2, 2)) / 6.0;
        var s = Math.Pow(e2, 2) * p * q / (4.0 * Math.Pow(r, 3));
        var t = Math.Pow((1 + s + Math.Sqrt(s * (2 + s))), 1.0 / 3.0);
        var u = r * (1 + t + 1 / t);
        var v = Math.Sqrt(Math.Pow(u, 2) + Math.Pow(e2, 2) * q);
        var w = e2 * (u + v - q) / (2.0 * v);
        var k = Math.Sqrt(u + v + Math.Pow(w, 2)) - w;
        var d = k * R / (k + e2);
        var height = (k + e2 - 1) / k * Math.Sqrt(Math.Pow(d, 2) + Math.Pow(correctedVector.X, 2));
        var temp = 1 / Math.Sqrt(Math.Pow(d, 2) + Math.Pow(correctedVector.X, 2));
        var x = temp * correctedVector.X;
        var y = temp * k / (k + e2) * correctedVector.Y;
        var z = temp * k / (k + e2) * correctedVector.Z;
        return DefaultEarthAxis.Transposed.MultiplyWith(new NVector(x, y, z, -height));
    }

    internal static NVector MultiplyWith(this Matrix M, NVector v)
    {
        var m = NVector.Length;
        var n = Matrix.Length;
        var v2 = new double[m];
        for (int i = 0; i < m; i++)
        {
            double sum = 0;
            for (int j = 0; j < n; j++)
            {
                sum += M.Values[i, j] * v.Values[j];
            }
            v2[i] = sum;
        }
        return new(v2);
    }

    private static NVector Unit(this NVector vector)
    {
        var currentNorm = Norm(vector);
        return new(vector.X / currentNorm, vector.Y / currentNorm, vector.Z / currentNorm);
    }

    private static double Norm(this NVector vector)
    {
        double sum = 0.0;
        for (int i = 0; i < NVector.Length; i++)
        {
            sum += Math.Pow(vector.Values[i], 2);
        }
        return Math.Sqrt(sum);
    }
}
