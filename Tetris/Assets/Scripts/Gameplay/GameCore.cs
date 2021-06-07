using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    public class GameCore : MonoBehaviour
    {
        private bool isLeftHanded = false;

        // Control buttons (disabled upon pause)
        public RectTransform controlButtonsParent;
        // Overlay panel for paused game
        public RectTransform pausePanel;
        // Overlay panel for end game
        public RectTransform endPanel;

        // Grid on which the game is played
        public TetrisGrid tetrisGrid;
        // Spawner for the tetrominos
        public TetrominoSpawner tetrominoSpawner;
        // Score manager
        public ScoreManager scoreManager;

        // Buttons that are hidden/shown based on isLeftHanded property
        public RectTransform leftPauseButton;
        public RectTransform rightPauseButton;
        public RectTransform leftRotateButton;
        public RectTransform rightRotateButton;

        private void Start()
        {
            // Subscribe to grids end game
            tetrisGrid.endGameDelegate += EndGame;
            // Setup initial button positions
            SetButtonsAccordingToHand();
        }

        private void EndGame()
        {
            // Disable controls
            ChangeControls(false);
            // Show end panel
            endPanel.gameObject.SetActive(true);
            // Stop time
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Disables/Enables control buttons
        /// </summary>
        private void ChangeControls(bool enable)
        {
            foreach (Transform button in controlButtonsParent)
            {
                button.GetComponent<Button>().enabled = enable;
            }
        }

        /// <summary>
        /// Hides/Shows GUI elements based on isLeftHanded property
        /// </summary>
        private void SetButtonsAccordingToHand()
        {
            leftPauseButton.gameObject.SetActive(!isLeftHanded);
            rightPauseButton.gameObject.SetActive(isLeftHanded);
            leftRotateButton.gameObject.SetActive(isLeftHanded);
            rightRotateButton.gameObject.SetActive(!isLeftHanded);
        }

        // Called from Unity Editor (GUI)
        public void ChangeHandSide()
        {
            isLeftHanded = !isLeftHanded;
            SetButtonsAccordingToHand();
        }

        // Called from Unity Editor (GUI)
        public void PauseGame()
        {
            // Disable control buttons
            ChangeControls(false);
            // Show pause panel
            pausePanel.gameObject.SetActive(true);
            // Stop time
            Time.timeScale = 0f;
        }

        // Called from Unity Editor (GUI)
        public void ResumeGame()
        {
            // Enable control buttons
            ChangeControls(true);
            // Hide pause panel
            pausePanel.gameObject.SetActive(false);
            // Resume time
            Time.timeScale = 1f;
        }

        // Called from Unity Editor (GUI)
        public void RestartGame()
        {
            // Restart game systems
            tetrisGrid.Restart();
            tetrominoSpawner.Restart();
            scoreManager.Restart();
            // Hide panels
            endPanel.gameObject.SetActive(false);
            pausePanel.gameObject.SetActive(false);
            // Enable controls
            ChangeControls(true);
            // Resume time
            Time.timeScale = 1f;
        }

        // Called from Unity Editor (GUI)
        public void QuitGame()
        {

        }
    }
}
