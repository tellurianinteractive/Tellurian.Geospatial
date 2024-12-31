namespace Tellurian.Geospatial.DistanceCalculators;

/// <summary>
/// Abtraction of distance calculation making it possible to 
/// extend with other distance calcutaion formulas.
/// </summary>
public interface IDistanceCalculator
{
    /// <summary>
    /// Method to implement.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>Shall return the <see cref="Distance"/> between twp <see cref="Position">positions</see>.</returns>
    Distance GetDistance(in Position from, in Position to);
}
