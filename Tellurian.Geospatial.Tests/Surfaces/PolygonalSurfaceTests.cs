using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tellurian.Geospatial.Tests;

namespace Tellurian.Geospatial.Surfaces.Tests
{
    [TestClass]
    public class PolygonalSurfaceTests
    { 
        [TestMethod]
        public void PositionIsInsidePolygon()
        {
            var target = TestPolygonalSurface;
            Assert.IsTrue(target.Includes(Position.FromDegrees(58.071851, 11.823159)));
        }

        [TestMethod]
        public void PositionIsOutsidePolygon()
        {
            var target = TestPolygonalSurface;
            Assert.IsFalse(target.Includes(Position.FromDegrees(58.073851, 11.823159)));
        }

        [TestMethod]
        public void CornerIsWithinPolygon()
        {
            var target = TestPolygonalSurface;
            Assert.IsTrue(target.BorderPositions.All(p => p.IsWithin(target)));
        }

        [TestMethod]
        public void XmlSerializationAndDeserializationWorks()
        {
            var target = TestPolygonalSurface;
            var actual = SerializationTester<PolygonalSurface>.XmlSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void JsonSerializationAndDeserializationWorks()
        {
            var target = TestPolygonalSurface;
            var actual = SerializationTester<PolygonalSurface>.JsonSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        // NOTE: For some reason record value equality does not work for PolygonalSurface
        private static bool AreValueEqual(PolygonalSurface a, PolygonalSurface b) =>
            a.ReferencePosition == b.ReferencePosition && Enumerable.SequenceEqual(a.BorderPositions, b.BorderPositions);

        private static PolygonalSurface TestPolygonalSurface => 
            new PolygonalSurface(new[] {
                Position.FromDegrees(58.072363, 11.823334),
                Position.FromDegrees(58.072232, 11.823686),
                Position.FromDegrees(58.071205, 11.822731),
                Position.FromDegrees(58.071406, 11.822586),
                Position.FromDegrees(58.072166, 11.823358),
                Position.FromDegrees(58.072265, 11.823224)
            }, Position.FromDegrees(58.071740, 11.823197));
    }
}
