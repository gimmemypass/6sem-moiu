using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace lab2
{
    public static class ArrayExt
    {
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row*cols*size, result, 0, cols*size);

            return result;
        }

        public static T[] GetCol<T>(this T[,] array, int col)
        {
            if (array == null)
                throw new ArgumentNullException("array");           
            

            int rows = array.GetLength(0);
            T[] result = new T[rows];

            for (int i = 0; i < rows; i++)
                result[i] = array[i, col];
            return result;
        }
        public static T[,] Make2DArray<T>(this T[] input, int rows = 1)
        {
            var cols = input.Length / rows; 
            T[,] output = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    output[i, j] = input[i * cols + j];
                }
            }
            return output;
        } 
        
        public static T[,] Make2DArray<T>(this List<T> input, int rows = 1)
        {
            var cols = input.Count / rows; 
            T[,] output = new T[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    output[i, j] = input[i * cols + j];
                }
            }
            return output;
        } 
   }
}