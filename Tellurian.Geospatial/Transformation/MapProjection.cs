using System.Runtime.Serialization;

namespace Tellurian.Geospatial.Transformation;

[DataContract]
public record MapProjection
{
    [DataMember(Name = "Ellipsoid")]
    public EarthEllipsoid Ellipsoid { get; init; }
    [DataMember(Name = "CentralMeridian")]
    public double CentralMeridian { get; init; }
    [DataMember(Name = "Scale")]
    public double Scale { get; init; }
    [DataMember(Name = "FalseNorthing")]
    public double FalseNorthing { get; init; }
    [DataMember(Name = "FalseEasting")]
    public double FalseEasting { get; init; }
}
