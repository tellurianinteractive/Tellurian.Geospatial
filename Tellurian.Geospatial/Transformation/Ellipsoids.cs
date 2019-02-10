namespace Tellurian.Geospatial.Transformation
{
    public static class Ellipsoids
    {

        public static EarthEllipsoid Grs80 => EarthEllipsoid.CreateFromSemiAxes(6378137.0, 6356752.31414);
        public static EarthEllipsoid Wgs84 => EarthEllipsoid.CreateFromSemiAxes(6378137.0, 6356752.3142);
    }
}
