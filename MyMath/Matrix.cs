using System.Runtime.InteropServices;

namespace MyMath
{
    public class Matrix
    {
        int rows;
        int columns;
        int[][] matrix;
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new int[rows][];
            for(int i=0;i<rows;i++) matrix[i] = new int[columns];
        }
        /// <summary>
        /// Square matrix
        /// </summary>
        /// <param name="rows"></param>
        public Matrix(int rows, bool identityMatrix = false) 
        {
            this.rows = rows;
            this.rows = rows;
            matrix = new int[rows][];
            for (int i = 0; i < rows; i++) matrix[i] = new int[rows];

            if (identityMatrix)
            {
                for (int i = 0; i < rows; i++)
                {
                    matrix[i][i] = 1;
                }
            }
        }
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }        
        public static Matrix operator + (Matrix a, Matrix b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows) throw new ArgumentException(message: "Can not sum matrix with different size");

            Matrix result = new Matrix(a.Rows, a.Columns);

            for(int i = 0; i < a.Rows; i++)
            {
                for(int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = a[i][j] + b[i][j];
                }
            }
            return result;
        }
        public static Matrix operator +(Matrix a, int b)
        {
            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = a[i][j] + b;
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix a, int b)
        {
            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = a[i][j] * b;
                }
            }
            return result;
        }
        public static Matrix operator -(Matrix a)
        {
            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = a[i][j] * -1;
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Columns != b.Rows) throw new ArgumentException(message: "To multiply matrices, number of columns in first matrix must be equal to number of columns in second matrix");

            Matrix result = new Matrix(a.Rows, b.Columns);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    int r = 0;
                    for(int k = 0; k < a.Columns; k++)
                    {
                        r += a[i][k] * b[k][j];
                    }
                    result[i][j] = r;
                }
            }
            return result;
        }
        public void Display()
        {
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0;j < Columns; j++)
                {
                    Console.Write(this[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public int[] this[int i]
        {
            get => matrix[i];
            set => matrix[i] = value;
        }


    }
}
