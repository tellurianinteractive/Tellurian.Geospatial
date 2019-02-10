using System;
using System.Diagnostics.CodeAnalysis;

namespace Tellurian.Geospatial.Transformation
{
    public struct GridCoordinate : IEquatable<GridCoordinate>
    {
        public GridCoordinate(double northing, double easting)
        {
            Northing = northing;
            Easting = easting;
        }

        public double Easting { get; }
        public double Northing { get; }

        public bool Equals(GridCoordinate other)
        {
            return Northing == other.Northing && Easting == other.Easting;
        }

        public override bool Equals(object obj)
        {
            return obj is GridCoordinate && Equals((GridCoordinate)obj);
        }

        public static bool operator ==(GridCoordinate one, GridCoordinate another)
        {
            return one.Equals(another);
        }
        public static bool operator !=(GridCoordinate one, GridCoordinate another)
        {
            return !one.Equals(another);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Easting.GetHashCode();
        }
    }
}
