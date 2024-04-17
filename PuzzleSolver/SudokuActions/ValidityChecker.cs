using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleSolver.SudokuActions
{
    public class ValidityChecker
    {
        public static bool IsBoardValid(char[][] board)
        {
            // Check rows
            for (int row = 0; row < 9; row++)
            {
                if (!IsRowValid(board, row))
                {
                    return false;
                }
            }

            // Check columns
            for (int col = 0; col < 9; col++)
            {
                if (!IsColumnValid(board, col))
                {
                    return false;
                }
            }

            // Check 3x3 sub-grids
            for (int row = 0; row < 9; row += 3)
            {
                for (int col = 0; col < 9; col += 3)
                {
                    if (!IsSubgridValid(board, row, col))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        private static bool IsRowValid(char[][] board, int row)
        {
            HashSet<char> set = new HashSet<char>();
            for (int col = 0; col < 9; col++)
            {
                char digit = board[row][col];
                if (digit != '.' && !set.Add(digit))
                {
                    return false; // Duplicate digit found
                }
            }
            return true;
        }

        private static bool IsColumnValid(char[][] board, int col)
        {
            HashSet<char> set = new HashSet<char>();
            for (int row = 0; row < 9; row++)
            {
                char digit = board[row][col];
                if (digit != '.' && !set.Add(digit))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsSubgridValid(char[][] board, int startRow, int startCol)
        {
            HashSet<char> set = new HashSet<char>();
            for (int row = startRow; row < startRow + 3; row++)
            {
                for (int col = startCol; col < startCol + 3; col++)
                {
                    char digit = board[row][col];
                    if (digit != '.' && !set.Add(digit))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
