# Tellurian.Geospatial
Strongly typed object model for geospatial calculations and transformations.
All types are also serializable with the *DataContractSerializer* and *System.Text.Json*.

Released under MIT license 2022-2024.

### Release  2.6.0
- **Stretch** changed from *record* to *struct*. This means that the package now is heap-allocation free.
- **NET 9 support** while still supporting .NET 8.
### Breaking changes
From release 2.5.0: 
- **ToString()** changed for *angle*, *latitude*, *longitude*, *position*, *vector*, and *stretch*.
See the source test project for expected output strings.
- **Public constructors** removed for *angle*, *distance*, *latitude*, *longitude*, *position*, and *speed*, 
as they was only intended for deserialization and this works without them. Use static factory methods instead.
- **Equality and comparisions** for *angle*, *distance*, and *speed* now correctly considerer *compare tolerance*.
- **Obsolete methods** are marked for future removal. Consider make the changes suggested.

#### From release 2.4.0: 
- Changed serialization property names for some types, which may cause serialization to break 
when older version of this package is used in the 'other' end of the wire.


## Overview of functionality
### Namespace *Tellurian.Geospatial*
Types for basic calculations of dictances etc. and building blocks for more advanced spatial algorithms.
* **Angle** representing 0 <= *degrees* < 360 and radians 0 <= *radians* < 2Π.
* **Distance** representing zero or positive distances in meters and kilometers.
* **Position** with **Latitude** and **Longitude** representing a two dimensional location on earth surface. The user must decide on which datum a position is expressed, for exampe WGS 84, ETRS 89 or similar. 
* **Speed** representing a zero or positive speed in m/s and km/h.
* **Stretch** between two **Position**s representing propertes for stretches on earth surface, ie. *distance*, *direction*, *on track distance* etc.
* **Vector** represening a **Distance** with an **Angle**.

Each of these objects has value safe initializers and useful methods for calculating distances, angles etc. 

### Namespace *Tellurian.Geospatial.Surfaces* 
Types for modelling surfaces. Added from release 2.1.0.
* **CircularSurface** representing a *ReferencePosition* with a *Radius*.
* **PolygonalSurface** representing a *ReferencePosition* sourrounded by a polygonal border.

Surfaces has a *Includes(Position)* method that tells if a *Position* lies on or within the surface's border. 
You can create your own by deriving from **Surface** base class.

### Namespace *Tellurian.Geospatial.Transform*
Types and methods for transforming between cartesian and planar coordinates.
* **EarthEllipsoid** represents the form of the earth. There are a few preconfigured, see below. You can easy create other.
* **MapProjection** represents how coordinates are mapped to the earth.  There are a few preconfigured, see below. You can easy create other.
* **GridCoordinate** represents a planar coordinate.
* **GaussKrügerTransformer** uses the objects above to transform between cartesian and planar coordinates.

There are built-in *map projections* for planar coordinates:
* **MapProjections.UTMxx**, where 'xx' is any of zone 32-35 used in Europe.
* **MapProjections.Sweref99TM** (same as UTM33 but extended to cover Sweden)
* **MapProjections.RT90** (older system used in Sweden).

There are built-in *ellipsoids*:
* **Ellipsoids.WGS84** used for most GPS-devices. **SWEREF99** differs < 1 meter from WGS84.
* **Ellipsoids.GRS80** used for many transformations to/from planar coordinates.
* **Ellipsoids.Hayford1910** 

### Namespace *Tellurian.Geospatial.DistanceCalculators*
The method for calculating distances between **Position**s is pluggable because applications have different requirements regarding  precision and speed of calculation. 
The following **DistanceCalculator** is included:
* **HaversineDistanceCalculator** is a fast calculation that is suitable for distances down to decimeters, suitable for tracking of movable objects. 
This is also the default **DistanceCalculator**.

You can write additional distance calculators by implementing the *IDistanceCalculator* interface and use it to calculate distances of **Stretch**es.

## Benchmarks
A **Tellurian.Geospatial.Benchmarks** project is available in the source code repository as part of
the solution for *Tellurian.Geospatial*.
This project can be used to analyse performance issues with this package.
If you add benchmarks, please also make a pull request to provide these to all of us.

## References
This implementation is inspired by part of *Latitude/longitude spherical geodesy tools*  
MIT Licence (c) Chris Veness 2002-2022 
https://www.movable-type.co.uk/scripts/latlong.html  
https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html
https://github.com/chrisveness/geodesy

Implementation of the **GaussKrügerTransformer** is based on formulas from Swedish Land Survey (Lantmäteriet)  
https://www.lantmateriet.se/sv/Kartor-och-geografisk-information/GPS-och-geodetisk-matning/Om-geodesi/Formelsamling/




