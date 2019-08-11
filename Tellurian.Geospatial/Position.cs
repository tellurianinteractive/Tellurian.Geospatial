using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using static System.Math;
using static Tellurian.Geospatial.Constants;

namespace Tellurian.Geospatial
{
    /// <summary>
    /// Reporesents a coordintate using <see cref="Latitude"/> and <see cref="Longitude"/>.
    /// </summary>
    /// <remarks>
    /// This implementation uses part of 'Latitude/longitude spherical geodesy tools'
    /// MIT Licence (c) Chris Veness 2002-2019
    /// https://www.movable-type.co.uk/scripts/latlong.html                      
    /// https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html 
    /// </remarks>
    [DataContract]
    public readonly struct Position : IEquatable<Position>
    {
        [DataMember(Name = "Latitude")]
        readonly Latitude _latitude;

        [DataMember(Name = "Longitude")]
        readonly Longitude _longitude;

        public static Position Origo => Position.FromDegrees(0, 0);

        public static Position FromDegrees(double latitude, double longitude) => new Position(latitude, longitude);
  
        public static Position FromRadians(double latitude, double longitude) => new Position(latitude * 180 / Math.PI, longitude * 180 / Math.PI);

        private Position(in double latitude, in double longitude)
        {
            _latitude = Latitude.FromDegrees(latitude);
            _longitude = Longitude.FromDegrees(longitude);
        }

        public bool IsOrigo => _latitude.IsZero && _longitude.IsZero;
        public Latitude Latitude => _latitude;
        public Longitude Longitude => _longitude; 

        /// <summary>
        /// Calculate the destina­tion point travelling along a (shortest distance) great circle arc.
        /// </summary>
        /// <param name="initialBearing"><see cref="Angle"/>Angle</param> of initial bearing.
        /// <param name="distance"><see cref="Distance"/></param> to destination.
        /// <returns><see cref="Position"/> for destination.</returns>
        /// <remarks>
        /// Formula from https://www.movable-type.co.uk/scripts/latlong.html
        /// </remarks>
        public Position Destination(in Angle initialBearing, in Distance distance )
        {
            var φ1 = Latitude.Radians;
            var λ1 = Longitude.Radians;
            var δ = initialBearing.Radians;
            var R = EarthMeanRadiusMeters;
            var d = distance.Meters;

            var φ2 = Asin(Sin(φ1) * Cos(d / R) + Cos(φ1) * Sin(d / R) * Cos(δ));
            var λ2 = λ1 + Atan2(Sin(δ) * Sin(d / R) * Cos(φ1), Cos(d / R) - Sin(φ1) * Sin(φ2));
            return FromRadians(φ2, λ2);
        }

        public static bool operator == (in Position one, in Position another) => one.Equals(another);
        public static bool operator !=(in Position one, in Position another) => ! one.Equals(another);
        public static Position operator -(in Position one, in Position another) => FromDegrees(one.Latitude.Degrees - another.Latitude.Degrees, one.Longitude.Degrees - another.Longitude.Degrees);
        public bool Equals(Position other) => Latitude == other.Latitude && Longitude == other.Longitude;
        public override bool Equals(object obj) => obj is Position && Equals((Position)obj);

        public override string ToString() => string.Format(CultureInfo.InvariantCulture,"{0},{1}", Latitude.Degrees, Longitude.Degrees);
        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => _latitude.GetHashCode() / 2 + _longitude.GetHashCode() / 2;
    }
}
