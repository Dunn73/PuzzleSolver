using PuzzleSolver.SudokuActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace PuzzleSolver.EventHandlers
{
    public static class ButtonEventHandlers
    {
        public static void SolveButton_Click(object sender, RoutedEventArgs e, TextBox[,] sudokuCells, char?[,] originalBoardState)
        {
            // Save the current board state
            SaveBoardState(sudokuCells, originalBoardState);

            char[][] board = new char[9][];
            for (int i = 0; i < 9; i++)
            {
                board[i] = new char[9];
                for (int j = 0; j < 9; j++)
                {
                    char num = sudokuCells[i, j].Text.Length > 0 ? sudokuCells[i, j].Text[0] : '.';
                    board[i][j] = num;
                }
            }
            if (!ValidityChecker.IsBoardValid(board))
            {
                MessageBox.Show("Not a valid board");
                return;
            }

            if (Solver.SolveSudoku(board))
            {
                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        sudokuCells[row, col].Text = board[row][col].ToString();
                    }
                }
                MessageBox.Show("Sudoku puzzle solved!");
            }
            else
            {
                MessageBox.Show("No solution exists for this Sudoku puzzle.");
            }
        }

        public static void ClearButton_Click(object sender, RoutedEventArgs e, TextBox[,] sudokuCells)
        {
            foreach (TextBox textBox in sudokuCells)
            {
                textBox.Clear();
            }
        }

        public static void ResetButton_Click(object sender, RoutedEventArgs e, TextBox[,] sudokuCells, char?[,] originalBoardState)
        {
            // Reset the board to the original unsolved state
            RestoreOriginalBoardState(sudokuCells, originalBoardState);
        }

        // Method to save the current board state
        private static void SaveBoardState(TextBox[,] sudokuCells, char?[,] originalBoardState)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    originalBoardState[row, col] = sudokuCells[row, col].Text.Length > 0 ? sudokuCells[row, col].Text[0] : '.';
                }
            }
        }

        // Method to restore the board to the original unsolved state
        private static void RestoreOriginalBoardState(TextBox[,] sudokuCells, char?[,] originalBoardState)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    sudokuCells[row, col].Text = originalBoardState[row, col] == '.' ? "" : originalBoardState[row, col].ToString();
                }
            }
        }
    }
}
