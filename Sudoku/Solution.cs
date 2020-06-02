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
        private int rows, columns;
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

        private Boolean FillCell(int r, int c, ArrayList cellOptions)
        {
            if (cellOptions.Count < 1)
                return false;

            Random rand = new Random();
            int opsIndex = rand.Next(cellOptions.Count);
            int num = (int)cellOptions[opsIndex];
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

        private int[] GetNextCell(int r, int c)
        {
            int[] nextCell = null;

            if (c == 8)
                nextCell = new int[] { (r + 1), 0 };
            else
                nextCell = new int[] { r, (c + 1) };

            return nextCell;
        }

        private ArrayList GetOptions(int r, int c)
        {
            ArrayList unavailableValues = new ArrayList();
            unavailableValues.AddRange(this.GetRow(r));
            unavailableValues.AddRange(this.GetCol(c));
            unavailableValues.AddRange(this.GetBlock(r, c));

            ArrayList uniqueOptions = new ArrayList();
            for (int d = 1; d < 10; d++)
            {
                if (!unavailableValues.Contains(d))
                    uniqueOptions.Add(d);
            }
            return uniqueOptions;
        }

        private ArrayList GetRow(int r)
        {
            ArrayList rowValues = new ArrayList();
            for (int i = 0; i < columns; i++)
            {
                if (solution[r, i] != 0)
                    rowValues.Add(solution[r, i]);
            }
            return rowValues;
        }

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
