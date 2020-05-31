/* 
 * Genevieve Peters - Code Sample
 * 
 * The following sample is the Game class from a Suduko WPF application
 * written in C# using the .NET environment in Visual Studio.
 * 
 * The Game class creates a valid Sudoku gameboard and adheres to the rules of Sudoku:
 * - Valid numbers = 1,2,3,4,5,6,7,8,9
 * - Valid numbers must only appear once in each row.
 * - Valid numbers must only appear once in each column.
 * - Valid numbers must only appear once in each 3x3 block starting in the top left corner of the board.
 * - Omitted cells must be filled in by the player.
 * - If the gameboard exactly matches the solution, the player wins.
 * 
 */

using System;
using System.Collections;

namespace Sudoku
{
    class Game
    {
        private int[,] solution; //complete gameboard
        private int[,] startBoard; //gameboard with omitted cells

        public Game()
        {
            solution = new int[9, 9];
            startBoard = new int[9, 9];
        }

        /// <summary>
        /// Generates a valid Sudoku board
        /// </summary>
        /// <param name="level">the number of cells to omit</param>
        public void Generate(int level)
        {
            int startRow = 0;
            int startCol = 0;
            bool filled = this.FillCell(startRow, startCol, this.GetOptions(startRow, startCol));
            if (filled)
                startBoard = this.HideCells(solution, level);
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
        private bool FillCell(int r, int c, ArrayList ops)
        {
            if (ops.Count < 1)
                return false;
            
            Random rand = new Random();
            int opsIndex = rand.Next(ops.Count);
            int num = (int)ops[opsIndex];
            solution[r, c] = num;
            if (r == 8 && c == 8)
                return true;
            else
            {
                int nextR = GetNextRow(r, c);
                int nextC = GetNextCol(r, c);
                bool success = FillCell(nextR, nextC, this.GetOptions(nextR, nextC));

                if (success)
                    return true;
                
                ops.RemoveAt(opsIndex);
                solution[r, c] = 0;
                return FillCell(r, c, ops);
            }
        }

        /// <summary>
        /// Returns the row number of the next cell
        /// following the current cell.
        /// </summary>
        /// <param name="r">row of current cell</param>
        /// <param name="c">column of current cell</param>
        /// <returns>row number of next cell</returns>
        private int GetNextRow(int r, int c)
        {
            if (c == 8)
                return r + 1;
            else
                return r;
        }

        /// <summary>
        /// Returns the column number of the next cell
        /// following the current cell.
        /// </summary>
        /// <param name="r">row of current cell</param>
        /// <param name="c">column of current cell</param>
        /// <returns>column number of next cell</returns>
        private int GetNextCol(int r, int c)
        {
            if (c == 8)
                return 0;
            else
                return c + 1;
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

        /// <summary>
        /// Returns a gameboard with omitted cells.
        /// </summary>
        /// <param name="fullBoard">solution for this game</param>
        /// <param name="level">number of cells to omit</param>
        /// <returns>gameboard with omitted cells</returns>
        private int[,] HideCells(int[,] fullBoard, int level)
        {
            int[,] modified = (int[,])fullBoard.Clone();
            ArrayList hideIndex = new ArrayList();
            Random rand = new Random();
            int i = 0;
            while (i < level)
            {
                int cellIndex = rand.Next(81);//0-80
                if (!hideIndex.Contains(cellIndex))
                {
                    hideIndex.Add(cellIndex);
                    i++;
                }
            }

            foreach (int cell in hideIndex)
            {
                int row = cell / 9;
                int col = cell % 9;
                modified[row, col] = 0;
            }

            return modified;
        }

        public int[,] GetStartBoard()
        {
            return startBoard;
        }

        public bool IsSolved(int[,] currentBoard)
        {
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    if (solution[r, c] != currentBoard[r, c])
                        return false;
                }
            }
            return true;
        }
    }
}
