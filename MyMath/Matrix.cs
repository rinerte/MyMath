using System.Runtime.InteropServices;
using System.Text;

namespace MyMath
{
    public class Matrix
    {
        int rows;
        int columns;
        int[][] matrix;
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new int[rows][];
            for(int i=0;i<rows;i++) matrix[i] = new int[columns];
        }
        public Matrix(int rows, bool identityMatrix = false) 
        {
            this.rows = rows;
            this.columns = rows;
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
        public int Determinant(Matrix M = null)
        {
            if (M == null) M = this;

            if(M.Rows!= M.Columns) throw new ArgumentException(message: "Determinant exist only for square matrix");
            if(M.Rows ==1) return M[0][0];

            int sum = 0;
            for(int i = 0; i < M.Rows; i++)
            {
                Matrix minor = new(M.Rows - 1);
                for(int k = 1; k < M.Rows; k++)
                {
                    List<int> list = new();
                    for(int j = 0; j < M.Rows; j++)
                    {
                        if (j != i) list.Add(M[k][j]);
                    }
                    minor[k - 1] = list.ToArray();
                }
                sum += (int)Math.Pow(-1, i) * M[0][i] * Determinant(minor);
            }
            return sum;
        }

        public bool NonSingular()
        {
            return Determinant(this) != 0;
        }
        // Base operations
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
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows) throw new ArgumentException(message: "Can not substract matrix with different size");
            Matrix result = new Matrix(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = a[i][j]- b[i][j];
                }
            }
            return result;
        }
        public static Matrix operator +(Matrix a, int b)
        {
            if(a.Columns!=a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix result = new Matrix(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for(int j=0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = a[i][j] + b;
                    } else 
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix operator +(int b,Matrix a)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix result = new Matrix(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = a[i][j] + b;
                    }
                    else
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix operator -(int b, Matrix a)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can subtract number only with square matrix");
            Matrix result = new Matrix(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = b - a[i][j];
                    }
                    else
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix operator -( Matrix a, int b)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix result = new Matrix(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = a[i][j] - b;
                    }
                    else
                        result[i][j] = a[i][j];
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
        public static Matrix operator *( int b, Matrix a)
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
        public static Matrix operator !(Matrix a)
        {
            Matrix result = new Matrix(a.Columns, a.Rows);
            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    result[i][j] = a[j][i];
                }
            }
            return result;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < Rows; i++)
            {
                for(int j = 0;j < Columns; j++)
                {
                    sb.Append(this[i][j] + " ");
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public int[] this[int i]
        {
            get => matrix[i];
            set => matrix[i] = value;
        }
    }
}
