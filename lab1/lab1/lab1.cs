using System;
using System.Collections.Generic;
using System.Linq;

namespace lab1
{
    internal class lab1
    {
        public static void Main(string[] args)
        {
            var sizeNumber = Console.ReadLine()?.Split(' ').Select(int.Parse).ToArray();
            var size = sizeNumber[0];
            var vectorNumber = sizeNumber[1] - 1;
 
            var A = Lab1Alg.FillMatrix(size, size);
            var B = Lab1Alg.FillMatrix(size, size);
 
            var x = Lab1Alg.TranposeMatrix(Lab1Alg.FillMatrix(1, size));
 
            for (int i = 0; i < size; i++)
            {
                A[i, vectorNumber] = x[i,0];
            }
            Console.WriteLine("A`");
            var inverseMatrix = Lab1Alg.Algorithm(A, B, x, vectorNumber);
            Lab1Alg.PrintMatrix(inverseMatrix);
        }


    }
}