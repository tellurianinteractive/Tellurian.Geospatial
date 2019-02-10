namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// A set of frequently used projections.
    /// </summary>
    public static class MapProjections
    {
        public static readonly MapProjection Rt90 = new MapProjection(Ellipsoids.Grs80, 15.8062845294444, 1.00000561024, -667.711, 1500064.274);
        public static readonly MapProjection Sweref99TM = new MapProjection(Ellipsoids.Grs80, 15.0, 0.9996, 0.0, 500000.0);
        public static readonly MapProjection Utm32 = new MapProjection(Ellipsoids.Grs80, 9.0, 0.9996, 0.0, 500000.0);
        public static readonly MapProjection Utm33 = new MapProjection(Ellipsoids.Grs80, 15.0, 0.9996, 0.0, 500000.0);
        public static readonly MapProjection Utm34 = new MapProjection(Ellipsoids.Grs80, 21.0, 0.9996, 0.0, 500000.0);
        public static readonly MapProjection Utm35 = new MapProjection(Ellipsoids.Grs80, 27.0, 0.9996, 0.0, 500000.0);
    }
}
