using System;
using System.Globalization;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal static partial class MatrixHelper
	{
		public static Double GetDeterminant(Double[,] matrix)
		{
			Int32 n;
			if (matrix.Rank != 2 || (n = matrix.GetLength(0)) != matrix.GetLength(1))
				throw new InvalidOperationException("Determinant is only for square matrices.");
			if (n == 0)
				return 1;

			Double determinant = MatrixHelper.GetDeterminant(matrix.AsSpan(), n);
			return determinant;
		}
		public static Double GetDeterminant(Double[,] matrix, out Double[,] inverse)
		{
			Int32 n;
			if (matrix.Rank != 2 || (n = matrix.GetLength(0)) != matrix.GetLength(1))
				throw new InvalidOperationException("Determinant is only for square matrices.");
			if (n == 0)
			{
				inverse = MatrixHelper.emptyMatrix;
				return 1;
			}

			Double determinant = MatrixHelper.GetDeterminant(matrix.AsSpan(), n, out inverse);
			return determinant;
		}
		public static Double[,] Multiply(Double[,] matrixA, Double[,] matrixB)
		{
			if (matrixA.Rank != 2)
				throw new InvalidOperationException("Invalid A matrix.");
			if (matrixB.Rank != 2)
				throw new InvalidOperationException("Invalid B matrix.");

			Int32 aRow = matrixA.GetLength(0);
			Int32 bCol = matrixB.GetLength(1);
			Int32 n;
			if ((n = matrixA.GetLength(1)) != matrixB.GetLength(0))
				throw new InvalidOperationException(
					"The number of columns in A matrix must equal the number of rows in B matrix.");

			if (aRow == 0 || n == 0 || bCol == 0) return MatrixHelper.emptyMatrix;
			Double[,] result = new Double[aRow, bCol];

			MatrixHelper.MultiplyMatrices(matrixA.AsSpan(), matrixB.AsSpan(), aRow, bCol, n, result.AsSpan());
			return result;
		}
		public static String[,] ToText(Double[,] source)
		{
			String[,] result = new String[source.GetLength(0), source.GetLength(1)];
			ReadOnlySpan<Double> sourceSpan = source.AsSpan();
			Span<String> resultSpan = result.AsSpan();
			for (Int32 i = 0; i < sourceSpan.Length; i++)
				resultSpan[i] = sourceSpan[i].ToString(CultureInfo.InvariantCulture);
			return result;
		}
		public static void Print(Double[,] matrix)
		{
			if (matrix.Rank != 2)
				throw new InvalidOperationException("Invalid matrix.");
			if (matrix.GetLength(0) <= 0 || matrix.GetLength(0) <= 0) return;
			MatrixHelper.PrintMatrix(matrix.AsSpan(), matrix.GetLength(0), matrix.GetLength(1));
		}
	}
}