using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tetris.Gameplay
{
    public class TetrisGrid : MonoBehaviour
    {
        public const int gridWidth = 10;
        public const int gridHeight = 21;

        public Transform tetrominosParent;

        // If grid[x][y] then it is filled, otherwise it is empty
        private bool[, ] grid;

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
                Tetromino tetromino = child.GetComponent<Tetromino>();
                // Destroy tetromino if all of his squares were destroyed
                if (tetromino.squares.All(square => square == null))
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public bool BlockTaken(int row, int col)
        {
            return grid[row, col];
        }

        public void AddToGrid(Tetromino tetromino)
        {
            // Set initial grid values
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
        }

        private bool[] CheckClearRows()
        {
            bool[] clearRows = new bool[gridHeight];

            // Check which rows should be cleared (from top to bottom)
            for (int col = gridHeight - 1; col >= 0; --col)
            {
                int row;
                for (row = 0; row < gridWidth; ++row)
                {
                    if (!grid[row, col])
                        break;
                }

                // If iteration through this whole row went without a break, then this row should be cleared
                if (row == gridWidth)
                    clearRows[col] = true;
                else
                    clearRows[col] = false;
            }

            return clearRows;
        }

        private void ClearRows(bool[] clearRows)
        {
            // Clear every row that needs to be cleared
            for (int col = gridHeight - 1; col >= 0; --col)
            {
                if (clearRows[col])
                {
                    // Remove squares from this row
                    foreach (Transform square in GetSquaresForRow(col))
                    {
                        DestroyImmediate(square.gameObject);
                    }
                    // Update the grid for this row
                    for (int row = 0; row < gridWidth; ++row)
                        grid[row, col] = false;
                    // Make everything above this row fall down
                    RowsFall(col + 1);
                }
            }
        }

        private IEnumerable<Transform> GetSquaresForRow(int row)
        {
            List<Transform> squares = new List<Transform>();
            foreach (Transform tetromino in tetrominosParent)
            {
                squares.AddRange(tetromino.GetComponent<Tetromino>().squares.Where(square => square != null && Mathf.RoundToInt(square.position.y) == row));
            }
            return squares;
        }

        private void RowsFall(int startingRow)
        {
            for (int col = startingRow; col < gridHeight; ++col)
            {
                IEnumerable<Transform> rowSquares = GetSquaresForRow(col);
                foreach (Transform square in rowSquares)
                {
                    // Update grid values
                    int x = Mathf.RoundToInt(square.position.x);
                    int y = Mathf.RoundToInt(square.position.y);
                    grid[x, y - 1] = true;
                    grid[x, y] = false;
                    // Move the square for one position down
                    square.position += Vector3.down;
                }
            }
        }
    }
}
