using System.Globalization;

namespace Tellurian.Geospatial.Tests;

[TestClass]
public class SpeedTests
{
    [TestInitialize]
    public void TestInitialize() => CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void NegativeSpeedThrows()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Speed.FromMetersPerSecond(-1));
    }

    [TestMethod]
    public void Equals()
    {
        Assert.AreEqual(Speed.FromMetersPerSecond(10), (Speed.FromMetersPerSecond(10)));
        Assert.AreNotEqual(Speed.FromMetersPerSecond(10), (Speed.FromMetersPerSecond(10.02)));
        Assert.AreNotEqual(Speed.FromMetersPerSecond(10), (new object()));
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = Speed.FromKilometersPerHour(89.5);
        var actual = SerializationTester<Speed>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = Speed.FromKilometersPerHour(89.5);
        var actual = SerializationTester<Speed>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void Operators()
    {
        Assert.IsTrue(Speed.FromKilometersPerHour(36) == Speed.FromMetersPerSecond(10), "Equals 1");
        Assert.IsFalse(Speed.FromKilometersPerHour(36) == Speed.FromMetersPerSecond(11), "Equals 2");
        Assert.IsFalse(Speed.FromKilometersPerHour(36) != Speed.FromMetersPerSecond(10), "Not Equals 1");
        Assert.IsTrue(Speed.FromKilometersPerHour(36) != Speed.FromMetersPerSecond(11), "Not Equals 2");
        Assert.IsTrue(Speed.FromKilometersPerHour(36) < Speed.FromMetersPerSecond(11));
        Assert.IsTrue(Speed.FromKilometersPerHour(36) <= Speed.FromMetersPerSecond(11));
        Assert.IsTrue(Speed.FromKilometersPerHour(36) <= Speed.FromMetersPerSecond(10));
        Assert.IsTrue(Speed.FromKilometersPerHour(36) > Speed.FromMetersPerSecond(9));
        Assert.IsTrue(Speed.FromKilometersPerHour(36) >= Speed.FromMetersPerSecond(9));
        Assert.IsTrue(Speed.FromKilometersPerHour(36) <= Speed.FromMetersPerSecond(10));
    }

    [TestMethod]
    public void FromKilometersPerHourWorks()
    {
        var target = Speed.FromKilometersPerHour(72);
        Assert.AreEqual(20.0, target.MetersPerSecond);
        Assert.AreEqual(72.0, target.KilometersPerHour);
    }

    [TestMethod]
    public void IsBelow()
    {
        Assert.IsTrue(Speed.Zero < 1);
        Assert.IsFalse(Speed.FromMetersPerSecond(1) < 1);
    }

    [TestMethod]
    public void ToStringMeterPerSecond()
    {
        Assert.AreEqual("10.5m/s", Speed.FromMetersPerSecond(10.5).ToString());
    }

    [TestMethod]
    public void IsZero()
    {
        Assert.AreEqual(0.0, Speed.Zero.MetersPerSecond);
        Assert.IsTrue(Speed.Zero.IsZero);
        Assert.IsFalse(Speed.FromMetersPerSecond(0.01).IsZero);
    }

    [TestMethod]
    public void CompareToLarger()
    {
        var s1 = Speed.FromMetersPerSecond(1);
        var s2 = Speed.FromMetersPerSecond(2);
        var result = s1.CompareTo(s2);
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public void CompareToSmaller()
    {
        var s1 = Speed.FromMetersPerSecond(1);
        var s2 = Speed.FromMetersPerSecond(2);
        var result = s2.CompareTo(s1);
        Assert.AreEqual(1, result);
    }


}
