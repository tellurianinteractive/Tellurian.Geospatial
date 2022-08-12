﻿using System.Globalization;

namespace Tellurian.Geospatial.Tests;

[TestClass]
public class LatitudeTests
{
    [TestInitialize]
    public void TestInitialize() => CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void Over90DegreesThrows()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Latitude.FromDegrees(90.1));
    }

    [TestMethod]
    public void UnderMinus90DegreesThrows()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Latitude.FromDegrees(-90.1));
    }

    [TestMethod]
    public void InitializationWithDegrees()
    {
        var target = Latitude.FromDegrees(45);
        Assert.AreEqual(45.0, target.Degrees);
        Assert.AreEqual(Math.PI / 4, target.Radians);
    }

    [TestMethod]
    public void InitializationWithRadians()
    {
        var target = Latitude.FromRadians(Math.PI / 4);
        Assert.AreEqual(45.0, target.Degrees);
        Assert.AreEqual(Math.PI / 4, target.Radians);
    }

    [TestMethod]
    public void Equals()
    {
        Assert.AreEqual(Latitude.FromDegrees(58), Latitude.FromDegrees(58));
        Assert.AreNotEqual(Latitude.FromDegrees(58), Latitude.FromDegrees(58.011));
        Assert.AreNotEqual(Latitude.FromDegrees(58), new object());
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = Latitude.FromDegrees(89.5);
        var actual = SerializationTester<Latitude>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = Latitude.FromDegrees(89.5);
        var actual = SerializationTester<Latitude>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void Operators()
    {
        Assert.IsTrue(Latitude.FromDegrees(1) == Latitude.FromDegrees(1));
        Assert.IsFalse(Latitude.FromDegrees(1) == Latitude.FromDegrees(2));
        Assert.IsTrue(Latitude.FromDegrees(1) != Latitude.FromDegrees(2));
        Assert.IsFalse(Latitude.FromDegrees(1) != Latitude.FromDegrees(1));
        Assert.IsTrue(Latitude.FromDegrees(1) < Latitude.FromDegrees(2));
        Assert.IsTrue(Latitude.FromDegrees(1) <= Latitude.FromDegrees(2));
        Assert.IsTrue(Latitude.FromDegrees(2) <= Latitude.FromDegrees(2));
        Assert.IsTrue(Latitude.FromDegrees(2) > Latitude.FromDegrees(1));
        Assert.IsTrue(Latitude.FromDegrees(2) >= Latitude.FromDegrees(1));
        Assert.IsTrue(Latitude.FromDegrees(2) >= Latitude.FromDegrees(2));
    }

    [TestMethod]
    public void ToStringWorks()
    {
        Assert.AreEqual("45.0ºN", Latitude.FromDegrees(45).ToString());
        Assert.AreEqual("45.0ºS", Latitude.FromDegrees(-45).ToString());
    }
}
