using System.Globalization;

namespace Tellurian.Geospatial.Tests;

[TestClass]
public class AngleTests
{
    [TestInitialize]
    public void TestInitialize() => CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void ZeroAngleIsValid()
    {
        var target = Angle.FromDegrees(0);
        Assert.AreEqual(0, target.Degrees);
    }

    [TestMethod]
    public void Angle359IsValid()
    {
        var target = Angle.FromDegrees(359.9999);
        Assert.AreEqual(359.9999, target.Degrees);
    }

    [TestMethod]
    public void NegativeAngleIsInvalid()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Angle.FromDegrees(-1));
    }

    [TestMethod]
    public void Angle360IsInvalid()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Angle.FromDegrees(360));
    }

    [TestMethod]
    public void Angle2PiIsInvalid()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => Angle.FromRadians(Math.PI * 2));
    }

    [TestMethod]
    public void FromZeroRadiansIsZeroDegrees()
    {
        var target = Angle.FromRadians(0);
        Assert.AreEqual(0, target.Radians);
        Assert.AreEqual(0, target.Degrees);
    }

    [TestMethod]
    public void FromPIRadiansIs180Degrees()
    {
        var target = Angle.FromRadians(Math.PI);
        Assert.AreEqual(Math.PI, target.Radians);
        Assert.AreEqual(180, target.Degrees);
    }

    [TestMethod]
    public void IsReverse()
    {
        Assert.AreEqual(45.0, Angle.FromDegrees(225).Reverse.Degrees);
        Assert.AreEqual(225.0, Angle.FromDegrees(45).Reverse.Degrees);
    }

    [TestMethod]
    public void IsComplement()
    {
        Assert.AreEqual(0.0, Angle.FromDegrees(0).Complement.Degrees);
        Assert.AreEqual(359.0, Angle.FromDegrees(1).Complement.Degrees);
    }

    [TestMethod]
    public void ToStringWorks()
    {
        var target = Angle.FromDegrees(1.5);
        Assert.AreEqual("1.50°", target.ToString());
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = Angle.FromDegrees(180);
        var actual = SerializationTester<Angle>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = Angle.FromDegrees(180);
        var actual = SerializationTester<Angle>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void IsAcute()
    {
        var target = Angle.FromDegrees(45);
        Assert.IsTrue(target.IsAcute);
        Assert.IsFalse(target.IsObtuse);
        Assert.IsFalse(target.IsRight);
        Assert.IsFalse(target.IsStraight);
    }

    [TestMethod]
    public void IsObtuse()
    {
        var target = Angle.FromDegrees(275);
        Assert.IsFalse(target.IsAcute);
        Assert.IsTrue(target.IsObtuse);
        Assert.IsFalse(target.IsRight);
        Assert.IsFalse(target.IsStraight);
    }

    [TestMethod]
    public void IsRight()
    {
        var target = Angle.FromDegrees(90);
        Assert.IsFalse(target.IsAcute);
        Assert.IsFalse(target.IsObtuse);
        Assert.IsTrue(target.IsRight);
        Assert.IsFalse(target.IsStraight);
    }

    [TestMethod]
    public void IsStraight()
    {
        var target = Angle.FromDegrees(180);
        Assert.IsFalse(target.IsAcute);
        Assert.IsFalse(target.IsObtuse);
        Assert.IsFalse(target.IsRight);
        Assert.IsTrue(target.IsStraight);
    }

    [TestMethod]
    public void Equals()
    {
        Assert.AreEqual(Angle.FromDegrees(10), Angle.FromDegrees(10));
        Assert.AreNotEqual(Angle.FromDegrees(10), new object());
        Assert.AreNotEqual(Angle.FromDegrees(9.99998), Angle.FromDegrees(10));
        Assert.AreEqual(Angle.FromDegrees(9.999999), Angle.FromDegrees(10));
    }

    [TestMethod]
    public void Operators()
    {
        Assert.IsTrue(Angle.FromDegrees(10) == Angle.FromDegrees(10));
        Assert.IsFalse(Angle.FromDegrees(10) == Angle.FromDegrees(11));
        Assert.IsTrue(Angle.FromDegrees(10) != Angle.FromDegrees(11));
        Assert.IsFalse(Angle.FromDegrees(10) != Angle.FromDegrees(10));
        Assert.AreEqual(Angle.FromDegrees(10), Angle.FromDegrees(355) - Angle.FromDegrees(345));
        Assert.AreEqual(Angle.FromDegrees(355), Angle.FromDegrees(10) + Angle.FromDegrees(345));
        Assert.AreEqual(Angle.FromDegrees(350), Angle.FromDegrees(10) - Angle.FromDegrees(20));
        Assert.AreEqual(Angle.FromDegrees(10), Angle.FromDegrees(350) + Angle.FromDegrees(20));
        Assert.IsTrue(Angle.FromDegrees(355) > Angle.FromDegrees(354));
        Assert.IsTrue(Angle.FromDegrees(355) >= Angle.FromDegrees(355));
        Assert.IsTrue(Angle.FromDegrees(354) < Angle.FromDegrees(355));
        Assert.IsTrue(Angle.FromDegrees(355) <= Angle.FromDegrees(355));
    }

    [TestMethod]
    public void Min()
    {
        Assert.AreEqual(Angle.FromDegrees(0), Angle.FromDegrees(0).Min(Angle.FromDegrees(0)));
        Assert.AreEqual(Angle.FromDegrees(10), Angle.FromDegrees(20).Min(Angle.FromDegrees(10)));
        Assert.AreEqual(Angle.FromDegrees(10), Angle.FromDegrees(20).Min(Angle.FromDegrees(30)));
        Assert.AreEqual(Angle.FromDegrees(180), Angle.FromDegrees(200).Min(Angle.FromDegrees(20)));
        Assert.AreEqual(Angle.FromDegrees(180), Angle.FromDegrees(20).Min(Angle.FromDegrees(200)));
    }
}
