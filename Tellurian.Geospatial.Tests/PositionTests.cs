using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tellurian.Geospatial.Tests
{
    [TestClass]
    public class PositionTests
    {
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
            Assert.AreNotEqual(Position.FromDegrees(10, -20), Position.FromDegrees(10, -20.001));
            Assert.AreNotEqual(Position.FromDegrees(10, -20), new object());
        }

        [TestMethod]
        public void Operators()
        {
            Assert.IsTrue(Position.FromDegrees(45, 45) == Position.FromRadians(Math.PI / 4, Math.PI/4));
            Assert.IsFalse(Position.FromDegrees(45, 45.1) == Position.FromRadians(Math.PI / 4, Math.PI / 4));
            Assert.IsFalse(Position.FromDegrees(45, 45) != Position.FromRadians(Math.PI / 4, Math.PI / 4));
            Assert.IsTrue(Position.FromDegrees(45, 45.1) != Position.FromRadians(Math.PI / 4, Math.PI / 4));
            Assert.AreEqual(Position.FromDegrees(44, 46), Position.FromDegrees(45, 45) - Position.FromDegrees(1, -1));
        }

        [TestMethod]
        public void SerializationAndDeserializationWorks()
        {
            var target = Position.FromDegrees(45, 135);
            var actual = SerializationTester<Position>.SerializeAndDeserialize(target);
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
    }
}
