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
        private Solution solution; //complete gameboard
        private int[,] startBoard; //gameboard with omitted cells

        public Game()
        {
            solution = new Solution();
            startBoard = null;
        }

        /// <summary>
        /// Returns a gameboard with omitted cells.
        /// </summary>
        /// <param name="fullBoard">solution for this game</param>
        /// <param name="level">number of cells to omit</param>
        /// <returns>gameboard with omitted cells</returns>
        public void StartGame(int level)
        {
            startBoard = solution.CopySolution();
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
                startBoard[row, col] = 0;
            }
        }

        public int[,] GetStartBoard()
        {
            return startBoard;
        }

        public Boolean IsWinner(int[,] currentBoard)
        {
            return solution.IsEqual(currentBoard);
        }
    }
}
