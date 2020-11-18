using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// Representation of the earths spherical form.
    /// </summary>
    /// <remarks>
    /// References: https://en.wikipedia.org/wiki/Earth_ellipsoid
    /// </remarks>
    [DataContract]
    public record EarthEllipsoid
    {
        public static EarthEllipsoid CreateFromSemiAxes(double semiMajorAxis, double semiMinorAxis) => 
            new EarthEllipsoid { SemiMajorAxis = semiMajorAxis, SemiMinorAxis = semiMinorAxis };

        public static EarthEllipsoid CreateFromSemiMajorAxisAndFlattening(double semiMajorAxis, double flattening) => 
            new EarthEllipsoid { SemiMajorAxis = semiMajorAxis, SemiMinorAxis = semiMajorAxis - (flattening * semiMajorAxis) };

        [DataMember(Name = "SemiMajorAxis")]
        public double SemiMajorAxis { get; init; }

        [DataMember(Name = "SemiMinorAxis")]
        public double SemiMinorAxis { get; init; }

        [JsonIgnore]
        public double Flattening => (SemiMajorAxis - SemiMinorAxis) / SemiMajorAxis;
    }
}
