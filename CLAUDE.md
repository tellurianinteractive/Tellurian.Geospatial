# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build entire solution
dotnet build Tellurian.Geospatial.slnx

# Build main library only
dotnet build Tellurian.Geospatial/Tellurian.Geospatial.csproj

# Run all tests
dotnet test Tellurian.Geospatial.Tests/Tellurian.Geospatial.Tests.csproj

# Run a specific test
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run benchmarks (requires Release mode)
dotnet run -c Release --project Tellurian.Geospatial.Benchmarks/Tellurian.Geospatial.Benchmarks.csproj
```

## Project Structure

- **Tellurian.Geospatial** - Main library (targets .NET 9/10, generates NuGet package)
- **Tellurian.Geospatial.Tests** - MSTest unit tests
- **Tellurian.Geospatial.Benchmarks** - BenchmarkDotNet performance tests

## Architecture

### Core Types (Tellurian.Geospatial namespace)
All core types are **readonly structs** with value semantics, designed to be heap-allocation free:

- **Position** - Latitude/Longitude coordinate. Use factory methods `FromDegrees()` or `FromRadians()`
- **Stretch** - Segment between two positions with distance/bearing calculations, cached results
- **Angle**, **Distance**, **Speed** - Value types with unit conversions (degrees/radians, meters/km, m/s/km/h)
- **Latitude**, **Longitude** - Constrained angle types for coordinates
- **Vector** - Distance with direction

### Distance Calculation (DistanceCalculators namespace)
Pluggable via `IDistanceCalculator` interface. Default is `HaversineDistanceCalculator` (suitable for tracking applications).

### Surfaces (Surfaces namespace)
Abstract `Surface` base class with `Includes(Position)` method:
- **CircularSurface** - Position with radius
- **PolygonalSurface** - Position with polygon border

### Coordinate Transformation (Transformation namespace)
- **GaussKreugerTransformer** - Converts between geodetic Position and planar GridCoordinate
- **MapProjection** - Preconfigured: UTM zones 32-35, Sweref99TM, RT90
- **EarthEllipsoid** - Preconfigured: WGS84, GRS80, Hayford1910

## Code Conventions

- File-scoped namespaces (`csharp_style_namespace_declarations = file_scoped`)
- Use static factory methods instead of public constructors for value types
- All types support `DataContractSerializer` and `System.Text.Json` serialization
- Greek letters used in formulas (φ for latitude, λ for longitude, etc.) following geodesy conventions
