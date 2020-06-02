using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Solution
    {
        private const int ROWS = 9;
        private const int COLUMNS = 9;
        private int[,] solution;

        public Solution()
        {
            solution = new int[9, 9];
            this.BuildSolution();
        }

        public void BuildSolution()
        {
            int startRow = 0;
            int startCol = 0;
            bool valid = this.FillCell(startRow, startCol, this.GetOptions(startRow, startCol));
            if (!valid)
                this.BuildSolution();
        }

        public int[,] GetSolution()
        {
            return solution;
        }

        public Boolean IsEqual(int[,] other)
        {
            if (other.Length != solution.Length)
                return false;

            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLUMNS; c++)
                {
                    if (solution[r, c] != other[r, c])
                        return false;
                }
            }
            return true;
        }

        public int[,] CopySolution()
        {
            int[,] copy = new int[ROWS, COLUMNS];
            for (int r = 0; r < ROWS; r++)
            {
                for (int c = 0; c < COLUMNS; c++)
                {
                    copy[r, c] = solution[r, c];
                }
            }

            return copy;
        }

        /// <summary>
        /// Returns true if the gameboard is valid.
        /// Recursive method that assigns random numbers from the list of
        /// optional numbers. If assignment is not valid, disregard number
        /// and choose another value from the options list. If list is empty,
        /// backtrack to the previous cell, choose a new value and repeat.
        /// </summary>
        /// <param name="r">row</param>
        /// <param name="c">column</param>
        /// <param name="ops">options list containing valid numbers for cell[r,c]</param>
        /// <returns>true if the gameboard is valid</returns>
        private Boolean FillCell(int r, int c, ArrayList cellOptions)
        {
            if (cellOptions.Count < 1)
                return false;

            Random rand = new Random();
            int opsIndex = rand.Next(cellOptions.Count);
            int num = (int) cellOptions[opsIndex];
            solution[r, c] = num;

            if (r == 8 && c == 8)
                return true;
            else
            {
                int nextR = this.GetNextCell(r,c)[0];
                int nextC = this.GetNextCell(r, c)[1];
                bool success = FillCell(nextR, nextC, this.GetOptions(nextR, nextC));

                if (success)
                    return true;

                cellOptions.RemoveAt(opsIndex);
                solution[r, c] = 0;
                return FillCell(r, c, cellOptions);
            }
        }

        /// <summary>
        /// Returns the row number of the next cell
        /// following the current cell.
        /// </summary>
        /// <param name="r">row of current cell</param>
        /// <param name="c">column of current cell</param>
        /// <returns>row number of next cell</returns>
        private int[] GetNextCell(int r, int c)
        {
            int[] nextCell = null;

            if (c == 8)
                nextCell = new int[] { (r + 1), 0 };
            else
                nextCell = new int[] { r, (c + 1) };

            return nextCell;
        }

        /// <summary>
        /// Returns a list of valid numbers for cell[row,col]
        /// </summary>
        /// <param name="r">row</param>
        /// <param name="c">column</param>
        /// <returns>list of valid numbers for cell[row,col]</returns>
        private ArrayList GetOptions(int r, int c)
        {
            ArrayList lists = new ArrayList();
            lists.AddRange(this.GetRow(r));
            lists.AddRange(this.GetCol(c));
            lists.AddRange(this.GetBlock(r, c));

            ArrayList options = new ArrayList();
            for (int d = 1; d < 10; d++)
            {
                if (!lists.Contains(d))
                    options.Add(d);
            }
            return options;
        }

        /// <summary>
        /// Returns a list of numbers currently in row r.
        /// </summary>
        /// <param name="r">row</param>
        /// <returns>list of numbers currently in row r</returns>
        private ArrayList GetRow(int r)
        {
            ArrayList row = new ArrayList();
            for (int i = 0; i < 9; i++)
            {
                if (solution[r, i] != 0)
                    row.Add(solution[r, i]);
            }
            return row;
        }

        /// <summary>
        /// Returns a list of numbers currently in column c
        /// </summary>
        /// <param name="c">column</param>
        /// <returns>list of numbers currently in column c</returns>
        private ArrayList GetCol(int c)
        {
            ArrayList col = new ArrayList();
            for (int i = 0; i < 9; i++)
            {
                if (solution[i, c] != 0)
                    col.Add(solution[i, c]);
            }
            return col;
        }

        /// <summary>
        /// Returns a list of numbers currently in the 3x3
        /// block where cell[r,c] resides
        /// </summary>
        /// <param name="r">row</param>
        /// <param name="c">column</param>
        /// <returns>list of numbers currently in block</returns>
        private ArrayList GetBlock(int r, int c)
        {
            int blockR = (r / 3) * 3;
            int blockC = (c / 3) * 3;

            ArrayList block = new ArrayList();
            for (int i = blockR; i < blockR + 3; i++)
            {
                for (int j = blockC; j < blockC + 3; j++)
                {
                    block.Add(solution[i, j]);
                }
            }
            return block;
        }
    }
}