namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// A set of frequently used projections.
    /// </summary>
    public static class MapProjections
    {
        public static readonly MapProjection Rt90 = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 15.8062845294444, Scale = 1.00000561024, FalseNorthing = -667.711, FalseEasting = 1500064.274 };
        public static readonly MapProjection Sweref99TM = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 15.0, Scale = 0.9996, FalseNorthing = 0.0, FalseEasting = 500000.0 };
        public static readonly MapProjection Utm32 = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 9.0, Scale = 0.9996, FalseNorthing = 0.0, FalseEasting = 500000.0 };
        public static readonly MapProjection Utm33 = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 15.0, Scale = 0.9996, FalseNorthing = 0.0, FalseEasting = 500000.0 };
        public static readonly MapProjection Utm34 = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 21.0, Scale = 0.9996, FalseNorthing = 0.0, FalseEasting = 500000.0 };
        public static readonly MapProjection Utm35 = new MapProjection { Ellipsoid = Ellipsoids.Grs80, CentralMeridian = 27.0, Scale = 0.9996, FalseNorthing = 0.0, FalseEasting = 500000.0 };
    }
}
