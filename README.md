# WPF Application PuzzleSolver

Sudoku puzzle solver using backtracking to find the first possible solution for the given input.

## Solve
- Using given user input, solves the sudoku if there is a possible solution.<br />
- If the user inputs an invalid board, a message will state that the board is invalid.<br />
- If there is no solution, a message will state that there is no solution.

## Unsolve
- Returns the board to the previous state as it was before the user hit the solve or clear button.

## Clear
- Deletes all elements from the board to create an empty puzzle.

## Allowable Inputs
- Only numbers 1-9 will be accepted as valid inputs in each cell of the puzzle grid.<br />
- The arrow keys can be used to traverse the grid.<br />
- Backspace will either delete the number in the current cell if one exists, or move backwards one cell.<br />
- Enter will move forwards one cell.

## Notes
- The cursor will always be positioned to the right of a number in a cell when traversing the grid with keys to make deleting a number faster.
