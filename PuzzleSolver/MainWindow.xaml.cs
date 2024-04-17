using PuzzleSolver.SudokuActions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PuzzleSolver{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private TextBox[,] sudokuCells = new TextBox[9, 9];

        // Two-dimensional array to store the original board state
        private char?[,] originalBoardState = new char?[9, 9];

        private void SolveButton_Click(object sender, RoutedEventArgs e){
            EventHandlers.ButtonEventHandlers.SolveButton_Click(sender, e, sudokuCells, originalBoardState);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e){
            EventHandlers.ButtonEventHandlers.ClearButton_Click(sender, e, sudokuCells);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e){
            EventHandlers.ButtonEventHandlers.ResetButton_Click(sender, e, sudokuCells, originalBoardState);
        }


        public MainWindow() {
            InitializeComponent();
            InitializeSudokuGrid();
        }
        private void InitializeSudokuGrid() {
            for (int row = 0; row < 9; row++) {
                for (int col = 0; col < 9; col++) {
                    TextBox textBox = new TextBox {
                        Width = 30,
                        Height = 30,
                        FontSize = 16,
                        TextAlignment = TextAlignment.Center,
                        MaxLength = 1
                    };
                    Grid.SetRow(textBox, row);
                    Grid.SetColumn(textBox, col);
                    sudokuCells[row, col] = textBox;
                    SudokuGrid.Children.Add(textBox);
                    textBox.PreviewTextInput += TextBoxEventHandlers.TextBox_PreviewTextInput;
                    textBox.PreviewKeyDown += (sender, e) =>TextBoxEventHandlers.TextBox_PreviewKeyDown(sender, e, sudokuCells);
                }
            }
        }
    }
}
