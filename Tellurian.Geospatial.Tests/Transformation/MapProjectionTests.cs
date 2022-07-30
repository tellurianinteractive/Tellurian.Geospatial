namespace Tellurian.Geospatial.Transformation.Tests;

[TestClass]
public class MapProjectionTests
{
    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = MapProjections.Sweref99TM;
        var actual = SerializationTester<MapProjection>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = MapProjections.Sweref99TM;
        var actual = SerializationTester<MapProjection>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

}
