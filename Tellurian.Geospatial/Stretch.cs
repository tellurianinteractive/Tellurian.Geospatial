using System;
using System.Runtime.CompilerServices;
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
    public sealed record Stretch
    {
        private static readonly IDistanceCalculator DefaultDistanceCalculator = new HaversineDistanceCalculator();

        private readonly IDistanceCalculator _DistanceCalculator;
        // We cash properties that may be called more than one time.
        private Distance? _Distance;
        private Angle? _InitialBearing;
        private Angle? _FinalBearing;
        private Angle? _Direction;

        public static Stretch Between(in Position from, in Position to) => new(from, to);
        public static Stretch Between(in Position from, in Position to, IDistanceCalculator distanceCalculator) => new(from, to, distanceCalculator);

        [JsonConstructor]
        public Stretch(Position from, Position to) : this(from, to, DefaultDistanceCalculator) { }


        private Stretch(in Position from, in Position to, IDistanceCalculator distanceCalculator)
        {
            From = from;
            To = to;
            _DistanceCalculator = distanceCalculator ?? DefaultDistanceCalculator;
            _Direction = null;
            _Distance = null;
            _InitialBearing = null;
            _FinalBearing = null;
        }

        [DataMember(Name = "From", IsRequired = true, Order = 1)]
        [JsonPropertyName("from")]
        public Position From { get; init; }
        [DataMember(Name = "To", IsRequired = true, Order = 2)]
        [JsonPropertyName("to")]
        public Position To { get; init; }

        [JsonIgnore]
        public bool IsZero => From == To;
        [JsonIgnore]
        public Angle Direction => IsZero ? Angle.Undefined : _Direction ?? (_Direction = RhumbBearing()).Value;
        [JsonIgnore]
        public Angle InitialBearing => IsZero ? Angle.Undefined : _InitialBearing ?? (_InitialBearing = GetInitialBearing()).Value;
        [JsonIgnore]
        public Angle FinalBearing => IsZero ? Angle.Undefined : _FinalBearing ?? (_FinalBearing = Angle.FromDegrees((Inverse.InitialBearing.Degrees + 180) % 360)).Value;
        [JsonIgnore]
        public Distance Distance => IsZero ? Distance.Zero : _Distance ?? (_Distance = _DistanceCalculator.GetDistance(From, To)).Value;
        [JsonIgnore]
        public Stretch Inverse => IsZero ? this : new(To, From);
        [JsonIgnore]
        public bool IsEastWestLine => !IsZero && From.Latitude.Equals(To.Latitude);

        public Distance GetDistance(IDistanceCalculator usingDistanceCalculator) => usingDistanceCalculator.GetDistance(From, To);



        private Angle GetInitialBearing()
        {
            var (φ1, λ1) = From.RadianCoordinates;
            var (φ2, λ2) = To.RadianCoordinates;
            var Δλ = λ2 - λ1;
            var y = Sin(Δλ) * Cos(φ2);
            var x = (Cos(φ1) * Sin(φ2)) - (Sin(φ1) * Cos(φ2) * Cos(Δλ));
            var b = Atan2(y, x);
            return Angle.FromRadians((b + Π2) % Π2);
        }

        internal Angle RhumbBearing()
        {
            var φ2 = To.Latitude.Radians;
            var φ1 = From.Latitude.Radians;
            var Δλ = To.Longitude.Radians - From.Longitude.Radians;
            var ΔΦ = Log(Tan((φ2 / 2) + (Π / 4)) / Tan((φ1 / 2) + (Π / 4)));
            var b = Atan2(Δλ, ΔΦ);
            return Angle.FromRadians((b + Π2) % Π2);
        }

        public override string ToString() => $"From {From} to {To}";
        public bool Equals(Stretch? other) => other is not null && other.From.Equals(From) && other.To.Equals(To);
        public override int GetHashCode() => HashCode.Combine(From, To);
    }

    public static class StretchExtensions
    {
        /// <summary>
        /// The distance of a point from a great-circle path (sometimes called cross track error). 
        /// </summary>
        /// <param name="at"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Distance CrossTrackDistance(this Stretch me, Position at)
        {
            const double R = EarthMeanRadiusMeters;
            var s = Stretch.Between(me.From, at);
            var δ = s.Distance.Meters;
            var θ1 = s.InitialBearing.Radians;
            var θ2 = me.InitialBearing.Radians;
            var d = Asin(Sin(δ / R) * Sin(θ1 - θ2)) * R;
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
            var θ1 = Stretch.Between(at, me.From).Direction;
            var θ2 = Stretch.Between(at, me.To).Direction;
            return θ1.Min(θ2);
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
            var δ = s.Distance.Meters;
            var Δδ = me.CrossTrackDistance(at).Meters;
            var δt = Acos(Cos(δ / r) / Cos(Δδ / r)) * r;
            return Distance.FromMeters(Abs(δt));
        }
    }
}
