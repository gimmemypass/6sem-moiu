using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Collections;
using Accord.Math;

namespace lab2
{
    internal class lab2
    {
        private static List<int> basis_indexes;
        private static List<int> non_basis_indexes;
        private static double[,] matrix_B;
        private static double[,] inverse_matrix_B;
        private static double[,] matrix_N;
        private static double[,] zero_point;
        private static double[,] changeVector;
        private static int changeIndex;
        private static double[,] matrix;
        private static double[,] targetVector;

        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the dimension:");
            var dims = int.Parse(Console.ReadLine() ?? throw new Exception());
            Console.WriteLine("Enter the target function's coefs:");
            targetVector = MoiuLib.FillMatrix(1, dims);
            
            Console.WriteLine("Enter count of conditions:");
            var rows = int.Parse(Console.ReadLine() ?? throw new Exception());
            
            if(rows > dims)
                throw new Exception("count of conditions must be less or equal than dimension");
            
            Console.WriteLine("Fill the matrix:");
            matrix = MoiuLib.FillMatrix(rows, dims);

            Console.WriteLine("Enter zero point:");
            zero_point = MoiuLib.FillMatrix(1, dims);

            basis_indexes = zero_point.GetRow(0).Select(((d, i) => new {d, i})).OrderByDescending((arg => arg.d)).Take(rows).Select((arg => arg.i)).ToList();
            non_basis_indexes = zero_point.GetRow(0).Select(((d, i) => i)).ToList().Except(basis_indexes).ToList();

            for (int i = 1; ; i++)
            {
                Console.WriteLine($"№{i} ITERATION");
                if (!BringZeroPoint())
                    break;


            }
            Console.WriteLine("zero point is satisfying the requirements");
            Console.ReadKey();

        }

        private static bool BringZeroPoint()
        {
             matrix_B = GetSubMatrixByCols(matrix, basis_indexes.ToArray());
             matrix_N = GetSubMatrixByCols(matrix, non_basis_indexes.ToArray());
             
             Console.WriteLine("matrix_B");
             MoiuLib.PrintMatrix(matrix_B);
 
             inverse_matrix_B = inverse_matrix_B != null ? MoiuLib.Algorithm(matrix_B, inverse_matrix_B, changeVector, changeIndex ) : matrix_B.Inverse();
             Console.WriteLine("inverse matrix_b");
             MoiuLib.PrintMatrix(inverse_matrix_B);
 
             double[,] Cn = targetVector.GetRow(0).Where((d, i) => non_basis_indexes.Contains(i)).ToList().Make2DArray();
             double[,] Cb = targetVector.GetRow(0).Where((d, i) => basis_indexes.Contains(i)).ToList().Make2DArray();
             var div = FindDiv(Cn, Cb, inverse_matrix_B, matrix_N);
 
             Console.WriteLine("div");
             MoiuLib.PrintMatrix(div);
             var c_indexes_more_zero = div.GetRow(0).Select(((d, i) => new {d, i}))
                 .Where((arg => arg.d > 0))
                 .Select((arg => arg.i))
                 .ToList();
             
             if (c_indexes_more_zero.Count > 0)
             {
                 var c_index = c_indexes_more_zero[0];
                 var c_column = matrix.GetColumn(c_index);
                 var increment = inverse_matrix_B.Dot(c_column);
                 var tettas = new Dictionary<int, double>();
                 for (int i = 0; i < increment.Length; i++)
                 {
                     if(increment[i] > 0)
                         tettas[i] = zero_point[0, basis_indexes[i]] / increment[i];
                 }
                 var tetta_max = tettas.Min((pair => pair.Value));
                 
                 UpdateData(increment, tetta_max, c_index);
                 Console.WriteLine("Updated zero point vector");
                 MoiuLib.PrintMatrix(zero_point);
                 return true;
             }

             return false;

        }
        private static void UpdateData(double[] increment, double tettaMax, int index)
        {
            var result = new double[basis_indexes.Count];
            for (int i = 0; i < basis_indexes.Count; i++)
            {
                result[i] = zero_point[0,basis_indexes[i]];
            }
            
            result = result.Subtract(MoiuLib.MultiplyMatrixAndScalar(increment, tettaMax));
            var min_ind = result.IndexOf(result.Min());
            for (int i = 0; i < basis_indexes.Count; i++)
            {
                zero_point[0, basis_indexes[i]] = result[i];
            }
            zero_point[0, index] = tettaMax;  


            non_basis_indexes[non_basis_indexes.IndexOf(index)] = basis_indexes[min_ind];
            basis_indexes[min_ind] = index;

            changeIndex = min_ind;
            changeVector = MoiuLib.TranposeMatrix(matrix.GetCol(index).Make2DArray());

        }

        public static double[,] FindDiv(
            double[,] Cn,
            double[,] Cb,
            double[,] Ab_inv,
            double[,] An)
        {
            var step1 = MoiuLib.MultiplyMatrix(Cb, Ab_inv);
            var step2 = MoiuLib.MultiplyMatrix(step1, An);
            var step3 = MoiuLib.MultiplyMatrixAndScalar(step2, -1);
            var result = MoiuLib.SumMatrix(Cn, step3);
            return result;
        }

        private static double[,] GetSubMatrixByCols(double[,] matrix, int[] basis)
        {
            var cols = basis.Length;
            var rows = matrix.GetLength(0);
            var result = new double[rows, cols];
            for (var i = 0; i < cols; i++)
            {
                var index = basis[i];
                for (var j = 0; j < rows; j++)
                {
                    result[j, i] = matrix[j, index];
                }
            }
            return result;
        }   
    }
}