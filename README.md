# Tellurian.Geospatial
## Namespace *Tellurian.Geospatial*
Strongly typed object model for geospatial calculations and transformations.
* **Angle** represening 0 <= *degrees* < 360 and radians 0 <= *radians* < 2Π.
* **Distance** reprenting zero or positive distances in meters and kilometers.
* **Position** with **Latitude** and **Longitude** representing a two dimensional location on earth surface. The user must decide on which datum a position is expressed, for exampe WGS 84, ETRS 98 or other. 
* **Speed** representing a zero or positive speed in m/s and km/h.
* **Stretch** between two **Position**s representing propertes for stretches on earth surface, ie. *distance*, *direction*, *on track distance* etc.
Each of these objects has value safe initializers and useful methods for calculating distances, angles etc. 

## Namespace *Tellurian.Geospatial.Transform*
Methods for transforming between cartesian and planar coordinates.
* **EarthEllipsoid** represents the form of the earth. There are a few preconfigured earth ellipsiods: Grs80 and Wgs84. You can easy create other.
* **MapProjection** represents how coordinates are mapped to the earth.  There are a few preconfigured: Utm32-35, and specifi for Sweden: Sweref99TM and Rt90
* **GridCoordinate** represents a planar coordinate.
* **GaussKrügerTransformer** uses the objects above to transform between cartesian and planar coordinates.

## Namespace *Tellurian.Geospatial.DistanceCalculators*
The method for calculating distances between **Position**s is pluggable because applications have different requirements regarding for example precision and speed. This **DistanceCalculator** is included:
* **HaversineDistanceCalculator**
