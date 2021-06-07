using UnityEngine;
using Tetris.Core;
using Tetris.Control;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    public class TetrominoSpawner : MonoBehaviour
    {
        public TetrominoControl tetrominoControl;
        public TetrisGrid tetrisGrid;
        public Tetromino[] tetrominos;

        public Image[] nextTetrominoImages;
        private Tetromino[] nextTetrominos;

        private void Start()
        {
            tetrisGrid.tetrominosParent = transform;
            nextTetrominos = new Tetromino[nextTetrominoImages.Length];
            for (int i = 0; i < nextTetrominoImages.Length; ++i)
            {
                nextTetrominos[i] = GetRandomTetromino();
                nextTetrominoImages[i].sprite = nextTetrominos[i].sprite;
            }
            SpawnTetromino();
        }

        public void SpawnTetromino()
        {
            // Spawn the next tetromino
            Tetromino nextTetromino = nextTetrominos[0];
            nextTetromino.tetrominoSpawner = this;
            nextTetromino.tetrisGrid = tetrisGrid;
            nextTetromino.control = tetrominoControl;
            ObjectSpawner.Spawn(nextTetromino.gameObject, nextTetromino.transform.position, transform);

            // Update next tetromino arrays
            for (int i = 0; i < nextTetrominoImages.Length - 1; ++i)
            {
                nextTetrominoImages[i].sprite = nextTetrominoImages[i + 1].sprite;
                nextTetrominos[i] = nextTetrominos[i + 1];
            }
            // Choose next random tetromino
            nextTetromino = GetRandomTetromino();
            nextTetrominos[nextTetrominos.Length - 1] = nextTetromino;
            nextTetrominoImages[nextTetrominoImages.Length - 1].sprite = nextTetromino.sprite;
        }

        private Tetromino GetRandomTetromino()
        {
            return tetrominos[Random.Range(0, tetrominos.Length)];
        }
    }
}
