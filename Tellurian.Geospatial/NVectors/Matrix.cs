namespace Tellurian.Geospatial.NVectors;

public struct Matrix
{
    internal const int Length = 3;
    internal double[,] Values { get; }

    internal Matrix(double[,] values)
    {
        Values = values;
    }

    internal Matrix Transposed
    {
        get
        {
            var m = Length;
            var n = Length;
            var transposed = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    transposed[i, j] = Values[j, i];
                }
            }
            return new(transposed);
        }
    }
}

internal static class MatrixExtensions
{
    public static bool IsSameAs(this Matrix me, Matrix other)
    {
        if (me.Values.GetLength(0) != other.Values.GetLength(0) ||
            me.Values.GetLength(1) != other.Values.GetLength(1))
            return false;
        for (int i = 0; i < me.Values.GetLength(0); i++)
            for (int j = 0; j < me.Values.GetLength(1); j++)
                if (me.Values[i, j] != other.Values[i, j])
                    return false;
        return true;
    }

}