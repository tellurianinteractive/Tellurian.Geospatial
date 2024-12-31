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
    public struct Stretch(in Position from, in Position to, IDistanceCalculator? distanceCalculator = null) : IEquatable<Stretch>
    {
        private static readonly IDistanceCalculator DefaultDistanceCalculator = new HaversineDistanceCalculator();
        private readonly IDistanceCalculator _DistanceCalculator = distanceCalculator ?? DefaultDistanceCalculator;

        public static Stretch Between(in Position from, in Position to, IDistanceCalculator? distanceCalculator = null) =>
            new(from, to, distanceCalculator);
        public static Stretch Along(in Position from, in Vector vector, IDistanceCalculator? distanceCalculator = null) =>
            new(from, from.Destination(vector), distanceCalculator);

        [JsonConstructor]
        public Stretch(Position from, Position to) : this(from, to, DefaultDistanceCalculator) { }

        [DataMember(Name = "From", IsRequired = true, Order = 1)]
        [JsonPropertyName("from")]
        public Position From { get; init; } = from;

        [DataMember(Name = "To", IsRequired = true, Order = 2)]
        [JsonPropertyName("to")]
        public Position To { get; init; } = to;
  
        // We cash values for methods that may be called more than one time.
        private Distance? _Distance = null;
        private Angle? _Direction = null;
        private Angle? _InitialBearing = null;
        private Angle? _FinalBearing = null;

        /// <summary>
        /// True if start and end <see cref="Position"/> are equal; otherwise false.
        /// </summary>
        [JsonIgnore]
        public readonly bool IsZero => From == To;

        /// <summary>
        /// Gets the <see cref="Angle"/> for the rhumb bearing.
        /// The result is cached, so repeatedly getting this property does not recalculate the value.
        /// </summary>
        [JsonIgnore]
        public Angle Direction => IsZero ? Angle.Undefined : _Direction ?? (_Direction = RhumbBearing()).Value;

        /// <summary>
        /// Gets the <see cref="Angle"/> for the initial bearing.
        /// The result is cached, so repeatedly getting this property does not recalculate the value.
        /// </summary>
        [JsonIgnore]
        public Angle InitialBearing => IsZero ? Angle.Undefined : _InitialBearing ?? (_InitialBearing = GetInitialBearing()).Value;

        /// <summary>
        /// Gets the <see cref="Angle"/> for the final bearing.
        /// The result is cached, so repeatedly getting this property does not recalculate the value.
        /// </summary>
        [JsonIgnore]
        public Angle FinalBearing => IsZero ? Angle.Undefined : _FinalBearing ?? (_FinalBearing = Angle.FromDegrees((Inverse.InitialBearing.Degrees + 180) % 360)).Value;

        /// <summary>
        /// Gets the <see cref="Geospatial.Distance"/> using the <see cref="IDistanceCalculator"/> provided in the constructor 
        /// or otherwise the default <see cref="HaversineDistanceCalculator"/>.
        /// The result is cached, so repeatedly getting this property does not recalculate the value.
        /// </summary>
        [JsonIgnore]
        public Distance Distance => IsZero ? Distance.Zero : _Distance ?? (_Distance = _DistanceCalculator.GetDistance(From, To)).Value;

        /// <summary>
        /// Gets a new <see cref="Stretch"/> that is the inverse of this.
        /// Each call creates a new instance of the inverse <see cref="Stretch"/>, except when this <see cref="IsZero"/>, then this instance is returned.
        /// </summary>
        [JsonIgnore]
        public readonly Stretch Inverse => IsZero ? this : new(To, From);

        [JsonIgnore]
        public readonly bool IsEastWestLine => !IsZero && From.Latitude.Equals(To.Latitude);

        /// <summary>
        /// Calculates the <see cref="Geospatial.Distance"/> of the <see cref="Stretch"/> using the provided <see cref="IDistanceCalculator"/>
        /// </summary>
        /// <param name="usingDistanceCalculator"></param>
        /// <returns><see cref="Geospatial.Distance"/></returns>
        /// <remarks>This calculation is perfomed each time it is called and the result is not cached.</remarks>
        public readonly Distance GetDistance(IDistanceCalculator usingDistanceCalculator) => usingDistanceCalculator.GetDistance(From, To);

        /// <summary>
        /// Detemines if a <see cref="Position"/> is within a maximum off track <see cref="Distance"/>.
        /// </summary>
        /// <param name="at"></param>
        /// <param name="maxOffTrackDistance"></param>
        /// <returns>True if the position <paramref name="at"/> is s within a maximum off track <paramref name="maxOffTrackDistance"/>.</returns>
        public bool IsOnTrack(Position at, Distance maxOffTrackDistance) => CrossTrackDistance(at) <= maxOffTrackDistance;

        /// <summary>
        /// Checks if a <see cref="Position"/> is anywhere between the strecth's two ends, regardless of offtrack distance.
        /// </summary>
        /// <param name="at"></param>
        /// <returns>True if the position <paramref name="at"/> is s between the <see cref="From">start</see> and <see cref="To">end</see> position.</returns>
        public readonly bool IsBetweenEnds(Position at) => at.IsBetween(From, To);

        /// <summary>
        /// Calculates the angle between the two directions starting from the position
        /// and ending in the start and end point of the stretch.
        /// </summary>
        /// <param name="at">The <see cref="Position">position</see> to get the angle for.</param>
        /// <returns>
        /// The angle between the two directions starting from the position
        /// and ending in the start and end point of the stretch.
        /// </returns>
        public readonly Angle MinAngle(Position at)
        {
            var θ1 = Between(at, From).Direction;
            var θ2 = Between(at, To).Direction;
            return θ1.Min(θ2);
        }

        /// <summary>
        /// The <see cref="Geospatial.Distance"/> of a point from a great-circle path (sometimes called cross track error). 
        /// </summary>
        /// <param name="at"></param>
        /// <returns>The <see cref="Geospatial.Distance"/> of a point from a great-circle path.</returns>
        public Distance CrossTrackDistance(Position at)
        {
            const double R = EarthMeanRadiusMeters;
            var s = Between(From, at);
            var δ = s.Distance.Meters;
            var θ1 = s.InitialBearing.Radians;
            var θ2 = InitialBearing.Radians;
            var d = Asin(Sin(δ / R) * Sin(θ1 - θ2)) * R;
            return Distance.FromMeters(Abs(d));
        }

        /// <summary>
        /// The along-track distance, calculated from the start point to the closest point on the path to the given point.
        /// </summary>
        /// <param name="at">The <see cref="Position">position</see> to caclulate the on track distance for.</param>
        /// <returns>The on track distance from the start of the stretch.</returns>
        public Distance OnTrackDistance(Position at)
        {
            const double r = EarthMeanRadiusMeters / 1000;
            var s = Between(From, at);
            var δ = s.Distance.Meters;
            var Δδ = CrossTrackDistance(at).Meters;
            var δt = Acos(Cos(δ / r) / Cos(Δδ / r)) * r;
            return Distance.FromMeters(Abs(δt));
        }
        private readonly Angle GetInitialBearing()
        {
            var (φ1, λ1) = From.RadianCoordinates;
            var (φ2, λ2) = To.RadianCoordinates;
            var Δλ = λ2 - λ1;
            var y = Sin(Δλ) * Cos(φ2);
            var x = (Cos(φ1) * Sin(φ2)) - (Sin(φ1) * Cos(φ2) * Cos(Δλ));
            var b = Atan2(y, x);
            return Angle.FromRadians((b + Π2) % Π2);
        }

        internal readonly Angle RhumbBearing()
        {
            var φ2 = To.Latitude.Radians;
            var φ1 = From.Latitude.Radians;
            var Δλ = To.Longitude.Radians - From.Longitude.Radians;
            var ΔΦ = Log(Tan((φ2 / 2) + (Π / 4)) / Tan((φ1 / 2) + (Π / 4)));
            var b = Atan2(Δλ, ΔΦ);
            return Angle.FromRadians((b + Π2) % Π2);
        }

        public override readonly string ToString() => $"From {From} to {To}";
        public readonly bool Equals(Stretch other) => other.From.Equals(From) && other.To.Equals(To);
        public override readonly bool Equals(object? obj) => obj is Stretch stretch && Equals(stretch);
        public override readonly int GetHashCode() => HashCode.Combine(From, To);

        public static bool operator==(Stretch a, Stretch b) => a.Equals(b);
        public static bool operator!=(Stretch a,  Stretch b) => !(a == b);
    }
}
