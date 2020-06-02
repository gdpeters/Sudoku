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
        private int rows;
        private int columns;
        private int[,] solution;

        public Solution(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            solution = new int[rows, columns];
            this.BuildSolution();
        }

        private void BuildSolution()
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

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (solution[r, c] != other[r, c])
                        return false;
                }
            }
            return true;
        }

        public int[,] CopySolution()
        {
            int[,] copy = new int[rows, columns];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    copy[r, c] = solution[r, c];
                }
            }

            return copy;
        }

        /// <summary>
        /// Returns true if the game is valid under Sudoku rules.
        /// Recursive method assigns random integers from the list of
        /// optional values. If assignment is not valid, remove number from options list
        /// and choose another value from the options list. If list is empty,
        /// backtrack to the previous cell, choose a new value and repeat.
        /// </summary>
        /// <param name="r">cell row</param>
        /// <param name="c">cell column</param>
        /// <param name="cellOptions">options list containing valid numbers for cell[r,c]</param>
        /// <returns>true if the gameboard is valid</returns>
        private Boolean FillCell(int r, int c, ArrayList<Integer> cellOptions)
        {
            if (cellOptions.Count < 1)
                return false;

            Random rand = new Random();
            int opsIndex = rand.Next(cellOptions.Count);
            int num = cellOptions[opsIndex];
            solution[r, c] = num;

            if (r == 8 && c == 8)
                return true;
            else
            {
                int nextR = this.GetNextCell(r,c)[0];
                int nextC = this.GetNextCell(r,c)[1];
                bool success = this.FillCell(nextR, nextC, this.GetOptions(nextR, nextC));

                if (success)
                    return true;
        
                cellOptions.RemoveAt(opsIndex);
                solution[r, c] = 0;
                return this.FillCell(r, c, cellOptions);
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
        private ArrayList<Integer> GetOptions(int r, int c)
        {
            ArrayList unavailableValues = new ArrayList<Integer>();
            unavailableValues.AddRange(this.GetRow(r));
            unavailableValues.AddRange(this.GetCol(c));
            unavailableValues.AddRange(this.GetBlock(r, c));

            ArrayList uniqueOptions = new ArrayList<Integer>();
            for (int d = 1; d < 10; d++)
            {
                if (!unavailableValues.Contains(d))
                    uniqueOptions.Add(d);
            }
            return uniqueOptions;
        }

        /// <summary>
        /// Returns a list of numbers currently in row r.
        /// </summary>
        /// <param name="r">row</param>
        /// <returns>list of numbers currently in row r</returns>
        private ArrayList<Integer> GetRow(int r)
        {
            ArrayList rowValues = new ArrayList<Integer>();
            for (int i = 0; i < columns; i++)
            {
                if (solution[r, i] != 0)
                    rowValues.Add(solution[r, i]);
            }
            return rowValues;
        }

        /// <summary>
        /// Returns a list of numbers currently in column c
        /// </summary>
        /// <param name="c">column</param>
        /// <returns>list of numbers currently in column c</returns>
        private ArrayList GetCol(int c)
        {
            ArrayList columnValues = new ArrayList();
            for (int i = 0; i < 9; i++)
            {
                if (solution[i, c] != 0)
                    columnValues.Add(solution[i, c]);
            }
            return columnValues;
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

            ArrayList blockValues = new ArrayList();
            for (int i = blockR; i < blockR + 3; i++)
            {
                for (int j = blockC; j < blockC + 3; j++)
                {
                    blockValues.Add(solution[i, j]);
                }
            }
            return blockValues;
        }
    }
}
