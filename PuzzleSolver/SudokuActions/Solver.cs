using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver.SudokuActions
{
    public class Solver
    {
        public static bool SolveSudoku(char[][] board)
        {
            return Solve(board);
        }
        public static bool Solve(char[][] board)
        {
            int emptyCellCount = 0;
            (int, int)[] emptyCells = new (int, int)[81];

            // Precompute empty cells and count
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row][col] == '.')
                    {
                        emptyCells[emptyCellCount++] = (row, col);
                    }
                }
            }

            Array.Resize(ref emptyCells, emptyCellCount);

            // Check for cells with only one option and fill them in
            for (int i = 0; i < emptyCellCount; i++)
            {
                var (row, col) = emptyCells[i];
                List<char> options = Options(board, row, col);
                if (options.Count == 1)
                {
                    board[row][col] = options[0];
                    emptyCellCount--; // decrement count 
                    emptyCells[i] = emptyCells[emptyCellCount]; // move the last empty cell to the current position
                    i--; // recheck current position
                }
            }

            Array.Resize(ref emptyCells, emptyCellCount);

            // Sort empty cells by number of options
            Array.Sort(emptyCells, (a, b) => Options(board, a.Item1, a.Item2).Count.CompareTo(Options(board, b.Item1, b.Item2).Count));

            return Backtrack(board, emptyCells, 0);
        }

        private static bool Backtrack(char[][] board, (int, int)[] emptyCells, int index)
        {
            if (index == emptyCells.Length)
                return true; // Sudoku solved

            var (row, col) = emptyCells[index];

            foreach (char num in Options(board, row, col))
            {
                board[row][col] = num;
                if (Backtrack(board, emptyCells, index + 1))
                    return true;
            }

            board[row][col] = '.'; // Backtrack
            return false; // No valid number found for this cell
        }

        private static List<char> Options(char[][] board, int row, int col)
        {
            List<char> output = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 9; i++)
            {
                output.Remove(board[row][i]);
                output.Remove(board[i][col]);
                output.Remove(board[3 * (row / 3) + i / 3][3 * (col / 3) + i % 3]);
            }
            return output;
        }
    }
}
