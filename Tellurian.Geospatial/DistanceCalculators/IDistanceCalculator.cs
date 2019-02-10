namespace Tellurian.Geospatial.DistanceCalculators
{
    /// <summary>
    /// Abtraction of distance calculation making it possible to 
    /// extend with other distance calcutaion formulas.
    /// </summary>
    public interface IDistanceCalculator
    {
        Distance GetDistance(Position from, Position to);
    }
}
