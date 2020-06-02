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
        public enum Difficulty { EASY, MEDIUM, HARD}
        private const int levelEasy = 30;
        private const int levelMedium = 40;
        private const int levelHard = 50;
        private const int rows = 9;
        private const int columns = 9;
        private int levelGame;
        private Solution solution; //complete gameboard
        private int[,] startBoard; //gameboard with omitted cells


        public Game()
        {
            solution = new Solution(rows, columns);
            startBoard = null;
        }

        public void StartGame(Difficulty d)
        {
            levelGame = this.GetLevel(d);

            startBoard = solution.CopySolution();
            ArrayList hideIndex = new ArrayList();
            Random rand = new Random();
            int i = 0;
            while (i < levelGame)
            {
                int cellIndex = rand.Next(rows*columns);//0-80
                if (!hideIndex.Contains(cellIndex))
                {
                    hideIndex.Add(cellIndex);
                    i++;
                }
            }

            foreach (int cell in hideIndex)
            {
                int row = cell / rows;
                int col = cell % columns;
                startBoard[row, col] = 0;
            }
        }

        private int GetLevel(Difficulty d)
        {
            if (d == Difficulty.HARD)
                return levelHard;
            else if (d == Difficulty.MEDIUM)
                return levelMedium;
            else
                return levelEasy;
        }

        public int[,] GetStartBoard()
        {
            return startBoard;
        }

        public bool? CheckProgress(int[,] currentBoard)
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (currentBoard[r,c] == 0)
                    {
                        return null;
                    }
                }
            }
            return solution.IsEqual(currentBoard);
        }
    }
}
