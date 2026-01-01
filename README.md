# Tellurian.Geospatial

[![NuGet](https://img.shields.io/nuget/v/Tellurian.Geospatial.svg)](https://www.nuget.org/packages/Tellurian.Geospatial/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

A strongly-typed, high-performance .NET library for geospatial calculations.
**Zero heap allocations** - all types are value types optimized for tracking and real-time applications.

## Features
- Calculate distances and bearings between coordinates
- Track positions along routes with cross-track and on-track distance
- Transform between geodetic (lat/lon) and planar grid coordinates (UTM, SWEREF99, RT90)
- Define geofences with circular and polygonal surfaces
- Full serialization support (System.Text.Json and DataContractSerializer)

## Installation

```
dotnet add package Tellurian.Geospatial
```

## Quick Start

### Calculate distance and bearing between two positions
```csharp
var stockholm = Position.FromDegrees(59.3262, 17.8420);
var london = Position.FromDegrees(51.5074, -0.1278);

var stretch = Stretch.Between(stockholm, london);

Console.WriteLine($"Distance: {stretch.Distance.Kilometers:F0} km");      // 1435 km
Console.WriteLine($"Direction: {stretch.Direction.Degrees:F1}°");         // 236.5°
Console.WriteLine($"Initial bearing: {stretch.InitialBearing.Degrees:F1}°"); // 227.5°
```

### Find destination from a starting point
```csharp
var start = Position.FromDegrees(58.0338, 11.7450);
var bearing = Angle.FromDegrees(97);
var distance = Distance.FromMeters(562);

var destination = start.Destination(bearing, distance);
// Position: 58.0332, 11.7545
```

### Check if a position is within a geofence
```csharp
var center = Position.FromDegrees(58.0724, 11.8233);
var geofence = new CircularSurface(center, Distance.FromMeters(100));

var vehicle = Position.FromDegrees(58.0725, 11.8235);
if (geofence.Includes(vehicle))
{
    Console.WriteLine("Vehicle is inside the geofence");
}
```

### Track position along a route
```csharp
var routeStart = Position.FromDegrees(58.0338, 11.7450);
var routeEnd = Position.FromDegrees(58.0332, 11.7545);
var route = Stretch.Between(routeStart, routeEnd);

var currentPosition = Position.FromDegrees(58.0333, 11.7502);

var crossTrack = route.CrossTrackDistance(currentPosition);  // Distance from route
var onTrack = route.OnTrackDistance(currentPosition);        // Progress along route

Console.WriteLine($"Off track: {crossTrack.Meters:F1} m");   // 15.9 m
Console.WriteLine($"Progress: {onTrack.Meters:F0} m");       // 311 m
```

### Transform between coordinate systems
```csharp
// Convert GPS coordinates to Swedish grid (SWEREF99 TM)
var gpsPosition = Position.FromDegrees(59.3262, 17.8420);
var gridCoord = GaussKreugerTransformer.ToGridCoordinate(gpsPosition, MapProjections.Sweref99TM);

Console.WriteLine($"N: {gridCoord.X}, E: {gridCoord.Y}");

// Convert back to GPS coordinates
var backToGps = GaussKreugerTransformer.ToPosition(gridCoord, MapProjections.Sweref99TM);
```

## Core Types

| Type | Description |
|------|-------------|
| `Position` | Latitude/Longitude coordinate |
| `Stretch` | Segment between two positions with distance, direction, and bearing calculations |
| `Distance` | Value in meters/kilometers with comparison tolerance |
| `Angle` | Value in degrees/radians (0-360°) |
| `Speed` | Value in m/s or km/h |
| `Vector` | Distance with direction |

## Surfaces (Geofencing)

| Type | Description |
|------|-------------|
| `CircularSurface` | Circle defined by center position and radius |
| `PolygonalSurface` | Polygon defined by border positions |

Both implement `Includes(Position)` to check if a point is inside the boundary.

## Coordinate Transformation

**Map Projections:** UTM zones 32-35, Sweref99TM, RT90

**Ellipsoids:** WGS84, GRS80, Hayford1910

## Pluggable Distance Calculation

The default `HaversineDistanceCalculator` is optimized for tracking applications (accurate to decimeters).
Implement `IDistanceCalculator` for custom algorithms.

## Requirements

- .NET 9.0 or .NET 10.0

## References

Inspired by [Latitude/longitude spherical geodesy tools](https://www.movable-type.co.uk/scripts/latlong.html) by Chris Veness.

Coordinate transformation based on formulas from [Swedish Land Survey (Lantmäteriet)](https://www.lantmateriet.se/sv/geodata/gps-geodesi-och-swepos/Om-geodesi/Formelsamling/).

## License

MIT License - see [LICENSE](LICENSE) for details.

---

## Release Notes

### 2.7.0
- .NET 10 support (while still supporting .NET 9)

### Breaking Changes (2.5.0+)
- `ToString()` output changed for Angle, Latitude, Longitude, Position, Vector, and Stretch
- Public constructors removed - use static factory methods (e.g., `Position.FromDegrees()`)
- Equality comparisons now respect comparison tolerance
