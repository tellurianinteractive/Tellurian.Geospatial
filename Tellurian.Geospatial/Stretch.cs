using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Tellurian.Geospatial.DistanceCalculators;
using static System.Math;
using static Tellurian.Geospatial.Constants;

namespace Tellurian.Geospatial
{
    /// <summary>
    /// Reporesents a stretch between two <see cref="Position">positions</see>.
    /// </summary>
    /// <remarks>
    /// This implementation uses part of 'Latitude/longitude spherical geodesy tools'
    /// MIT Licence (c) Chris Veness 2002-2019
    /// https://www.movable-type.co.uk/scripts/latlong.html                      
    /// https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html 
    /// </remarks>
    [DataContract]
    public sealed class Stretch
    {
        private static readonly IDistanceCalculator DefaultDistanceCalculator = new HaversineDistanceCalculator();

        [DataMember(Name = "From", IsRequired = true, Order = 1)]
        private readonly Position _From;
        [DataMember(Name = "To", IsRequired = true, Order = 1)]
        private readonly Position _To;

        private readonly IDistanceCalculator _DistanceCalculator;
        private readonly Lazy<Distance> _Distance;
        private readonly Lazy<Angle> _InitialBearing;
        private readonly Lazy<Angle> _FinalBearing;
        private readonly Lazy<Angle> _Direction;

        public static Stretch Between(Position from, Position to) => new Stretch(from, to);
        public static Stretch Between(Position from, Position to, IDistanceCalculator distanceCalculator) => new Stretch(from, to, distanceCalculator);

        private Stretch(Position from, Position to) : this(from, to, DefaultDistanceCalculator) { }

        private Stretch(Position from, Position to, IDistanceCalculator distanceCalculator)
        {
            _From = from;
            _To = to;
            _DistanceCalculator = distanceCalculator ?? DefaultDistanceCalculator;
            _Distance = new Lazy<Distance>(() => _DistanceCalculator.GetDistance(from, to));
            _Direction = new Lazy<Angle>(() => RhumbBearing());
            _InitialBearing = new Lazy<Angle>(() => GetInitialBearing());
            _FinalBearing = new Lazy<Angle>(() => Angle.FromDegrees((Inverse.InitialBearing.Degrees + 180) % 360));
        }

        public bool IsZero => From == To;
        public Angle Direction => _Direction.Value;
        public Angle InitialBearing => _InitialBearing.Value;
        public Angle FinalBearing => _FinalBearing.Value;
        public Position From => _From;
        public Position To => _To;
        public Distance Distance => _Distance.Value;
        public Stretch Inverse => new Stretch(_To, _From);
        public bool IsEastWestLine => _From.Latitude.Equals(_To.Latitude);


        /// <summary>
        /// The distance of a point from a great-circle path (sometimes called cross track error). 
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public Distance CrossTrackDistance(Position at)
        {
            const double R = EarthMeanRadiusMeters;
            var s = new Stretch(_From, at);
            var δ13 = s.Distance.Meters;
            var θ13 = s.InitialBearing.Radians;
            var θ12 = InitialBearing.Radians;
            var d = Asin(Sin(δ13 / R) * Sin(θ13 - θ12)) * R;
            return Distance.FromMeters(Abs(d));
        }
        /// <summary>
        ///     ''' Calculates the angle between the two directions starting from the position
        ///     ''' and ending in the start and end point of the stretch.
        ///     ''' </summary>
        ///     ''' <param name="at">The <see cref="Position">position</see> to get the angle for.</param>
        ///     ''' <returns>The angle between the two directions starting from the position
        ///     ''' and ending in the start and end point of the stretch.</returns>
        ///     ''' <remarks></remarks>
        public Angle MinAngle(Position at)
        {
            var a1 = new Stretch(at, From).Direction;
            var a2 = new Stretch(at, To).Direction;
            return a1.Min(a2);
        }

        /// <summary>
        ///     ''' The along-track distance, calculated from the start point to the closest point on the path to the given point.
        ///     ''' </summary>
        ///     ''' <param name="at">The <see cref="Position">position</see> to caclulate the on track distance for.</param>
        ///     ''' <returns>The on track distance from the start of the stretch.</returns>
        ///     ''' <remarks></remarks>
        public Distance OnTrackDistance(Position at)
        {
            const double r = EarthMeanRadiusMeters / 1000;
            var s = new Stretch(From, at);
            var d13 = s.Distance.Meters;
            var dXt = CrossTrackDistance(at).Meters;
            var d = Acos(Cos(d13 / r) / Cos(dXt / r)) * r;
            return Distance.FromMeters(Abs(d));
        }

        private Angle GetInitialBearing()
        {
            var lat1 = _From.Latitude.Radians;
            var lat2 = _To.Latitude.Radians;
            var dLon = _To.Longitude.Radians - _From.Longitude.Radians;
            var y = Sin(dLon) * Cos(lat2);
            var x = Cos(lat1) * Sin(lat2) - Sin(lat1) * Cos(lat2) * Cos(dLon);
            var b = Math.Atan2(y, x);
            return Angle.FromRadians((b + PI2) % PI2);
        }

        internal Angle RhumbBearing()
        {
            var lat2 = _To.Latitude.Radians;
            var lat1 = _From.Latitude.Radians;
            var dPhi = Log(Tan(lat2 / 2 + Math.PI / 4) / Tan(lat1 / 2 + Math.PI / 4));
            var dLon = _To.Longitude.Radians - _From.Longitude.Radians;
            var b = Atan2(dLon, dPhi);
            return Angle.FromRadians((b + PI2) % PI2);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Stretch;
            if (other is null) return false;
            return From == other.From && To == other.To;
        }

        public static bool operator ==(Stretch value1, Stretch value2)
        {
            if (value1 is null || value2 is null) return false;
            return value1.Equals(value2);
        }

        public static bool operator !=(Stretch value1, Stretch value2)
        {
            if (value1 is null || value2 is null) return true;
            return !value1.Equals(value2);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return From.GetHashCode();
        }
    }
}
