using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tellurian.Geospatial.Transformation.Tests
{
    [TestClass]
    public class EllipsoidTests
    {
        const double Tolerance = 0.000000000001;

        [TestMethod]
        public void CreateFromSemiAxes()
        {
            var target = EarthEllipsoid.CreateFromSemiAxes(6378137.0, 6356752.31414);
            AssertEllipsoid(target, 6378137.0, 6356752.31414, 0.00335281068123805);
        }

        [TestMethod]
        public void CreateFromSemiMajorAxisAndFlattening()
        {
            var target = EarthEllipsoid.CreateFromSemiMajorAxisAndFlattening(6378137.0, 0.00335281068123805);
            AssertEllipsoid(target, 6378137.0, 6356752.31414, 0.00335281068123805);
        }

        [TestMethod]
        public void CreateGrs80()
        {
            var target = Ellipsoids.Grs80;
            AssertEllipsoid(target, 6378137.0, 6356752.31414, 0.00335281068123805);
        }

         [TestMethod]
        public void CreateWgs84()
        {
            var target = Ellipsoids.Wgs84;
            AssertEllipsoid(target, 6378137, 6356752.3142, 0.00335281067183099);
        }

        private static void AssertEllipsoid(EarthEllipsoid actual, double expectedSemiMajorAxis, double expectedSemiMinorAxis, double expectedFlattening)
        {
            const double Tolerance = 0.000000000001;
            Assert.AreEqual(expectedSemiMajorAxis, actual.SemiMajorAxis, Tolerance);
            Assert.AreEqual(expectedSemiMinorAxis, actual.SemiMinorAxis, Tolerance);
            Assert.AreEqual(expectedFlattening, actual.Flattening, Tolerance);
        }
    }
}
