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
    public partial class MainWindow : Window{
        private TextBox[,] sudokuCells = new TextBox[9, 9];
        // Two-dimensional array to store the original board state
        private char?[,] originalBoardState = new char?[9, 9];


        public MainWindow(){
            InitializeComponent();
            InitializeSudokuGrid();
        }
        private void InitializeSudokuGrid(){
            for (int row = 0; row < 9; row++){
                for (int col = 0; col < 9; col++){
                    TextBox textBox = new TextBox{
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
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                }
            }
        }
        private void SolveButton_Click(object sender, RoutedEventArgs e){
            // Save the current board state
            SaveBoardState();

            char[][] board = new char[9][];
            for (int i = 0; i < 9; i++){
                board[i] = new char[9];
                for (int j = 0; j < 9; j++){
                    char num = sudokuCells[i, j].Text.Length > 0 ? sudokuCells[i, j].Text[0] : '.';
                    board[i][j] = num;
                }
            }
            if (!IsBoardValid(board)) {
                MessageBox.Show("Not a valid board");
                return;
            }

            if (SolveSudoku(board)){
                for (int row = 0; row < 9; row++){
                    for (int col = 0; col < 9; col++){
                        sudokuCells[row, col].Text = board[row][col].ToString();
                    }
                }
                MessageBox.Show("Sudoku puzzle solved!");
            }
            else{
                MessageBox.Show("No solution exists for this Sudoku puzzle.");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e){
            foreach (TextBox textBox in sudokuCells){
                textBox.Clear();
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e){
            // Reset the board to the original unsolved state
            RestoreOriginalBoardState();
        }
        // Method to save the current board state
        private void SaveBoardState(){
            for (int row = 0; row < 9; row++){
                for (int col = 0; col < 9; col++){
                    originalBoardState[row, col] = sudokuCells[row, col].Text.Length > 0 ? sudokuCells[row, col].Text[0] : '.';
                }
            }
        }

        // Method to restore the board to the original unsolved state
        private void RestoreOriginalBoardState(){
            for (int row = 0; row < 9; row++){
                for (int col = 0; col < 9; col++){
                    sudokuCells[row, col].Text = originalBoardState[row, col] == '.' ? "" : originalBoardState[row, col].ToString();
                }
            }
        }

        private bool IsBoardValid(char[][] board){
            // Check rows
            for (int row = 0; row < 9; row++){
                if (!IsRowValid(board, row)){
                    return false;
                }
            }

            // Check columns
            for (int col = 0; col < 9; col++){
                if (!IsColumnValid(board, col)){
                    return false;
                }
            }

            // Check 3x3 sub-grids
            for (int row = 0; row < 9; row += 3){
                for (int col = 0; col < 9; col += 3){
                    if (!IsSubgridValid(board, row, col)){
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsRowValid(char[][] board, int row){
            HashSet<char> set = new HashSet<char>();
            for (int col = 0; col < 9; col++){
                char digit = board[row][col];
                if (digit != '.' && !set.Add(digit)){
                    return false; // Duplicate digit found
                }
            }
            return true;
        }

        private bool IsColumnValid(char[][] board, int col){
            HashSet<char> set = new HashSet<char>();
            for (int row = 0; row < 9; row++){
                char digit = board[row][col];
                if (digit != '.' && !set.Add(digit)){
                    return false; // Duplicate digit found
                }
            }
            return true;
        }

        private bool IsSubgridValid(char[][] board, int startRow, int startCol){
            HashSet<char> set = new HashSet<char>();
            for (int row = startRow; row < startRow + 3; row++){
                for (int col = startCol; col < startCol + 3; col++){
                    char digit = board[row][col];
                    if (digit != '.' && !set.Add(digit)){
                        return false; // Duplicate digit found
                    }
                }
            }
            return true;
        }

        public bool SolveSudoku(char[][] board){
            return Solve(board);
        }


        private bool Solve(char[][] board){
            int emptyCellCount = 0;
            (int, int)[] emptyCells = new (int, int)[81];

            // Precompute empty cells and count
            for (int row = 0; row < 9; row++){
                for (int col = 0; col < 9; col++){
                    if (board[row][col] == '.'){
                        emptyCells[emptyCellCount++] = (row, col);
                    }
                }
            }

            Array.Resize(ref emptyCells, emptyCellCount);

            // Check for cells with only one option and fill them in
            for (int i = 0; i < emptyCellCount; i++){
                var (row, col) = emptyCells[i];
                List<char> options = Options(board, row, col);
                if (options.Count == 1){
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

        private bool Backtrack(char[][] board, (int, int)[] emptyCells, int index){
            if (index == emptyCells.Length)
                return true; // Sudoku solved

            var (row, col) = emptyCells[index];

            foreach (char num in Options(board, row, col)){
                board[row][col] = num;
                if (Backtrack(board, emptyCells, index + 1))
                    return true;
            }

            board[row][col] = '.'; // Backtrack
            return false; // No valid number found for this cell
        }

        public List<char> Options(char[][] board, int row, int col){
            List<char> output = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 9; i++){
                output.Remove(board[row][i]);
                output.Remove(board[i][col]);
                output.Remove(board[3 * (row / 3) + i / 3][3 * (col / 3) + i % 3]);
            }
            return output;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e){
            e.Handled = !char.IsDigit(e.Text, 0) || e.Text[0] < '1' || e.Text[0] > '9';
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e){
            var textBox = sender as TextBox;
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
                    if (textBox.Text.Length > 0){
                        int caretIndex = textBox.CaretIndex;
                        textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                        textBox.CaretIndex = caretIndex - 1;
                    }
                    else if (col > 0 || row > 0){
                        if (col > 0){
                            sudokuCells[row, col - 1].Focus();
                            sudokuCells[row, col - 1].CaretIndex = 1; // Move cursor to the rightmost position
                        }
                        else{
                            sudokuCells[row - 1, 8].Focus();
                            sudokuCells[row - 1, 8].CaretIndex = 1; // Move cursor to the rightmost position
                        }
                    }
                    e.Handled = true;
                    break;
            }
        }
    }
}
