using System.Globalization;

namespace Tellurian.Geospatial.Tests;

[TestClass]
public class VectorTests
{
    [TestInitialize]
    public void TestInitialize() => CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = TestVector;
        var actual = SerializationTester<Vector>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = TestVector;
        var actual = SerializationTester<Vector>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    private static Vector TestVector =>
        Vector.From (Angle.FromDegrees(45), Distance.FromMeters(10));
}
