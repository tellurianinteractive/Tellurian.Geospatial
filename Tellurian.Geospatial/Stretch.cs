using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
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
    public struct Stretch : IEquatable<Stretch>
    {
        private static readonly IDistanceCalculator DefaultDistanceCalculator = new HaversineDistanceCalculator();

        [DataMember(Name = "From", IsRequired = true, Order = 1)]
        private readonly Position _From;

        [DataMember(Name = "To", IsRequired = true, Order = 2)]
        private readonly Position _To;

        private readonly IDistanceCalculator _DistanceCalculator;
        private Distance? _Distance;
        private Angle? _InitialBearing;
        private Angle? _FinalBearing;
        private Angle? _Direction;

        public static Stretch Between(in Position from, in Position to) => new Stretch(from, to);
        public static Stretch Between(in Position from, in Position to, in IDistanceCalculator distanceCalculator) => new Stretch(from, to, distanceCalculator);
        
        [JsonConstructor]
        public Stretch(Position from, Position to) : this(from, to, DefaultDistanceCalculator) { }

        private Stretch(Position from, Position to, IDistanceCalculator distanceCalculator)
        {
            _From = from;
            _To = to;
            _DistanceCalculator = distanceCalculator ?? DefaultDistanceCalculator;
            _Direction = null;
            _Distance = null;
            _InitialBearing = null;
            _FinalBearing = null;
        }
        [JsonPropertyName("from")]
        public Position From => _From;
        [JsonPropertyName("to")]
        public Position To => _To;
        [JsonIgnore]
        public bool IsZero => From == To;
        [JsonIgnore]
        public Angle Direction => _Direction ?? (_Direction = RhumbBearing()).Value;
        [JsonIgnore]
        public Angle InitialBearing => _InitialBearing ?? (_InitialBearing = GetInitialBearing()).Value;
        [JsonIgnore]
        public Angle FinalBearing => _FinalBearing ?? (_FinalBearing = Angle.FromDegrees((Inverse.InitialBearing.Degrees + 180) % 360)).Value;
        [JsonIgnore]
        public Distance Distance => _Distance ?? (_Distance = _DistanceCalculator.GetDistance(From, To)).Value;
        [JsonIgnore]
        public Stretch Inverse => new Stretch(_To, _From);
        [JsonIgnore]
        public bool IsEastWestLine => _From.Latitude.Equals(_To.Latitude);

        public Distance GetDistance(IDistanceCalculator useDistanceCalculator) => useDistanceCalculator.GetDistance(From, To);

        private Angle GetInitialBearing()
        {
            var lat1 = _From.Latitude.Radians;
            var lat2 = _To.Latitude.Radians;
            var dLon = _To.Longitude.Radians - _From.Longitude.Radians;
            var y = Sin(dLon) * Cos(lat2);
            var x = (Cos(lat1) * Sin(lat2)) - (Sin(lat1) * Cos(lat2) * Cos(dLon));
            var b = Math.Atan2(y, x);
            return Angle.FromRadians((b + PI2) % PI2);
        }

        internal Angle RhumbBearing()
        {
            var lat2 = _To.Latitude.Radians;
            var lat1 = _From.Latitude.Radians;
            var dPhi = Log(Tan((lat2 / 2) + (Math.PI / 4)) / Tan((lat1 / 2) + (Math.PI / 4)));
            var dLon = _To.Longitude.Radians - _From.Longitude.Radians;
            var b = Atan2(dLon, dPhi);
            return Angle.FromRadians((b + PI2) % PI2);
        }

        public override bool Equals(object obj) => obj is Stretch stretch && Equals(stretch);
        public bool Equals(Stretch other) => From.Equals(other.From) && To.Equals(other.To);

        public static bool operator ==(in Stretch value1, in Stretch value2) => value1.Equals(value2);
        public static bool operator !=(in Stretch value1, in Stretch value2) => !value1.Equals(value2);

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => From.GetHashCode();
    }

    public static class StretchExtensions
    {
        /// <summary>
        /// The distance of a point from a great-circle path (sometimes called cross track error). 
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public static Distance CrossTrackDistance(this Stretch me, Position at)
        {
            const double R = EarthMeanRadiusMeters;
            var s = Stretch.Between(me.From, at);
            var δ13 = s.Distance.Meters;
            var θ13 = s.InitialBearing.Radians;
            var θ12 = me.InitialBearing.Radians;
            var d = Asin(Sin(δ13 / R) * Sin(θ13 - θ12)) * R;
            return Distance.FromMeters(Abs(d));
        }

        public static bool IsOnTrack(this Stretch me, Position at, Distance maxOffTrackDistance) => me.CrossTrackDistance(at) <= maxOffTrackDistance;

        /// <summary>
        /// Checks if a <see cref="Position"/> is anywhere between the strecth's two ends, regardless of offtrack distance.
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        public static bool IsBetweenEnds(this Stretch me, Position at) => at.IsBetween(me.From, me.To);

        /// <summary>
        ///     ''' Calculates the angle between the two directions starting from the position
        ///     ''' and ending in the start and end point of the stretch.
        ///     ''' </summary>
        ///     ''' <param name="at">The <see cref="Position">position</see> to get the angle for.</param>
        ///     ''' <returns>The angle between the two directions starting from the position
        ///     ''' and ending in the start and end point of the stretch.</returns>
        ///     ''' <remarks></remarks>
        public static Angle MinAngle(this Stretch me, Position at)
        {
            var a1 = Stretch.Between(at, me.From).Direction;
            var a2 = Stretch.Between(at, me.To).Direction;
            return a1.Min(a2);
        }

        /// <summary>
        ///     ''' The along-track distance, calculated from the start point to the closest point on the path to the given point.
        ///     ''' </summary>
        ///     ''' <param name="at">The <see cref="Position">position</see> to caclulate the on track distance for.</param>
        ///     ''' <returns>The on track distance from the start of the stretch.</returns>
        ///     ''' <remarks></remarks>
        public static Distance OnTrackDistance(this Stretch me, Position at)
        {
            const double r = EarthMeanRadiusMeters / 1000;
            var s = Stretch.Between(me.From, at);
            var d13 = s.Distance.Meters;
            var dXt = me.CrossTrackDistance(at).Meters;
            var d = Acos(Cos(d13 / r) / Cos(dXt / r)) * r;
            return Distance.FromMeters(Abs(d));
        }
    }
}
