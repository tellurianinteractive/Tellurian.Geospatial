namespace Tellurian.Geospatial.Tests;

[TestClass]
public class DistanceTests
{
    [TestMethod]
    public void DefaultDistanceIsZero()
    {
        var target = new Distance();
        Assert.IsTrue(target.IsZero);
    }

    [TestMethod]
    public void NegativeDistanceThrows()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Distance.FromMeters(-1));
    }

    [TestMethod]
    public void PositiveDistanceWorks()
    {
        var target = Distance.FromMeters(1);
        Assert.AreEqual(1, target.Meters);
    }

    [TestMethod]
    public void EquatableEqualsWorks()
    {
        var one = Distance.FromMeters(1);
        var another = Distance.FromMeters(1);
        Assert.IsTrue(one.Equals(another));
    }

    [TestMethod]
    public void ObjectEqualsWorks()
    {
        var one = Distance.FromMeters(1);
        var another = Distance.FromMeters(1);
        Assert.AreEqual(one, another);
    }

    [TestMethod]
    public void EqualsWorks()
    {
        Assert.IsTrue(Distance.FromMeters(1).Equals(Distance.FromMeters(1)));
        Assert.IsFalse(Distance.FromMeters(1).Equals(Distance.FromMeters(0.99)));
        Assert.IsFalse(Distance.FromMeters(1).Equals(new object()));
    }

    [TestMethod]
    public void NotEqualsWorks()
    {
        var one = Distance.FromMeters(1);
        var another = Distance.FromMeters(2);
        Assert.IsTrue(one != another);
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = Distance.FromMeters(1.5);
        var actual = SerializationTester<Distance>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = Distance.FromMeters(1.5);
        var actual = SerializationTester<Distance>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void ToStringWorks()
    {
        var target = Distance.FromMeters(1);
        Assert.AreEqual("1m", target.ToString());
    }

    [TestMethod]
    public void Zero()
    {
        Assert.AreEqual(Distance.Zero.Meters, 0.0);
    }

    [TestMethod]
    public void Operators()
    {
        Assert.IsTrue(Distance.FromMeters(10) == Distance.FromMeters(10));
        Assert.IsFalse(Distance.FromMeters(10) != Distance.FromMeters(10));
        Assert.IsFalse(Distance.FromMeters(10) == Distance.FromMeters(11));
        Assert.IsTrue(Distance.FromMeters(10) != Distance.FromMeters(11));
        Assert.IsTrue(Distance.FromMeters(10) < Distance.FromMeters(11));
        Assert.IsTrue(Distance.FromMeters(10) <= Distance.FromMeters(11));
        Assert.IsTrue(Distance.FromMeters(11) > Distance.FromMeters(10));
        Assert.IsTrue(Distance.FromMeters(11) >= Distance.FromMeters(10));
    }

}
