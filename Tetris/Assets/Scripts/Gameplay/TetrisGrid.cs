using UnityEngine;

namespace Tetris.Gameplay
{
    public class TetrisGrid : MonoBehaviour
    {
        public const int gridWidth = 10;
        public const int gridHeight = 21;

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
        }
    }
}
