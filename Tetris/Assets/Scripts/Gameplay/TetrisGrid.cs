using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tetris.Gameplay
{
    public class TetrisGrid : MonoBehaviour
    {
        public delegate void OnEndGame();
        public OnEndGame endGameDelegate;

        public const int gridWidth = 10;
        public const int gridHeight = 21;

        public Transform tetrominosParent;

        // Data that represents where squares are currently placed on the grid
        // NOTE: Tetromino that is currently falling down is not shown in the grid
        // NOTE: Grids data is inversed from the actual visual grid representation
        private bool[,] grid;

        private void Start()
        {
            // Initialize grid with "false" values (empty grid)
            grid = new bool[gridWidth, gridHeight];
            for (int row = 0; row < gridWidth; ++row)
            {
                for (int col = 0; col < gridHeight; ++col)
                {
                    grid[row, col] = false;
                }
            }
        }

        private void Update()
        {
            // Iterate over every tetromino
            foreach (Transform child in tetrominosParent)
            {
                // Destroy tetromino if all of his squares were destroyed
                if (child.GetComponent<Tetromino>().squares.All(square => square == null))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summary>
        /// Get grid value for given row and column.
        /// NOTE: Grids data is inversed from the actual visual grid representation.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public bool BlockTaken(int row, int column)
        {
            return grid[column, row];
        }

        /// <summary>
        /// Tetromino is added to grid once he has finished moving.
        /// </summary>
        /// <param name="tetromino"></param>
        public void AddToGrid(Tetromino tetromino)
        {
            // Set grid values to true where tetromino has landed
            foreach (Transform square in tetromino.squares)
            {
                grid[Mathf.RoundToInt(square.transform.position.x), Mathf.RoundToInt(square.transform.position.y)] = true;
            }
            // Check if there are any rows to be cleared
            bool[] clearRows = CheckClearRows();
            if (clearRows.Any(shouldClear => shouldClear))
            {
                // Clear the rows
                ClearRows(clearRows);
            }
            // Check if the game has ended
            if (CheckEndGame())
                endGameDelegate();
        }

        /// <summary>
        /// Checks if the game has ended by checking the last row of grid for positive value
        /// </summary>
        /// <returns></returns>
        private bool CheckEndGame()
        {
            for (int column = 0; column < gridWidth; ++column)
            {
                if (grid[column, gridHeight - 1])
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if there are filled rows.
        /// </summary>
        /// <returns></returns>
        private bool[] CheckClearRows()
        {
            bool[] clearRows = new bool[gridHeight];

            // Check which rows should be cleared (from top to bottom)
            for (int row = gridHeight - 1; row >= 0; --row)
            {
                int column;
                for (column = 0; column < gridWidth; ++column)
                {
                    if (!grid[column, row])
                        break;
                }

                // If iteration through this whole row went without a break, then this row should be cleared
                if (column == gridWidth)
                    clearRows[row] = true;
                else
                    clearRows[row] = false;
            }

            return clearRows;
        }

        /// <summary>
        /// Remove rows that are filled and move those that are above cleared ones.
        /// </summary>
        /// <param name="clearRows"></param>
        private void ClearRows(bool[] clearRows)
        {
            // Clear every row that needs to be cleared
            for (int row = gridHeight - 1; row >= 0; --row)
            {
                if (clearRows[row])
                {
                    // Remove squares from this row
                    foreach (Transform square in GetSquaresForRow(row))
                    {
                        DestroyImmediate(square.gameObject);
                    }
                    // Update the grid for this row (whole row should be false due to the fact that it's cleared)
                    for (int column = 0; column < gridWidth; ++column)
                        grid[column, row] = false;
                    // Make everything above this row fall down
                    RowsFall(row + 1);
                }
            }
        }

        /// <summary>
        /// Updates the grid in a way that every row from startingRow till the last one is moved by one position down.
        /// </summary>
        /// <param name="startingRow"></param>
        private void RowsFall(int startingRow)
        {
            for (int row = startingRow; row < gridHeight; ++row)
            {
                IEnumerable<Transform> rowSquares = GetSquaresForRow(row);
                foreach (Transform square in rowSquares)
                {
                    // Update grid values (grid values are updated only for positions where squares are in this row)
                    int x = Mathf.RoundToInt(square.position.x);
                    int y = Mathf.RoundToInt(square.position.y);
                    grid[x, y - 1] = true;
                    grid[x, y] = false;
                    // Move the square for one position down
                    square.position += Vector3.down;
                }
            }
        }

        /// <summary>
        /// Gets all of the square that are in this row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private IEnumerable<Transform> GetSquaresForRow(int row)
        {
            List<Transform> squares = new List<Transform>();
            foreach (Transform tetromino in tetrominosParent)
            {
                squares.AddRange(tetromino.GetComponent<Tetromino>().squares.Where(square => square != null && Mathf.RoundToInt(square.position.y) == row));
            }
            return squares;
        }
    }
}
