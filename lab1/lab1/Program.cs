using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var size_number = Console.ReadLine()?.Split(' ').Select(int.Parse).ToArray();
            var size = size_number[0];
            var vectorNumber = size_number[1] - 1;

            var A = FillMatrix(size, size);
            var B = FillMatrix(size, size);

            var x = TranposeMatrix(FillMatrix(1, size));

            for (int i = 0; i < size; i++)
            {
                A[i, vectorNumber] = x[i,0];
            }
            Console.WriteLine("A`");
            PrintMatrix(A);
            var inverseMatrix = Algorithm(A, B, x, vectorNumber);

            PrintMatrix(inverseMatrix);
        }

        private static void PrintMatrix(double[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                    Console.Write($"{matrix[i,j]}  ");
                Console.WriteLine();
            }
        }

        private static double[,] Algorithm(
            double[,] A,
            double[,] B,
            double[,] x,
            int vectorNumber)
        {
            var l = MultiplyMatrix(B, x);
            Console.WriteLine("l");
            PrintMatrix(l);
            var l1 = l;
            var k = l1[vectorNumber, 0]; 
            l1[vectorNumber, 0] = -1;
            for(var i = 0; i < l1.GetLength(0); i++)
            {
                l1[i, 0] /= -k;
            }
            Console.WriteLine("l1");
            PrintMatrix(l1);
            var D = GetIdentityMatrix(A.GetLength(0));
            Console.WriteLine("D");
            for (var i = 0; i < A.GetLength(0); i++)
                D[i, vectorNumber] = l1[i,0];
            PrintMatrix(D);
            return MultiplyMatrix(D, B);
        }
        
        #region HelpMethods
        private static double[,] FillMatrix(int row, int col)
        {
            var matrix = new double[row, col];
            for (var i = 0; i < row; i++)
            {
                var numbers = Console.ReadLine()?.Split(' ').Select(int.Parse).ToArray();
                for (var j = 0; j < col; j++)
                {
                    matrix[i,j] = numbers[j];
                }
            }
            return matrix;
        }

        private static double[,] GetIdentityMatrix(int size)
        {
            var matrix = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    matrix[i, j] = (i == j) ? 1 : 0;
            return matrix;
        }
        private static double[,] TranposeMatrix(double[,] matrix)
        {
            var row = matrix.GetLength(0);
            var col = matrix.GetLength(1);
            
            var newArray = new double[col, row];
            for (int j = 0; j < col; j++)
            for (int r = 0; r < row; r++)
                newArray[j, r] = matrix[r, j];
            return newArray;
        }
        
        private static double[,] MultiplyMatrix(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[,] kHasil = new double[rA, cB];
            if (cA != rB)
            {
                throw new Exception("can NOT be multiplied");
            }
            for (int i = 0; i < rA; i++)
            {
                for (int j = 0; j < cB; j++)
                {
                    temp = 0;
                    for (int k = 0; k < cA; k++)
                    {
                        temp += A[i, k] * B[k, j];
                    }
                    kHasil[i, j] = temp;
                }
            }
            return kHasil;
        }

        #endregion
    }
}