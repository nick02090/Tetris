using UnityEngine;
using Tetris.Core;
using Tetris.Control;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Tetris.Gameplay
{
    public class TetrominoSpawner : MonoBehaviour
    {
        [SerializeField]
        // Tetromino control that is assigned to newly spawned tetromino
        public TetrominoControl tetrominoControl;
        [SerializeField]
        // Tetris grid that newly spawned tetromino will be added to
        private TetrisGrid tetrisGrid;

        [SerializeField]
        // Array of all tetromino prefabs
        private Tetromino[] tetrominos;

        [SerializeField]
        // UI Image elements that show the upcoming tetromino sprites
        private Image[] nextTetrominoImages;

        // Tetrominos that will be spawned next
        private Queue<Tetromino> nextTetrominos;

        // Flag for spawning new tetrominos
        private bool spawnNewTetrominos = true;

        private void Start()
        {
            // Subscribe to end game
            tetrisGrid.endGameDelegate += StopSpawning;
            // Initialize member variables
            nextTetrominos = new Queue<Tetromino>(nextTetrominoImages.Length);
            // Prepare next tetrominos
            PrepareTetrominos();
            // Spawn first tetromino
            SpawnTetromino();
        }

        private void StopSpawning()
        {
            spawnNewTetrominos = false;
        }

        /// <summary>
        /// Creates new random queue of tetrominos
        /// </summary>
        private void PrepareTetrominos()
        {
            foreach (Image nextTetrominoImage in nextTetrominoImages)
            {
                Tetromino nextTetromino = GetRandomTetromino();
                nextTetrominos.Enqueue(nextTetromino);
                nextTetrominoImage.sprite = nextTetromino.Sprite;
            }
        }

        /// <summary>
        /// Restarts spawner values
        /// </summary>
        public void Restart()
        {
            nextTetrominos.Clear();
            PrepareTetrominos();
            spawnNewTetrominos = true;
            SpawnTetromino();
        }

        public void SpawnTetromino()
        {
            if (!spawnNewTetrominos)
                return;

            // Get the next tetromino from the array
            Tetromino nextTetromino = nextTetrominos.Dequeue();
            // Spawn new tetromino
            Tetromino spawnedTetromino = ObjectSpawner.Spawn(nextTetromino.gameObject, nextTetromino.transform.position, tetrisGrid.TetrominosParent).GetComponent<Tetromino>();
            // Initialize tetromino member variables
            spawnedTetromino.tetrisGrid = tetrisGrid;
            spawnedTetromino.control = tetrominoControl;
            // Subscribe this method to tetrominos death
            spawnedTetromino.onDeathDelegate += SpawnTetromino;

            // Update next tetromino images array
            for (int i = 0; i < nextTetrominoImages.Length - 1; ++i)
            {
                nextTetrominoImages[i].sprite = nextTetrominoImages[i + 1].sprite;
            }
            // Choose next random tetromino
            Tetromino newTetromino = GetRandomTetromino();
            nextTetrominos.Enqueue(newTetromino);
            nextTetrominoImages[nextTetrominoImages.Length - 1].sprite = newTetromino.Sprite;
        }

        /// <summary>
        /// Randomly chooses next tetromino from prefab tetrominos array
        /// </summary>
        /// <returns></returns>
        private Tetromino GetRandomTetromino()
        {
            return tetrominos[Random.Range(0, tetrominos.Length)];
        }
    }
}
