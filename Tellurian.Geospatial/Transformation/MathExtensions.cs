namespace Tellurian.Geospatial.Transformation
{
    /// <summary>
    /// Supplementary functions missing in <see cref="System.Math"/>.
    /// </summary>
    internal static class Math
    {
        public static double Atanh(double value) { return 0.5 * System.Math.Log((1.0 + value) / (1.0 - value)); }
    }
}
