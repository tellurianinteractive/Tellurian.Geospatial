namespace Tellurian.Geospatial.DistanceCalculators
{
    /// <summary>
    /// Abtraction of distance calculation making it possible to 
    /// extend with other distance calcutaion formulas.
    /// </summary>
    public interface IDistanceCalculator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
            Justification = "We ignore this here.")]
        Distance GetDistance(in Position from, in Position to);
    }
}
