using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;

namespace MyMath
{
    public class Matrix<T> where T : struct, IConvertible, IComparable, IFormattable
    {
        // plan: why I did it generic?
        // remove this changes
        int rows;
        int columns;
        T[][] matrix;
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new T[rows][];
            for(int i=0;i<rows;i++) matrix[i] = new T[columns];
        }
        public Matrix(int rows, bool identityMatrix = false) 
        {
            this.rows = rows;
            this.columns = rows;
            matrix = new T[rows][];
            for (int i = 0; i < rows; i++) matrix[i] = new T[rows];

            if (identityMatrix)
            {
                for (int i = 0; i < rows; i++)
                {
                    matrix[i][i] = (T)Convert.ChangeType(1, typeof(T));
                }
            }
        }
        public static Matrix<T> operator !(Matrix<T> a)
        {
            T D = a.Determinant();
            if (D.Equals(default(T))) throw new ArgumentException(message: "Determinant is equal to 0, matrix is singular, inverse matrix does not exist");

            Matrix<T> AT = new(a.Rows);
            
            for (int i = 0; i < AT.Rows; i++)
            {
                for (int j = 0; j < AT.Rows; j++)
                {
                    AT[i][j] = (T)Convert.ChangeType(Math.Pow(-1, i + j) * Convert.ToDouble(Minor(a, i, j).Determinant()), typeof(T));
                }
            }

            //AT = ~AT;
            ////AT = AT * (1 / Convert.ToDouble(D));
            //AT = Multiply(AT, Divide((T)Convert.ChangeType(1, typeof(T)), (T)Convert.ChangeType(D, typeof(T))));
            //return AT;

            T inverseD = (T)Convert.ChangeType(1 / Convert.ToDouble(D), typeof(T));
            AT = AT * inverseD;
            return AT;
        }
        public T Determinant(Matrix<T> M = null)
        {
            if (M == null) M = this;

            if(M.Rows!= M.Columns) throw new ArgumentException(message: "Determinant exist only for square matrix");
            if(M.Rows ==1) return M[0][0];

            double sum = 0;
            for (int i = 0; i < M.Rows; i++)
            {
                Matrix<T> minor = Minor(M, 0, i);
                sum += Math.Pow(-1, i) * Convert.ToDouble(M[0][i]) * Convert.ToDouble(Determinant(minor));
            }
            return (T)Convert.ChangeType(sum, typeof(T));
        }
        //static Matrix Minor(Matrix M, int im, int jm)
        //{
        //    if(M.Rows!=M.Columns || M.Rows<2) throw new ArgumentException("Too small matrix");
        //    Matrix minor = new(M.Rows - 1);

        //    int index = 0;
        //    for (int i = 0; i < M.Rows; i++)
        //    {
        //        List<int> list = new();
        //        for(int j=0; j < M.Rows; j++)
        //        {
        //            if (i != im && j!=jm)
        //            {
        //                list.Add(M[i][j]);
        //            }
        //        }
        //        minor[index] = list.ToArray();
        //        if (i != im) index++;
        //    }
        //    return minor;
        //}
        static Matrix<T> Minor(Matrix<T> M, int rowToRemove, int colToRemove)
        {
            if (M.Rows != M.Columns || M.Rows < 2) throw new ArgumentException("Too small matrix");
            Matrix<T> minor = new Matrix<T>(M.Rows - 1, M.Columns - 1);

            int minorRow = 0;
            for (int i = 0; i < M.Rows; i++)
            {
                if (i == rowToRemove) continue;
                int minorCol = 0;
                for (int j = 0; j < M.Columns; j++)
                {
                    if (j == colToRemove) continue;
                    minor[minorRow][minorCol] = M[i][j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }

        public bool NonSingular()
        {
            return !Determinant(this).Equals(default(T));
        }
        // Base operations
        public static Matrix<T> operator +(Matrix<T> a, Matrix<T> b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows) throw new ArgumentException(message: "Can not sum matrix with different size");

            Matrix<T> result = new Matrix<T>(a.Rows, a.Columns);

            for(int i = 0; i < a.Rows; i++)
            {
                for(int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = Add(a[i][j] , b[i][j]);
                }
            }
            return result;
        }
        public static Matrix<T> operator -(Matrix<T> a, Matrix<T> b)
        {
            if (a.Columns != b.Columns || a.Rows != b.Rows) throw new ArgumentException(message: "Can not substract matrix with different size");
            Matrix<T> result = new Matrix<T>(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = Subtract( a[i][j], b[i][j]);
                }
            }
            return result;
        }
        public static Matrix<T> operator +(Matrix<T> a, T b)
        {
            if(a.Columns!=a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix<T> result = new Matrix<T>(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for(int j=0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = Add( a[i][j] , b);
                    } else 
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix<T> operator +(T b, Matrix<T> a)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix<T> result = new Matrix<T>(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] =Add( a[i][j],b);
                    }
                    else
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix<T> operator -(T b, Matrix<T> a)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can subtract number only with square matrix");
            Matrix<T> result = new Matrix<T>(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = Subtract( b ,a[i][j]);
                    }
                    else
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }
        public static Matrix<T> operator -(Matrix<T> a, T b)
        {
            if (a.Columns != a.Rows) throw new ArgumentException(message: "You can add number only to square matrix");
            Matrix<T> result = new Matrix<T>(a.Rows, a.Rows);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    if (i == j)
                    {
                        result[i][j] = Subtract(a[i][j],b);
                    }
                    else
                        result[i][j] = a[i][j];
                }
            }
            return result;
        }


        public static Matrix<T> operator *(Matrix<T> a, T b)
        {
            Matrix<T> result = new Matrix<T>(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = Multiply(a[i][j], b);
                }
            }
            return result;
        }
        public static Matrix<T> operator *( T b, Matrix<T> a)
        {
            Matrix<T> result = new Matrix<T>(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                    result[i][j] = Multiply(a[i][j], b);
                }
            }
            return result;
        }
        public static Matrix<T> operator -(Matrix<T> a)
        {
            Matrix<T> result = new Matrix<T>(a.Rows, a.Columns);

            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < a.Columns; j++)
                {
                   result[i][j] = Negate(a[i][j]);
                }
            }
            return result;
        }
        public static Matrix<T> operator *(Matrix<T> a, Matrix<T> b)
        {
            if (a.Columns != b.Rows) throw new ArgumentException(message: "To multiply matrices, number of columns in first matrix must be equal to number of columns in second matrix");

            Matrix<T> result = new Matrix<T>(a.Rows, b.Columns);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    T sum = default(T);
                    for (int k = 0; k < a.Columns; k++)
                    {
                        sum = sum = Add(sum, Multiply(a[i][k], b[k][j])); 
                    }
                    result[i][j] = sum;
                }
            }
            return result;
        }
       
        public static Matrix<T> operator ~(Matrix<T> a)
        {
            Matrix<T> result = new Matrix<T>(a.Columns, a.Rows);
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
        public T[] this[int i]
        {
            get => matrix[i];
            set => matrix[i] = value;
        }
        private static readonly Func<T, T, T> Add = CreateBinaryOperation(Expression.Add);
        private static readonly Func<T, T, T> Subtract = CreateBinaryOperation(Expression.Subtract);
        private static readonly Func<T, T, T> Multiply = CreateBinaryOperation(Expression.Multiply);
        private static readonly Func<T, T> Negate = CreateUnaryOperation(Expression.Negate);
        private static readonly Func<T, T, T> Divide = CreateBinaryOperation(Expression.Divide);


        private static Func<T, T, T> CreateBinaryOperation(Func<Expression, Expression, BinaryExpression> operation)
        {
            var paramA = Expression.Parameter(typeof(T));
            var paramB = Expression.Parameter(typeof(T));
            var body = operation(paramA, paramB);
            return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
        }

        private static Func<T, T> CreateUnaryOperation(Func<Expression, UnaryExpression> operation)
        {
            var param = Expression.Parameter(typeof(T));
            var body = operation(param);
            return Expression.Lambda<Func<T, T>>(body, param).Compile();
        }

    }
}
