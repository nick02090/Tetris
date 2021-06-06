using UnityEngine;
using Tetris.Core;
using Tetris.Control;

namespace Tetris.Gameplay
{
    public class TetrominoSpawner : MonoBehaviour
    {
        public TetrominoControl tetrominoControl;
        public TetrisGrid tetrisGrid;
        public Tetromino[] tetrominos;

        private void Start()
        {
            tetrisGrid.tetrominosParent = transform;
            SpawnTetromino();
        }

        public void SpawnTetromino()
        {
            Tetromino tetromino = GetRandomTetromino();
            tetromino.tetrominoSpawner = this;
            tetromino.tetrisGrid = tetrisGrid;
            tetromino.control = tetrominoControl;
            ObjectSpawner.Spawn(tetromino.gameObject, tetromino.transform.position, transform);
        }

        private Tetromino GetRandomTetromino()
        {
            return tetrominos[Random.Range(0, tetrominos.Length)];
        }
    }
}
