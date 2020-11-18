namespace Tellurian.Geospatial.Surfaces
{
    public static class StretchExtensions
    {
        public static bool Intersects(this Stretch me, Stretch other) => AreIntersecting(me.AsVector(), other.AsVector());
        internal static Vector AsVector(this Stretch me) => new Vector(me.From.AsCoordinate(), me.To.AsCoordinate());
        internal static Coordinate AsCoordinate(this Position me) => new Coordinate(me.Latitude.Degrees, me.Longitude.Degrees);

        internal static bool AreIntersecting(this Vector v1, Vector v2)
        {
            double d1, d2;
            double a1, a2, b1, b2, c1, c2;

            // Convert vector 1 to a line (line 1) of infinite length.
            // We want the line in linear equation standard form: A*x + B*y + C = 0
            // See: http://en.wikipedia.org/wiki/Linear_equation
            a1 = v1.B.Y - v1.A.Y;
            b1 = v1.A.X - v1.B.X;
            c1 = (v1.B.X * v1.A.Y) - (v1.A.X * v1.B.Y);

            // Every point (x,y), that solves the equation above, is on the line,
            // every point that does not solve it, is not. The equation will have a
            // positive result if it is on one side of the line and a negative one 
            // if is on the other side of it. We insert (x1,y1) and (x2,y2) of vector
            // 2 into the equation above.
            d1 = (a1 * v2.A.X) + (b1 * v2.A.Y) + c1;
            d2 = (a1 * v2.B.X) + (b1 * v2.B.Y) + c1;

            // If d1 and d2 both have the same sign, they are both on the same side
            // of our line 1 and in that case no intersection is possible. Careful, 
            // 0 is a special case, that's why we don't test ">=" and "<=", 
            // but "<" and ">".
            if (d1 > 0 && d2 > 0) return false;
            if (d1 < 0 && d2 < 0) return false;

            // The fact that vector 2 intersected the infinite line 1 above doesn't 
            // mean it also intersects the vector 1. Vector 1 is only a subset of that
            // infinite line 1, so it may have intersected that line before the vector
            // started or after it ended. To know for sure, we have to repeat the
            // the same test the other way round. We start by calculating the 
            // infinite line 2 in linear equation standard form.
            a2 = v2.B.Y - v2.A.Y;
            b2 = v2.A.X - v2.B.X;
            c2 = (v2.B.X * v2.A.Y) - (v2.A.X * v2.B.Y);

            // Calculate d1 and d2 again, this time using points of vector 1.
            d1 = (a2 * v1.A.X) + (b2 * v1.A.Y) + c2;
            d2 = (a2 * v1.B.X) + (b2 * v1.B.Y) + c2;

            // Again, if both have the same sign (and neither one is 0),
            // no intersection is possible.
            if (d1 > 0 && d2 > 0) return false;
            if (d1 < 0 && d2 < 0) return false;

            // If we get here, only two possibilities are left. Either the two
            // vectors intersect in exactly one point or they are collinear, which
            // means they intersect in any number of points from zero to infinite.
            if ((a1 * b2) - (a2 * b1) < double.Epsilon) return false;

            // If they are not collinear, they must intersect in exactly one point.
            return true;
        }
    }

    internal ref struct Vector
    {
        public Vector(Coordinate a, Coordinate b) { A = a; B = b; }
        public readonly Coordinate A;
        public readonly Coordinate B;
    }

    internal readonly ref struct Coordinate
    {
        public Coordinate(double x, double y) { X = x; Y = y; }
        public readonly double X;
        public readonly double Y;
    }
}