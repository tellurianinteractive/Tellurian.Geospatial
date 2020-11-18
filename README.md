# Tellurian.Geospatial
Strongly typed object model for geospatial calculations and transformations.
All types are also serializable with the *DataContractSerializer* and *System.Text.Json*.
From release 2.0.0 this package only supports .NET 5+.

Released under MIT license 2020.
## Namespace *Tellurian.Geospatial*
Types for basic calculations of dictances etc. and building blocks for more advanced spatial algorithms.
* **Angle** representing 0 <= *degrees* < 360 and radians 0 <= *radians* < 2Π.
* **Distance** representing zero or positive distances in meters and kilometers.
* **Position** with **Latitude** and **Longitude** representing a two dimensional location on earth surface. The user must decide on which datum a position is expressed, for exampe WGS 84, ETRS 89 or similar. 
* **Speed** representing a zero or positive speed in m/s and km/h.
* **Stretch** between two **Position**s representing propertes for stretches on earth surface, ie. *distance*, *direction*, *on track distance* etc.

Each of these objects has value safe initializers and useful methods for calculating distances, angles etc. 

## Namespace *Tellurian.Geospatial.Surfaces* 
Types for modelling surfaces. Added from release 2.1.0.
* **CircularSurface** representing a *ReferencePosition* with a *Radius*.
* **PolygonalSurface** representing a *ReferencePosition* sourrounded by a polygonal border.

Surfaces has a *Includes(Position)* method that tells if a *Position* lies on or within the surface's border. 

## Namespace *Tellurian.Geospatial.Transform*
Types and methods for transforming between cartesian and planar coordinates.
* **EarthEllipsoid** represents the form of the earth. There are a few preconfigured earth ellipsiods: *Grs80* and *Wgs84*. You can easy create other.
* **MapProjection** represents how coordinates are mapped to the earth.  There are a few preconfigured: *Utm32-35*, and specific for Sweden: *Sweref99TM* and *Rt90*.
* **GridCoordinate** represents a planar coordinate.
* **GaussKrügerTransformer** uses the objects above to transform between cartesian and planar coordinates.

## Namespace *Tellurian.Geospatial.DistanceCalculators*
The method for calculating distances between **Position**s is pluggable because applications have different requirements regarding  precision and speed of calculation. 
The following **DistanceCalculator** is included:
* **HaversineDistanceCalculator** is a fast calculation that is suitable for distances down to decimeters, suitable for tracking of movable objects. 
This is also the default **DistanceCalculator**.

You can write additional distance calculators by implementing the *IDistanceCalculator* interface and use it to calculate distances of **Stretch**es.

## References
This implementation is inspired by part of *Latitude/longitude spherical geodesy tools*  
MIT Licence (c) Chris Veness 2002-2020  
https://www.movable-type.co.uk/scripts/latlong.html  
https://www.movable-type.co.uk/scripts/geodesy/docs/module-latlon-spherical.html
https://github.com/chrisveness/geodesy

Implementation of the **GaussKrügerTransformer** is based on formulas from Swedish Land Survey (Lantmäteriet)  
https://www.lantmateriet.se/sv/Kartor-och-geografisk-information/GPS-och-geodetisk-matning/Om-geodesi/Formelsamling/




