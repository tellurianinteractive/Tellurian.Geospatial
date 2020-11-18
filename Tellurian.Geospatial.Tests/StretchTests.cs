using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tellurian.Geospatial.DistanceCalculators;

namespace Tellurian.Geospatial.Tests
{
    [TestClass]
    public class StretchTests
    {
        [TestMethod]
        public void LongStretchHaveCorrectProperties()
        {
            var target = Stretch.Between(TestData.Stockholm, TestData.Wellington);
            Assert.AreEqual(17445.0, target.Distance.Kilometers, 0.1);
            Assert.AreEqual(127.296, target.Direction.Degrees, 0.001);
            Assert.AreEqual(48.853, target.InitialBearing.Degrees, 0.001);
            Assert.AreEqual(149.276, target.FinalBearing.Degrees, 0.001);
            Assert.IsFalse(target.IsEastWestLine);
            Assert.IsFalse(target.IsZero);
        }

        [TestMethod]
        public void ShortStretchHaveCorrectProperties()
        {
            var target = Stretch.Between(TestData.Hövik, TestData.Höviksnäs);
            Assert.AreEqual(0.5620, target.Distance.Kilometers, 0.001);
            Assert.AreEqual(97.137, target.Direction.Degrees, 0.001);
            Assert.AreEqual(97.133, target.InitialBearing.Degrees, 0.001);
            Assert.AreEqual(97.141, target.FinalBearing.Degrees, 0.001);
            Assert.IsFalse(target.IsEastWestLine);
            Assert.IsFalse(target.IsZero);
        }

        [TestMethod]
        public void CreateWithDistanceCalculator()
        {
            var target = Stretch.Between(TestData.Hövik, TestData.Höviksnäs, new HaversineDistanceCalculator());
            Assert.AreEqual(TestData.Hövik, target.From);
            Assert.AreEqual(TestData.Höviksnäs, target.To);
        }

        [TestMethod]
        public void NullAsDistanceCalculatorUsesDefault()
        {
            var target = Stretch.Between(TestData.Hövik, TestData.Höviksnäs, null);
            Assert.AreEqual(0.5620, target.Distance.Kilometers, 0.001);
        }

        [TestMethod]
        public void EqualsIsCorrect()
        {
            Assert.AreEqual(Stretch.Between(TestData.Stockholm, TestData.Wellington), Stretch.Between(TestData.Stockholm, TestData.Wellington));
            Assert.AreNotEqual(Stretch.Between(TestData.Stockholm, TestData.Wellington), Stretch.Between(TestData.Hövik, TestData.Höviksnäs));
            Assert.AreNotEqual(Stretch.Between(TestData.Stockholm, TestData.Wellington), new object());
        }

        [TestMethod]
        public void XmlSerializationAndDeserializationWorks()
        {
            var target = Stretch.Between(Position.FromDegrees(58.1, 11.9), Position.FromDegrees(59.1, 12.9));
            var actual = SerializationTester<Stretch>.XmlSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void JsonSerializationAndDeserializationWorks()
        {
            var target = Stretch.Between(Position.FromDegrees(58.1, 11.9), Position.FromDegrees(59.1, 12.9));
            var actual = SerializationTester<Stretch>.JsonSerializeAndDeserialize(target);
            Assert.AreEqual(target, actual);
        }

        [TestMethod]
        public void Operators()
        {
            Assert.IsTrue(Stretch.Between(TestData.Stockholm, TestData.Wellington) == Stretch.Between(TestData.Stockholm, TestData.Wellington));
            Assert.IsFalse(Stretch.Between(TestData.Stockholm, TestData.Wellington) == Stretch.Between(TestData.Hövik, TestData.Höviksnäs));
            Assert.IsFalse(Stretch.Between(TestData.Stockholm, TestData.Wellington) != Stretch.Between(TestData.Stockholm, TestData.Wellington));
            Assert.IsTrue(Stretch.Between(TestData.Stockholm, TestData.Wellington) != Stretch.Between(TestData.Hövik, TestData.Höviksnäs));
        }

        [TestMethod]
        public void IsInverse()
        {
            var target = Stretch.Between(TestData.Hövik, TestData.Höviksnäs);
            var actual = target.Inverse;
            Assert.AreEqual(target.To, actual.From);
            Assert.AreEqual(target.From, actual.To);
        }

        [TestMethod]
        public void MinAngle()
        {
            var s = Stretch.Between(TestData.Hövik, TestData.Höviksnäs);
            Assert.AreEqual(Angle.FromDegrees(173.471107), s.MinAngle(Position.FromDegrees(58.033296, 11.750197)));
        }

        [TestMethod]
        public void IsEastWestLine()
        {
            var target = Stretch.Between(Position.FromDegrees(50, -4), Position.FromDegrees(50, 87));
            Assert.IsTrue(target.IsEastWestLine);
        }

        [TestMethod]
        public void IsZero()
        {
            var target = Stretch.Between(TestData.Hövik, TestData.Hövik);
            Assert.IsTrue(target.IsZero);
            Assert.IsTrue(target.Distance.IsZero);
        }

        [TestMethod]
        public void CrossTrackDistance()
        {
            var s = Stretch.Between(TestData.Hövik, TestData.Höviksnäs);
            Assert.AreEqual(Distance.FromMeters(20.950), s.CrossTrackDistance(Position.FromDegrees(58.033668, 11.743887)));
            Assert.AreEqual(Distance.FromMeters(15.855), s.CrossTrackDistance(Position.FromDegrees(58.033296, 11.750197)));
            Assert.AreEqual(Distance.FromMeters(23.632), s.CrossTrackDistance(Position.FromDegrees(58.032905, 11.755030)));
        }

        [TestMethod]
        public void OnTrackDistance()
        {
            var s = Stretch.Between(TestData.Hövik, TestData.Höviksnäs);
            Assert.AreEqual(Distance.FromMeters(62.640), s.OnTrackDistance(Position.FromDegrees(58.033668, 11.743887)));
            Assert.AreEqual(Distance.FromMeters(311.088), s.OnTrackDistance(Position.FromDegrees(58.033296, 11.750197)));
            Assert.AreEqual(Distance.FromMeters(598.805), s.OnTrackDistance(Position.FromDegrees(58.032905, 11.755030)));
        }
    }
}
