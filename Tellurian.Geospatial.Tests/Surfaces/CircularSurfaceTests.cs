using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tellurian.Geospatial.Tests;

namespace Tellurian.Geospatial.Surfaces.Tests
{
    [TestClass]
    public class CircularSurfaceTests
    {
        [TestMethod]
        public void PositionIsInsideCircle()
        {
            var target = new CircularSurface(Position.FromDegrees(58.072363, 11.823334), Distance.FromMeters(1));
            Assert.IsTrue(target.Includes(Position.FromDegrees(58.072363, 11.823334)));
        }

        [TestMethod]
        public void PositionIsOutsideCircle()
        {
            var target = new CircularSurface(Position.FromDegrees(58.072363, 11.823334), Distance.FromMeters(1));
            Assert.IsFalse(target.Includes(Position.FromDegrees(58.072363, 11.825)));
        }

        [TestMethod]
        public void XmlSerializationAndDeserializationWorks()
        {
            var target = new CircularSurface(Position.FromDegrees(58.1,11.9), Distance.FromMeters(5));
            var actual = SerializationTester<CircularSurface>.XmlSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void JsonSerializationAndDeserializationWorks()
        {
            var target = new CircularSurface(Position.FromDegrees(58.1, 11.9), Distance.FromMeters(5));
            var actual = SerializationTester<CircularSurface>.JsonSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }
    }
}
