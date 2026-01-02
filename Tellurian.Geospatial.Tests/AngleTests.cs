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
        Assert.Throws<ArgumentOutOfRangeException>(() => Angle.FromDegrees(-1));
    }

    [TestMethod]
    public void Angle360IsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Angle.FromDegrees(360));
    }

    [TestMethod]
    public void Angle2PiIsInvalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Angle.FromRadians(Math.PI * 2));
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
        Assert.AreNotEqual(Angle.FromDegrees(10-2*Angle.CompareTolerance), Angle.FromDegrees(10));
        Assert.AreEqual(Angle.FromDegrees(10-Angle.CompareTolerance), Angle.FromDegrees(10));
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

    #region IsBetween tests

    [TestMethod]
    public void IsBetween_NormalRange_AngleInside()
    {
        // 90° is between 60° and 120°
        Assert.IsTrue(Angle.FromDegrees(90).IsBetween(Angle.FromDegrees(60), Angle.FromDegrees(120)));
    }

    [TestMethod]
    public void IsBetween_NormalRange_AngleAtLowerBound()
    {
        // 60° is at the lower bound of [60°, 120°]
        Assert.IsTrue(Angle.FromDegrees(60).IsBetween(Angle.FromDegrees(60), Angle.FromDegrees(120)));
    }

    [TestMethod]
    public void IsBetween_NormalRange_AngleAtUpperBound()
    {
        // 120° is at the upper bound of [60°, 120°]
        Assert.IsTrue(Angle.FromDegrees(120).IsBetween(Angle.FromDegrees(60), Angle.FromDegrees(120)));
    }

    [TestMethod]
    public void IsBetween_NormalRange_AngleOutsideBelow()
    {
        // 50° is below the range [60°, 120°]
        Assert.IsFalse(Angle.FromDegrees(50).IsBetween(Angle.FromDegrees(60), Angle.FromDegrees(120)));
    }

    [TestMethod]
    public void IsBetween_NormalRange_AngleOutsideAbove()
    {
        // 130° is above the range [60°, 120°]
        Assert.IsFalse(Angle.FromDegrees(130).IsBetween(Angle.FromDegrees(60), Angle.FromDegrees(120)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleInsideAboveLower()
    {
        // 350° is in the range [315°, 45°] (wrap-around)
        Assert.IsTrue(Angle.FromDegrees(350).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleInsideBelowUpper()
    {
        // 30° is in the range [315°, 45°] (wrap-around)
        Assert.IsTrue(Angle.FromDegrees(30).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleAtZero()
    {
        // 0° is in the range [315°, 45°] (wrap-around)
        Assert.IsTrue(Angle.FromDegrees(0).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleAtLowerBound()
    {
        // 315° is at the lower bound of [315°, 45°]
        Assert.IsTrue(Angle.FromDegrees(315).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleAtUpperBound()
    {
        // 45° is at the upper bound of [315°, 45°]
        Assert.IsTrue(Angle.FromDegrees(45).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleOutside()
    {
        // 180° is outside the range [315°, 45°]
        Assert.IsFalse(Angle.FromDegrees(180).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleJustOutsideAboveUpper()
    {
        // 50° is just outside the range [315°, 45°]
        Assert.IsFalse(Angle.FromDegrees(50).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_WrapAroundRange_AngleJustOutsideBelowLower()
    {
        // 310° is just outside the range [315°, 45°]
        Assert.IsFalse(Angle.FromDegrees(310).IsBetween(Angle.FromDegrees(315), Angle.FromDegrees(45)));
    }

    [TestMethod]
    public void IsBetween_SameAngles_AngleMatches()
    {
        // 90° is between [90°, 90°] (single point)
        Assert.IsTrue(Angle.FromDegrees(90).IsBetween(Angle.FromDegrees(90), Angle.FromDegrees(90)));
    }

    [TestMethod]
    public void IsBetween_SameAngles_AngleDoesNotMatch()
    {
        // 91° is not between [90°, 90°]
        Assert.IsFalse(Angle.FromDegrees(91).IsBetween(Angle.FromDegrees(90), Angle.FromDegrees(90)));
    }

    [TestMethod]
    public void IsBetween_UndefinedAngle_ReturnsFalse()
    {
        Assert.IsFalse(Angle.Undefined.IsBetween(Angle.FromDegrees(0), Angle.FromDegrees(90)));
    }

    [TestMethod]
    public void IsBetween_UndefinedLowerBound_ReturnsFalse()
    {
        Assert.IsFalse(Angle.FromDegrees(45).IsBetween(Angle.Undefined, Angle.FromDegrees(90)));
    }

    [TestMethod]
    public void IsBetween_UndefinedUpperBound_ReturnsFalse()
    {
        Assert.IsFalse(Angle.FromDegrees(45).IsBetween(Angle.FromDegrees(0), Angle.Undefined));
    }

    #endregion
}
