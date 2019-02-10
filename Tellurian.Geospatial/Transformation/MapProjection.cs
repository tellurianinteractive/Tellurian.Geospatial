namespace Tellurian.Geospatial.Transformation
{
    public sealed class MapProjection
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
        public double Scale { get; } = 1.0;
        public double FalseNorthing { get; }
        public double FalseEasting { get; }
    }
}
