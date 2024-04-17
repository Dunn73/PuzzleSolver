using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PuzzleSolver{
    public static class TextBoxEventHandlers{
        public static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            e.Handled = !char.IsDigit(e.Text, 0) || e.Text[0] < '1' || e.Text[0] > '9';
        }
        public static void TextBox_PreviewKeyDown(object sender, KeyEventArgs e, TextBox[,] sudokuCells){

            TextBox textBox = sender as TextBox;
            int row = Grid.GetRow(textBox);
            int col = Grid.GetColumn(textBox);

            switch (e.Key){
                case Key.Left:
                    if (col > 0){
                        sudokuCells[row, col - 1].Focus();        // New cell to focus on
                        sudokuCells[row, col - 1].CaretIndex = 1; // Move cursor to the rightmost position
                    }
                    e.Handled = true;
                    break;
                case Key.Right:
                    if (col < 8){
                        sudokuCells[row, col + 1].Focus();
                        sudokuCells[row, col + 1].CaretIndex = 1;
                    }
                    e.Handled = true;
                    break;
                case Key.Up:
                    if (row > 0){
                        sudokuCells[row - 1, col].Focus();
                        sudokuCells[row - 1, col].CaretIndex = 1;
                    }
                    e.Handled = true;
                    break;
                case Key.Down:
                    if (row < 8){
                        sudokuCells[row + 1, col].Focus();
                        sudokuCells[row + 1, col].CaretIndex = 1;
                    }
                    e.Handled = true;
                    break;

                case Key.Enter:
                    if (col < 8 || row < 8){
                        if (col < 8)
                            sudokuCells[row, col + 1].Focus();
                        else
                            sudokuCells[row + 1, 0].Focus();
                    }
                    e.Handled = true;
                    break;
                case Key.Back:
                    if (textBox.Text.Length > 0 && textBox.CaretIndex != 0){
                        int caretIndex = textBox.CaretIndex;
                        textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                        textBox.CaretIndex = caretIndex - 1;
                    }
                    else if (col > 0 || row > 0){
                        if (col > 0){
                            sudokuCells[row, col - 1].Focus();
                            sudokuCells[row, col - 1].CaretIndex = 1;
                        }
                        else{
                            sudokuCells[row - 1, 8].Focus();
                            sudokuCells[row - 1, 8].CaretIndex = 1;
                        }
                    }
                    e.Handled = true;
                    break;
            }
        }
    }
}
