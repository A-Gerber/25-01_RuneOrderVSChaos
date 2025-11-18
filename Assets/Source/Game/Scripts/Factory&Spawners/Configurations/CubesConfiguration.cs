using System.Collections.Generic;
using UnityEngine;

internal abstract class CubesConfiguration
{
    private const int Divider = 2;
    private const float EquiprobableCoefficient = 0.5f;

    private int OffsetX;
    private int OffsetZ;

    protected int[,] Configuration;

    internal virtual List<LocalPosition> GenerateConfiguration()
    {
        Configuration = GetStartConfiguration();

        Transpose();
        CalculateOffsets();
        CalculateCoefficients(out int CoefficientX, out int CoefficientZ);

        List<LocalPosition> coordinate = new();

        for (int i = 0; i < Configuration.GetLength(0); i++)
        {
            for (int j = 0; j < Configuration.GetLength(1); j++)
            {
                if (Configuration[i, j] == 1)
                {
                    coordinate.Add(new LocalPosition((i - OffsetX) * CoefficientX, (j - OffsetZ) * CoefficientZ));
                }
            }
        }

        return coordinate;
    }

    protected abstract int[,] GetStartConfiguration();
    protected abstract bool IsCalculateCoefficients();
    protected abstract bool IsTranspose();

    private void CalculateOffsets()
    {
        OffsetX = Configuration.GetLength(0) / Divider;
        OffsetZ = Configuration.GetLength(1) / Divider;
    }

    private void CalculateCoefficients(out int CoefficientX, out int CoefficientZ)
    {
        if (IsCalculateCoefficients())
        {
            int[,] coefficients = new int[,] {
                    { 1, 1 },
                    { 1, -1 },
                    { -1, 1 },
                    { -1, -1 }
                };

            int index = Random.Range(0, coefficients.GetLength(0));

            CoefficientX = coefficients[index, 0];
            CoefficientZ = coefficients[index, 1];
        }
        else
        {
            CoefficientX = 1;
            CoefficientZ = 1;
        }
    }

    private void Transpose()
    {
        if (IsTranspose() && Random.value > EquiprobableCoefficient)
        {
            (OffsetZ, OffsetX) = (OffsetX, OffsetZ);

            int rows = Configuration.GetLength(0);
            int cols = Configuration.GetLength(1);

            int[,] transposed = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    transposed[j, i] = Configuration[i, j];
                }
            }

            Configuration = transposed;
        }
    }
}