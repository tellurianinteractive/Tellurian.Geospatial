using System.Globalization;

namespace Tellurian.Geospatial.Tests;

[TestClass]
public class PositionTests
{
    [TestInitialize]
    public void TestInitialize() => CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

    [TestMethod]
    public void IsInvalid()
    {
        Assert.IsTrue(Position.FromDegrees(0, 0).IsOrigo);
        Assert.IsFalse(Position.FromDegrees(1, 0).IsOrigo);
        Assert.IsFalse(Position.FromDegrees(0, 1).IsOrigo);
    }

    [TestMethod]
    public void Equals()
    {
        Assert.AreEqual(Position.FromDegrees(10, -20), Position.FromDegrees(10, -20));
        Assert.AreNotEqual(Position.FromDegrees(10, -20), Position.FromDegrees(10, -20.011));
        Assert.AreNotEqual(Position.FromDegrees(10, -20), new object());
    }

    [TestMethod]
    public void Operators()
    {
        Assert.IsTrue(Position.FromDegrees(45, 45) == Position.FromRadians(Math.PI / 4, Math.PI / 4));
        Assert.IsFalse(Position.FromDegrees(45, 45.1) == Position.FromRadians(Math.PI / 4, Math.PI / 4));
        Assert.IsFalse(Position.FromDegrees(45, 45) != Position.FromRadians(Math.PI / 4, Math.PI / 4));
        Assert.IsTrue(Position.FromDegrees(45, 45.1) != Position.FromRadians(Math.PI / 4, Math.PI / 4));
    }

    [TestMethod]
    public void XmlSerializationAndDeserializationWorks()
    {
        var target = Position.FromDegrees(45, 135);
        var actual = SerializationTester<Position>.XmlSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void JsonSerializationAndDeserializationWorks()
    {
        var target = Position.FromDegrees(45, 135);
        var actual = SerializationTester<Position>.JsonSerializeAndDeserialize(target);
        Assert.AreEqual(target, actual);
    }

    [TestMethod]
    public void ToStringWorks()
    {
        var target = Position.FromDegrees(45.1, 135.123456);
        Assert.AreEqual("45.1,135.123456", target.ToString());
    }

    [TestMethod]
    public void ShortDistanceIsCorrect()
    {
        var actual = TestData.Hövik.Destination(Angle.FromDegrees(97.133), Distance.FromMeters(562));
        Assert.AreEqual(TestData.Höviksnäs, actual);
    }

    [TestMethod]
    public void LongDistanceIsCorrect()
    {
        var actual = TestData.Stockholm.Destination(Angle.FromDegrees(48.852658287660404), Distance.FromKilometers(17444.959542176839));
        Assert.AreEqual(TestData.Wellington, actual);
    }

    [TestMethod]
    public void Origo()
    {
        Assert.AreEqual(Position.FromDegrees(0, 0), Position.Origo);
    }

    #region IsBetween tests

    [TestMethod]
    public void PositionIsBetween()
    {
        var left = Position.FromDegrees(50, 10);
        var right = left.Destination(Angle.Zero, Distance.FromMeters(1000));
        var target = left.Destination(Angle.FromDegrees(45), Distance.FromMeters(1000));
        Assert.IsTrue(target.IsBetween(left, right));
    }

    [TestMethod]
    public void LeftPositionIsNotBetween()
    {
        var left = Position.FromDegrees(50, 10);
        var right = left.Destination(Angle.Zero, Distance.FromMeters(1000));
        Assert.IsFalse(left.IsBetween(left, right));
    }

    [TestMethod]
    public void RightPositionIsNotBetween()
    {
        var left = Position.FromDegrees(50, 10);
        var right = left.Destination(Angle.Zero, Distance.FromMeters(1000));
        Assert.IsFalse(right.IsBetween(left, right));
    }

    [TestMethod]
    public void PositionIsNotBetween()
    {
        var left = Position.FromDegrees(50, 10);
        var right = left.Destination(Angle.Zero, Distance.FromMeters(1000));
        var target = left.Destination(Angle.FromDegrees(45).Reverse, Distance.FromMeters(1000));
        Assert.IsFalse(target.IsBetween(left, right));
    }

    [TestMethod]
    public void LeftPositionInRightAngleIsNotBetween()
    {
        var left = Position.FromDegrees(50, 10);
        var right = left.Destination(Angle.Zero, Distance.FromMeters(1000));
        var target = left.Destination(Angle.Right, Distance.FromMeters(1000));
        Assert.IsFalse(target.IsBetween(left, right));
    }

    [TestMethod]
    public void RightPositionInRightAngleIsNotBetween()
    {
        var right = Position.FromDegrees(50, 10);
        var left = right.Destination(Angle.Zero.Reverse, Distance.FromMeters(1000));
        var target = right.Destination(Angle.Right-2*Angle.CompareTolerance, Distance.FromMeters(1));
        Assert.IsFalse(target.IsBetween(left, right));
    }

    #endregion
}
