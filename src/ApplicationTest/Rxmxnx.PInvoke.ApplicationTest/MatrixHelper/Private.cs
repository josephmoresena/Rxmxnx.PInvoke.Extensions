using System;
using System.Buffers;
using System.Text;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal static partial class MatrixHelper
	{
		private const Double epsilon = 1e-12;

		private static readonly Double[,] emptyMatrix = new Double[0, 0];

		private static Double GetDeterminant(ReadOnlySpan<Double> matrix, Int32 n)
		{
			Double[]? array = default;
			Span<Double> tempMatrix = n > 4 ?
				(array = ArrayPool<Double>.Shared.Rent(matrix.Length)).AsSpan()[..matrix.Length] :
				stackalloc Double[matrix.Length];
			try
			{
				matrix.CopyTo(tempMatrix);

				Double det = 1;
				Int32 swaps = 0;

				for (Int32 row = 0; row < n; row++)
				{
					Double pivot = MatrixHelper.GetPivot(tempMatrix, n, false, row, out Int32 maxRow);

					if (Math.Abs(pivot) < MatrixHelper.epsilon)
						return 0;

					swaps += MatrixHelper.SwapRow(tempMatrix, n, row, maxRow) ? 1 : 0;

					for (Int32 k = row + 1; k < n; k++)
					{
						Double factor = tempMatrix[k * n + row] / tempMatrix[row * n + row];
						for (Int32 j = row; j < n; j++)
							tempMatrix[k * n + j] -= factor * tempMatrix[row * n + j];
					}
				}

				for (Int32 i = 0; i < n; i++)
					det *= tempMatrix[i * n + i];
				if (swaps % 2 != 0) det = -det;
				return det;
			}
			finally
			{
#if !CSHARP_90
				if (array != null)
#else
				if (array is not null)
#endif
					ArrayPool<Double>.Shared.Return(array);
			}
		}
		private static Double GetDeterminant(ReadOnlySpan<Double> matrix, Int32 n, out Double[,] inverse)
		{
			Double[]? array = default;
			Span<Double> augmented = n > 4 ?
				(array = ArrayPool<Double>.Shared.Rent(2 * matrix.Length)).AsSpan()[..(2 * matrix.Length)] :
				stackalloc Double[2 * matrix.Length];
			try
			{
				Double determinant = 1.0;
				Int32 swaps = 0;

				MatrixHelper.FillAugmented(matrix, n, augmented);
				for (Int32 row = 0; row < n; row++)
				{
					Double pivot = MatrixHelper.GetPivot(augmented, n, true, row, out Int32 maxRow);
					if (Math.Abs(pivot) < MatrixHelper.epsilon)
					{
						inverse = MatrixHelper.emptyMatrix;
						return 0;
					}

					determinant *= pivot;
					swaps += MatrixHelper.SwapRow(augmented, 2 * n, row, maxRow) ? 1 : 0;
					MatrixHelper.NormalizeRow(augmented, n, row, pivot);
					MatrixHelper.RowReduction(augmented, n, row);
				}

				inverse = new Double[n, n];
				if (swaps % 2 != 0)
					determinant = -determinant;
				MatrixHelper.CopyInverse(augmented, n, inverse.AsSpan());
				return determinant;
			}
			finally
			{
#if !CSHARP_90
				if (array != null)
#else
				if (array is not null)
#endif
					ArrayPool<Double>.Shared.Return(array);
			}
		}
		private static void MultiplyMatrices(ReadOnlySpan<Double> matrixA, ReadOnlySpan<Double> matrixB, Int32 aRow,
			Int32 bCol, Int32 n, Span<Double> result)
		{
			for (Int32 row = 0; row < aRow; row++)
			for (Int32 aCol = 0; aCol < bCol; aCol++)
			{
				Double sum = 0;
				for (Int32 col = 0; col < n; col++)
					sum += matrixA[row * n + col] * matrixB[col * bCol + aCol];
				result[row * bCol + aCol] = sum;
			}
		}

		private static void FillAugmented(ReadOnlySpan<Double> matrix, Int32 n, Span<Double> augmented)
		{
			// [A | I]
			for (Int32 row = 0; row < n; row++)
			for (Int32 col = 0; col < n; col++)
			{
				(augmented[row * 2 * n + col], augmented[row * 2 * n + col + n]) =
					(matrix[row * n + col], row == col ? 1.0 : 0.0);
			}
		}
		private static void CopyInverse(Span<Double> augmented, Int32 n, Span<Double> inverseAsSpan)
		{
			for (Int32 i = 0; i < n; i++)
			for (Int32 j = 0; j < n; j++)
				inverseAsSpan[i * n + j] = augmented[i * 2 * n + j + n];
		}
		private static void NormalizeRow(Span<Double> augmented, Int32 n, Int32 row, Double pivot)
		{
			for (Int32 col = 0; col < 2 * n; col++)
				augmented[row * 2 * n + col] /= pivot;
		}
		private static void RowReduction(Span<Double> augmented, Int32 n, Int32 normalizedRow)
		{
			for (Int32 row = 0; row < n; row++)
			{
				if (row == normalizedRow) continue;
				Double factor = augmented[row * 2 * n + normalizedRow];
				for (Int32 col = 0; col < 2 * n; col++)
					augmented[row * 2 * n + col] -= factor * augmented[normalizedRow * 2 * n + col];
			}
		}
		private static Double GetPivot(ReadOnlySpan<Double> matrix, Int32 n, Boolean isAugmented, Int32 currentRow,
			out Int32 maxRow)
		{
			maxRow = currentRow;

			Int32 multiplier = isAugmented ? 2 : 1;
			for (Int32 row = currentRow + 1; row < n; row++)
			{
				if (Math.Abs(matrix[row * multiplier * n + currentRow]) >
				    Math.Abs(matrix[maxRow * multiplier * n + currentRow]))
					maxRow = row;
			}
			return matrix[maxRow * multiplier * n + currentRow];
		}
		public static void PrintMatrix(ReadOnlySpan<Double> matrix, Int32 nRow, Int32 nCol)
		{
			Int32 addCol = (matrix.Length - nRow * nCol) / nRow;
			StringBuilder? strBuild = addCol > 0 ? new StringBuilder() : default;
			for (Int32 row = 0; row < nRow; row++)
			{
				strBuild?.Append("\t|\t");
				for (Int32 col = 0; col < nCol; col++)
				{
					Console.Write($"{matrix[row * (nCol + addCol) + col]:0.####}\t");
					if (strBuild is null || col >= addCol) continue;
					strBuild.Append($"{matrix[row * (nCol + addCol) + col + nRow]:0.####}\t");
				}
				Console.WriteLine(strBuild?.ToString());
				strBuild?.Clear();
			}
		}
		private static Boolean SwapRow(Span<Double> matrix, Int32 nCol, Int32 row1, Int32 row2)
		{
			if (row1 == row2) return false;
			for (Int32 col = 0; col < nCol; col++)
			{
				(matrix[row1 * nCol + col], matrix[row2 * nCol + col]) = (matrix[row2 * nCol + col],
					matrix[row1 * nCol + col]);
			}
			return true;
		}
	}
}