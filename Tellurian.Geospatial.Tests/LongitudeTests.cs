using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tellurian.Geospatial.Tests
{
    [TestClass]
    public class LongitudeTests
    {
        [TestMethod]
        public void Over90DegreesThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Longitude.FromDegrees(180.1));
        }

        [TestMethod]
        public void UnderMinus90DegreesThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Longitude.FromDegrees(-180.1));
        }

        [TestMethod]
        public void InitializationWithDegrees()
        {
            var target = Longitude.FromDegrees(45);
            Assert.AreEqual(45.0, target.Degrees);
            Assert.AreEqual(Math.PI / 4, target.Radians);
        }

        [TestMethod]
        public void InitializationWithRadians()
        {
            var target = Longitude.FromRadians(Math.PI / 4);
            Assert.AreEqual(45.0, target.Degrees);
            Assert.AreEqual(Math.PI / 4, target.Radians);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.AreEqual(Longitude.FromDegrees(58), Longitude.FromDegrees(58));
            Assert.AreNotEqual(Longitude.FromDegrees(58), Longitude.FromDegrees(58.011));
            Assert.AreNotEqual(Longitude.FromDegrees(58), new object());
        }

        [TestMethod]
        public void XmlSerializationAndDeserializationWorks()
        {
            var target = Longitude.FromDegrees(89.5);
            var actual = SerializationTester<Longitude>.XmlSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void JsonSerializationAndDeserializationWorks()
        {
            var target = Longitude.FromDegrees(89.5);
            var actual = SerializationTester<Longitude>.JsonSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void Operators()
        {
            Assert.IsTrue(Longitude.FromDegrees(1) == Longitude.FromDegrees(1));
            Assert.IsFalse(Longitude.FromDegrees(1) == Longitude.FromDegrees(2));
            Assert.IsTrue(Longitude.FromDegrees(1) != Longitude.FromDegrees(2));
            Assert.IsFalse(Longitude.FromDegrees(1) != Longitude.FromDegrees(1));
            Assert.IsTrue(Longitude.FromDegrees(1) < Longitude.FromDegrees(2));
            Assert.IsTrue(Longitude.FromDegrees(1) <= Longitude.FromDegrees(2));
            Assert.IsTrue(Longitude.FromDegrees(2) <= Longitude.FromDegrees(2));
            Assert.IsTrue(Longitude.FromDegrees(2) > Longitude.FromDegrees(1));
            Assert.IsTrue(Longitude.FromDegrees(2) >= Longitude.FromDegrees(1));
            Assert.IsTrue(Longitude.FromDegrees(2) >= Longitude.FromDegrees(2));
        }

        [TestMethod]
        public void ToStringWorks()
        {
            Assert.AreEqual("45,0", Longitude.FromDegrees(45).ToString());
        }
    }
}
