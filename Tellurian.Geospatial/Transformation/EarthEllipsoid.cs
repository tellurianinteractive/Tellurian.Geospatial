namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// Representation of the earths spherical form.
    /// </summary>
    /// <remarks>
    /// References: https://en.wikipedia.org/wiki/Earth_ellipsoid
    /// </remarks>
    public struct EarthEllipsoid
    {
        public static EarthEllipsoid CreateFromSemiAxes(double semiMajorAxis, double semiMinorAxis) => new EarthEllipsoid(semiMajorAxis, semiMinorAxis);
        public static EarthEllipsoid CreateFromSemiMajorAxisAndFlattening(double semiMajorAxis, double flattening)
        {
            var semiMinorAxis = semiMajorAxis - flattening * semiMajorAxis;
            return new EarthEllipsoid(semiMajorAxis, semiMinorAxis);
        }

        private EarthEllipsoid(double semiMajorAxis, double semiMinorAxis)
        {
            SemiMajorAxis = semiMajorAxis;
            SemiMinorAxis = semiMinorAxis;
            Flattening = (SemiMajorAxis - SemiMinorAxis) / SemiMajorAxis;
        }

        public double Flattening { get; }
        public double SemiMajorAxis { get; }
        public double SemiMinorAxis { get; }
    }
}
