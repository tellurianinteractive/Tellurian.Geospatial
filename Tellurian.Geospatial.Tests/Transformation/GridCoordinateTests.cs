namespace Tellurian.Geospatial.Transformation.Tests;

[TestClass]
public class GridCoordinateTests
{
    const double Northing = 36346.345;
    const double Easting = 12324.43;

    [TestMethod]
    public void ConstructorWorks()
    {
        var target = new GridCoordinate(Northing, Easting);
        Assert.AreEqual(Northing, target.Northing);
        Assert.AreEqual(Easting, target.Easting);
    }

    [TestMethod]
    public void EqualsWorks()
    {
        var one = new GridCoordinate(12345.67, 12345.78);
        var another = new GridCoordinate(12345.67, 12345.78);
        Assert.AreEqual(one, another);
        Assert.IsTrue(one == another);
        Assert.IsFalse(one != another);
        Assert.AreNotEqual(one, new object());
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = new GridCoordinate(123.4, 987.6);
        var actual = SerializationTester<GridCoordinate>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = new GridCoordinate(123.4, 987.6);
        var actual = SerializationTester<GridCoordinate>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

}
