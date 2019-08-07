using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tellurian.Geospatial.Tests
{
    [TestClass]
    public class SpeedTests
    {
        [TestMethod]
        public void NegativeSpeedThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>  Speed.FromMetersPerSecond(-1));
        }

        [TestMethod]
        public void Equals()
        {
            Assert.AreEqual(Speed.FromMetersPerSecond(10),(Speed.FromMetersPerSecond(10)));
            Assert.AreNotEqual(Speed.FromMetersPerSecond(10),(Speed.FromMetersPerSecond(10.02)));
            Assert.AreNotEqual(Speed.FromMetersPerSecond(10),(new object()));
        }

        [TestMethod]
        public void SerializationAndDeserializationWorks()
        {
            var target = Speed.FromKilometersPerHour(89.5);
            var actual = SerializationTester<Speed>.SerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void Operators()
        {
            Assert.IsTrue(Speed.FromKilometersPerHour(36) == Speed.FromMetersPerSecond(10));
            Assert.IsFalse(Speed.FromKilometersPerHour(36) == Speed.FromMetersPerSecond(11));
            Assert.IsFalse(Speed.FromKilometersPerHour(36) != Speed.FromMetersPerSecond(10));
            Assert.IsTrue(Speed.FromKilometersPerHour(36) != Speed.FromMetersPerSecond(11));
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
            Assert.IsTrue(Speed.Zero.IsBelow(1));
            Assert.IsFalse(Speed.FromMetersPerSecond(1).IsBelow(1));
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

    }
}
