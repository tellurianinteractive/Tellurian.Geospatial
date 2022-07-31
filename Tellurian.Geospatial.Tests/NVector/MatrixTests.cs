using Tellurian.Geospatial.NVectors;

namespace Tellurian.Geospatial.Tests.NVector;

[TestClass]
public class MatrixTests
{
    [TestMethod]
    public void TransposesCorrectly()
    {
        var target = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
        var expected = new Matrix(new double[,] { { 1, 4, 7 }, { 2, 5, 8 }, { 3, 6, 9 } });
        var actual = target.Transposed;
        Assert.IsTrue(actual.IsSameAs(expected));
    }
}
