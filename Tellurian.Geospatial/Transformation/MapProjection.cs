namespace Tellurian.Geospatial.Transformation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types",
        Justification = "There are no reason for comparing ma projections.")]
    public readonly struct MapProjection
    {
        public MapProjection(EarthEllipsoid ellipsoid, double centralMeridian, double scale, double falseNorthing, double falseEasting)
        {
            Ellipsoid = ellipsoid;
            CentralMeridian = centralMeridian;
            Scale = scale;
            FalseNorthing = falseNorthing;
            FalseEasting = falseEasting;
        }

        public EarthEllipsoid Ellipsoid { get; }
        public double CentralMeridian { get; }
        public double Scale { get; }
        public double FalseNorthing { get; }
        public double FalseEasting { get; }
    }
}
