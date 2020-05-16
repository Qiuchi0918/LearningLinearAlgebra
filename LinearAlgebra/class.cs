using System;
namespace LinearAlgebra
{
    public static class Settings
    {
        public static double ignoreRoundOffThreshold = 0.0000001;
    }
    public class CustomException
    {
        public class MatrixNotInvertibleException : Exception { }
        public class MatrixNotSquareException : Exception { }
        public class NoneZeroEntryNotFoundException : Exception { }
        public class MatrixOperationNotDefinedException : Exception { }
    }
    public class Array2
    {
        public Array2(double[][] mat)
        {
            storage = mat;
            rowCount = storage.GetLength(0);
            colCount = storage[0].Length;
        }
        public Array2(int row, int col)
        {
            this.storage = new double[row][];
            rowCount = row;
            colCount = col;
            for (int curRow = 0; curRow < row; curRow++)
            {
                this.storage[curRow] = new double[col];
            }
        }
        public Array2(int row, int col, params double[] entries)
        {
            if (row * col == entries.Length)
            {
                int curEnt = 0;
                rowCount = row;
                colCount = col;
                this.storage = new double[row][];
                for (int curRow = 0; curRow < row; curRow++)
                {
                    this.storage[curRow] = new double[col];
                    for (int curCol = 0; curCol < col; curCol++)
                    {
                        storage[curRow][curCol] = entries[curEnt++];
                    }
                }
            }
        }
        protected double[][] storage;
        public int rowCount, colCount;
        public int rank = -1;
        public int MaxRank => rowCount < colCount ? rowCount : colCount;
        public double[] this [int row]
        {
            get => storage[row];
            set => storage[row] = value;
        }
        /// <summary>
        /// 显示Array2于控制台上
        /// </summary>
        public void Display()
        {
            for (int curRow = 0; curRow < rowCount; curRow++)
            {
                for (int curCol = 0; curCol < colCount; curCol++)
                {
                    Console.Write(string.Format("{0,-10}", Math.Round(storage[curRow][curCol], 3)));
                }
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine();
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine();
            }
        }
        /// <summary>
        /// 判断行数列数是否分别相等
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public bool OfSameSize(Array2 array)
        {
            return colCount == array.colCount && rowCount == array.rowCount;
        }
        /// <summary>
        /// 判断是否是方阵
        /// </summary>
        /// <returns></returns>
        public bool IsSquare()
        {
            return this.colCount == this.rowCount;
        }
        /// <summary>
        /// 忽略RoundOff错误
        /// </summary>
        public void IgnoreRoundOffError()
        {
            for (int curRow = 0; curRow < rowCount; curRow++)
            {
                for (int curCol = 0; curCol < colCount; curCol++)
                {
                    if (Math.Abs(storage[curRow][curCol]) < 0.0000001)
                    {
                        storage[curRow][curCol] = 0;
                    }
                }
            }
        }
        /// <summary>
        /// 将指定两行互换位置
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        public bool RowInterchange(int row1, int row2)
        {
            if (row1 != row2)
            {
                double temp = 0;
                for (int i = 0; i < colCount; i++)
                {
                    temp = storage[row1][i];
                    storage[row1][i] = storage[row2][i];
                    storage[row2][i] = temp;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 将指定两列互换位置
        /// </summary>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        public void ColumnInterchange(int col1, int col2)
        {
            if (col1 != col2)
            {
                double temp = 0;
                for (int curEntry = 0; curEntry < rowCount; curEntry++)
                {
                    temp = storage[curEntry][col1];
                    storage[curEntry][col1] = storage[curEntry][col2];
                    storage[curEntry][col2] = temp;
                }
            }
        }
        /// <summary>
        /// 将指定行的n倍加到另一指定行上
        /// </summary>
        /// <param name="fromRow"></param>
        /// <param name="toRow"></param>
        /// <param name="time"></param>
        public void AddTimesOfRowToRow(int fromRow, int toRow, double time)
        {
            if (fromRow != toRow)
            {
                for (int curCol = 0; curCol < colCount; curCol++)
                {
                    storage[toRow][curCol] += time * storage[fromRow][curCol];
                }
            }
        }
        /// <summary>
        /// 将指定列的n倍加到另一指定列上
        /// </summary>
        /// <param name="fromCol"></param>
        /// <param name="toCol"></param>
        /// <param name="time"></param>
        public void AddTimesOfColToCol(int fromCol, int toCol, double time)
        {
            if (fromCol != toCol)
            {
                for (int curRow = 0; curRow < rowCount; curRow++)
                {
                    storage[curRow][toCol] += time * storage[curRow][fromCol];
                }
            }
        }
        /// <summary>
        /// 将指定行的n倍加到另一指定行上，使指定项为0
        /// </summary>
        /// <param name="targetRow"></param>
        /// <param name="targetCol"></param>
        /// <param name="fromRow"></param>
        public void EliminateEntryByAddMutipliedRow(int targetRow, int targetCol, int fromRow)
        {
            if (storage[targetRow][targetCol] != 0)
            {
                double multiplier = storage[targetRow][targetCol] / storage[fromRow][targetCol];
                for (int curCol = 0; curCol < colCount; curCol++)
                {
                    storage[targetRow][curCol] -= storage[fromRow][curCol] * multiplier;
                }
            }
        }
        /// <summary>
        /// 全部项缩放n倍
        /// </summary>
        /// <param name="scalar"></param>
        public void OverallScaling(double scalar)
        {
            for (int curRow = 0; curRow < rowCount; curRow++)
            {
                for (int curCol = 0; curCol < colCount; curCol++)
                {
                    storage[curRow][curCol] *= scalar;
                }
            }
        }
        /// <summary>
        /// 将指定行缩放至其n倍
        /// </summary>
        /// <param name="row"></param>
        /// <param name="time"></param>
        public void RowScaling(int row, double factor)
        {
            for (int curCol = 0; curCol < colCount; curCol++)
            {
                storage[row][curCol] *= factor;
            }
        }
        /// <summary>
        /// 将指定列缩放至其n倍
        /// </summary>
        /// <param name="col"></param>
        /// <param name="factor"></param>
        public void ColumnScaling(int col, double factor)
        {
            for (int curRow = 0; curRow < rowCount; curRow++)
            {
                storage[curRow][col] *= factor;
            }
        }
        /// <summary>
        /// 将第一列中最大元素所在行与第一行互换位置
        /// </summary>
        /// <param name="mat"></param>
        public void MoveLargeRowToTop() { }
        /// <summary>
        /// 返回某列最大值和其对应行号
        /// </summary>
        /// <param name="col"></param>
        /// <param name="abs"></param>
        /// <returns></returns>
        public double[] FindLargestEntryInCol(int col, bool abs)
        {
            int largestIndex = -1;
            double largestValue;
            if (abs)
            {
                largestValue = 0;
                for (int curRow = 0; curRow < colCount; curRow++)
                {
                    if (Math.Abs(storage[curRow][col]) > Math.Abs(largestValue))
                    {
                        largestValue = storage[curRow][col];
                        largestIndex = curRow;
                    }
                }
            }
            else
            {
                largestValue = double.MinValue;
                for (int curRow = 0; curRow < colCount; curRow++)
                {
                    if (storage[curRow][col] > largestValue)
                    {
                        largestValue = storage[curRow][col];
                        largestIndex = curRow;
                    }
                }
            }
            if (largestIndex == -1)
            {
                throw new CustomException.NoneZeroEntryNotFoundException();
            }
            return new double[2] { largestIndex, largestValue };
        }
        /// <summary>
        /// 找到指定行左数第一个非零元素位置
        /// </summary>
        /// <param name="row"></param>
        /// <returns>未找到则为-1</returns>
        public int FindFirstNoneZeroOfRow(int row)
        {
            for (int curCol = 0; curCol < colCount; curCol++)
            {
                if (storage[row][curCol] != 0)
                {
                    return curCol;
                }
            }
            return -1;
        }
        /// <summary>
        /// 行阶梯化
        /// </summary>
        /// <param name="reduced"></param>
        /// <returns>行互换次数为奇</returns>
        public bool RowEchelonize(bool reduced)
        {
            this.rank = rowCount <= colCount ? rowCount : colCount;
            bool pnReverse = false;
            #region Forward
            int biggestIndex = 0;
            double biggestNum = 0;
            for (int curRow = 0; curRow < rowCount; curRow++)
            {
                double curAbs = Math.Abs(storage[curRow][0]);
                if (curAbs > biggestNum)
                {
                    biggestNum = curAbs;
                    biggestIndex = curRow;
                }
            }
            if (RowInterchange(0, biggestIndex))
            {
                pnReverse = !pnReverse;
            }
            int rankLose = 0;
            for (int curOperRow = 0; curOperRow < rowCount - 1 - rankLose; curOperRow++)
            {
                while (storage[curOperRow][curOperRow + rankLose] == 0)
                {
                    for (int cycleTime = 0; cycleTime < rowCount - curOperRow; cycleTime++)
                    {
                        for (int curRowPosition = curOperRow; curRowPosition < rowCount - 1; curRowPosition++)
                        {
                            if (RowInterchange(curRowPosition, curRowPosition + 1))
                            {
                                pnReverse = !pnReverse;
                            }
                        }
                        if (storage[curOperRow][curOperRow + rankLose] != 0)
                        {
                            goto pivotLocated;
                        }
                    }
                    rankLose++;
                    if (rankLose + curOperRow == MaxRank)
                    {
                        this.rank -= rankLose;
                        goto Exit;
                    }
                }
                pivotLocated : for (int curOperedRow = curOperRow + 1; curOperedRow < rowCount; curOperedRow++)
                {
                    this.EliminateEntryByAddMutipliedRow(curOperedRow, curOperRow + rankLose, curOperRow);
                }
            }
            this.rank -= rankLose;
            #endregion
            #region Backward
            if (reduced)
            {
                for (int curOperRow = this.rank - 1; curOperRow >= 0; curOperRow--)
                {
                    int pivotCol = 0;
                    for (int curCol = 0; curCol < colCount; curCol++)
                    {
                        if (storage[curOperRow][curCol] != 0)
                        {
                            pivotCol = curCol;
                            break;
                        }
                    }
                    RowScaling(curOperRow, 1 / storage[curOperRow][pivotCol]);
                    for (int curOperedRow = 0; curOperedRow < curOperRow; curOperedRow++)
                    {
                        EliminateEntryByAddMutipliedRow(curOperedRow, pivotCol, curOperRow);
                    }
                }
            }
            #endregion
            Exit : IgnoreRoundOffError();
            return pnReverse;
        }
        public Matrix SolveLinearEquation(Matrix b)
        {
            throw new NotImplementedException();
        }
    }
    public class Matrix : Array2
    {
        public Matrix(double[][] mat) : base(mat) { }
        public Matrix(int row, int col) : base(row, col) { }
        public Matrix(int row, int col, params double[] entries) : base(row, col, entries) { }
        #region 运算符
        public static Matrix operator +(Matrix mat1, Matrix mat2)
        {
            if (!mat1.OfSameSize(mat2))
            {
                throw new CustomException.MatrixOperationNotDefinedException();
            }
            Matrix result = new Matrix(mat1.rowCount, mat1.colCount);
            for (int i = 0; i < mat1.rowCount; i++)
            {
                for (int j = 0; j < mat1.colCount; j++)
                {
                    result.storage[i][j] = mat1.storage[i][j] + mat2.storage[i][j];
                }
            }
            return result;
        }
        public static Matrix operator -(Matrix mat1, Matrix mat2)
        {
            if (!mat1.OfSameSize(mat2))
            {
                throw new CustomException.MatrixOperationNotDefinedException();
            }
            Matrix result = new Matrix(mat1.rowCount, mat1.colCount);
            for (int i = 0; i < mat1.rowCount; i++)
            {
                for (int j = 0; j < mat1.colCount; j++)
                {
                    result.storage[i][j] = mat1.storage[i][j] - mat2.storage[i][j];
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix mat1, Matrix mat2)
        {
            if (mat1.colCount != mat2.rowCount)
            {
                throw new CustomException.MatrixOperationNotDefinedException();
            }
            Matrix result = new Matrix(mat1.rowCount, mat2.colCount);
            for (int curCol = 0; curCol < mat2.colCount; curCol++)
            {
                for (int curRow = 0; curRow < mat1.rowCount; curRow++)
                {
                    double sum = 0;
                    for (int curEntry = 0; curEntry < mat1.colCount; curEntry++)
                    {
                        sum += mat1.storage[curRow][curEntry] * mat2.storage[curEntry][curCol];
                    }
                    result.storage[curRow][curCol] = sum;
                }
            }
            result.IgnoreRoundOffError();
            return result;
        }
        public static Matrix operator *(Matrix mat, double scalar)
        {
            Matrix temp = mat.Clone();
            temp.OverallScaling(scalar);
            return temp;
        }
        public static Matrix operator *(double scalar, Matrix mat)
        {
            Matrix temp = mat.Clone();
            temp.OverallScaling(scalar);
            return temp;
        }
        public static bool operator ==(Matrix mat1, Matrix mat2)
        {
            if (!mat1.OfSameSize(mat2))
            {
                return false;
            }
            for (int curRow = 0; curRow < mat1.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < mat1.colCount; curCol++)
                {
                    if (mat1[curRow][curCol] != mat2[curRow][curCol])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool operator !=(Matrix mat1, Matrix mat2)
        {
            if (!mat1.OfSameSize(mat2))
            {
                return true;
            }
            for (int curRow = 0; curRow < mat1.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < mat1.colCount; curCol++)
                {
                    if (mat1[curRow][curCol] != mat2[curRow][curCol])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 行对齐矩阵合并运算符
        /// </summary>
        /// <param name="leftMat"></param>
        /// <param name="rightMat"></param>
        /// <returns></returns>
        public static Matrix operator |(Matrix leftMat, Matrix rightMat)
        {
            Matrix result = new Matrix(leftMat.rowCount, leftMat.colCount + rightMat.colCount);
            for (int curRow = 0; curRow < leftMat.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < leftMat.colCount + rightMat.colCount; curCol++)
                {
                    if (curCol < leftMat.colCount)
                    {
                        result[curRow][curCol] = leftMat.storage[curRow][curCol];
                    }
                    if (curCol >= leftMat.colCount)
                    {
                        result[curRow][curCol] = rightMat[curRow][curCol - leftMat.colCount];
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 逆矩阵运算符
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix operator ~(Matrix mat)
        {
            if (mat.colCount != mat.rowCount)
            {
                throw new CustomException.MatrixNotInvertibleException();
            }
            Matrix matMerge = mat | Matrix.DiagonalMatrix(mat.rowCount, true, null);
            matMerge.RowEchelonize(true);
            Matrix[] leftRightMat = SplitBySpecifiedColumn(matMerge, mat.rowCount);
            if (leftRightMat[0].FindFirstNoneZeroOfRow(mat.rowCount - 1) == -1)
            {
                throw new CustomException.MatrixNotInvertibleException();
            }
            else
            {
                return leftRightMat[1];
            }
        }
        /// <summary>
        /// 转置矩阵运算符
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix operator !(Matrix mat)
        {
            Matrix transpose = new Matrix(mat.colCount, mat.colCount);
            for (int curRow = 0; curRow < mat.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < mat.colCount; curCol++)
                {
                    transpose[curCol][curRow] = mat[curRow][curCol];
                }
            }
            return transpose;
        }
        #endregion
        /// <summary>
        /// 返回于指定列分裂的矩阵
        /// </summary>
        /// <param name="leftMatColCount"></param>
        /// <returns></returns>
        public static Matrix[] SplitBySpecifiedColumn(Matrix mat, int leftMatColCount)
        {
            Matrix leftMat = new Matrix(mat.rowCount, leftMatColCount),
                rightMat = new Matrix(mat.rowCount, mat.colCount - leftMatColCount);
            for (int curRow = 0; curRow < mat.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < mat.colCount; curCol++)
                {
                    if (curCol < leftMat.colCount)
                    {
                        leftMat.storage[curRow][curCol] = mat.storage[curRow][curCol];
                    }
                    else
                    {
                        rightMat.storage[curRow][curCol - leftMat.colCount] = mat.storage[curRow][curCol];
                    }
                }
            }
            return new Matrix[2] { leftMat, rightMat };
        }
        /// <summary>
        /// 返回可自定义的对角矩阵
        /// </summary>
        /// <param name="size"></param>
        /// <param name="identity"></param>
        /// <param name="diagEntries"></param>
        /// <returns></returns>
        public static Matrix DiagonalMatrix(int size, bool identity, params double[] diagEntries)
        {
            Matrix arr = new Matrix(size, size);
            for (int curRow = 0; curRow < size; curRow++)
            {
                for (int curCol = 0; curCol < size; curCol++)
                {
                    if (identity)
                    {
                        arr.storage[curRow][curCol] = curRow == curCol ? 1 : 0;
                    }
                    else
                    {
                        arr.storage[curRow][curCol] = curRow == curCol ? diagEntries[curRow] : 0;
                    }
                }
            }
            return arr;
        }
        /// <summary>
        /// 返回可自定义的列向量
        /// </summary>
        /// <param name="size"></param>
        /// <param name="identicalEntries"></param>
        /// <param name="entries"></param>
        /// <returns></returns>
        public static Matrix ColumnVector(int size, bool identicalEntries, params double[] entries)
        {
            double[][] tempStorage = new double[size][];
            for (int curRow = 0; curRow < size; curRow++)
            {
                tempStorage[curRow] = new double[1] { identicalEntries ? 1 : entries[curRow] };
            }
            return new Matrix(tempStorage);
        }
        /// <summary>
        /// 返回矩阵的行列式
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Determinant GetDeterminant(Matrix mat)
        {
            return new Determinant(mat.storage);
        }
        /// <summary>
        /// 返回矩阵副本
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public Matrix Clone()
        {
            Matrix clone = new Matrix(this.rowCount, this.colCount);
            for (int curRow = 0; curRow < this.rowCount; curRow++)
            {
                for (int curCol = 0; curCol < this.colCount; curCol++)
                {
                    clone[curRow][curCol] = this [curRow][curCol];
                }
            }
            return clone;
        }
        /// <summary>
        /// 指数迭代法解特征值
        /// </summary>
        /// <returns></returns>
        public object[] PowerMethodEigenSolver()
        {
            if (!IsSquare())
            {
                throw new CustomException.MatrixNotSquareException();
            }
            Matrix xOfCurIteration = Matrix.ColumnVector(this.rowCount, true);
            double curLamda = double.MinValue;
            for (int iterationNum = 0;; iterationNum++)
            {
                xOfCurIteration = this * xOfCurIteration;
                curLamda = xOfCurIteration.FindLargestEntryInCol(0, true)[1];
                if (this * xOfCurIteration == curLamda * xOfCurIteration)
                {
                    break;
                }
                xOfCurIteration *= (1 / curLamda);
            }
            return new object[2] { curLamda, xOfCurIteration };
        }
    }
    public class Determinant : Array2
    {
        public Determinant(double[][] mat) : base(mat) { }
        public Determinant(int row, int col) : base(row, col) { }
        public Determinant(int row, int col, params double[] entries) : base(row, col, entries) { }
        #region 运算符
        public static double operator +(Determinant deter1, Determinant deter2)
        {
            return deter1.Value() + deter2.Value();
        }
        public static double operator -(Determinant deter1, Determinant deter2)
        {
            return deter1.Value() - deter2.Value();
        }
        public static double operator *(Determinant deter1, Determinant deter2)
        {
            return deter1.Value() * deter2.Value();
        }
        public static double operator /(Determinant deter1, Determinant deter2)
        {
            return deter1.Value() / deter2.Value();
        }
        #endregion
        /// <summary>
        /// 返回行列式值
        /// </summary>
        /// <returns></returns>
        public double Value()
        {
            if (rowCount != colCount)
            {
                return 0;
            }
            bool pnReverse = false;
            if (this.rank == -1)
            {
                pnReverse = RowEchelonize(false);
            }
            if (rank < colCount)
            {
                return 0;
            }
            double result = 1;
            for (int curDiagEntry = 0; curDiagEntry < rowCount; curDiagEntry++)
            {
                result *= storage[curDiagEntry][curDiagEntry];
            }
            if (Math.Abs(result) < Settings.ignoreRoundOffThreshold)
            {
                result = 0;
            }
            return result * (pnReverse ? -1 : 1);
        }
    }
}