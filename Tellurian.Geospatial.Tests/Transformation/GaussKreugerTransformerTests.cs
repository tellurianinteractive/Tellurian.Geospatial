namespace Tellurian.Geospatial.Transformation.Tests;

[TestClass]
public class GaussKreugerTransformerTests
{
    [TestMethod]
    public void RT90ToSweref99()
    {
        var expected = Position.FromDegrees(60.666450715376, 17.1324873919442);
        var projection = MapProjections.Rt90;
        var coordinate = new GridCoordinate(6728429, 1572570);
        var actual = GaussKreugerTransformer.ToPosition(coordinate, projection);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Sweref99ToRT90()
    {
        var expected = new GridCoordinate(6728429, 1572570); ;
        var projection = MapProjections.Rt90;
        var position = Position.FromDegrees(60.666450715376, 17.1324873919442);
        var actual = GaussKreugerTransformer.ToGridCoordinate(position, projection);
        Assert.AreEqual(expected, actual);
    }
}
