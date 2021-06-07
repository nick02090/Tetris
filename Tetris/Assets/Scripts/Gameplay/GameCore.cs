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
            DisableControls();
            // Show end panel
            endPanel.gameObject.SetActive(true);
            // Stop time
            Time.timeScale = 0f;
        }

        /// <summary>
        /// Disables control buttos
        /// </summary>
        private void DisableControls()
        {
            foreach (Transform button in controlButtonsParent)
            {
                button.GetComponent<Button>().enabled = false;
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
            DisableControls();
            // Show pause panel
            pausePanel.gameObject.SetActive(true);
            // Stop time
            Time.timeScale = 0f;
        }

        // Called from Unity Editor (GUI)
        public void ResumeGame()
        {
            // Disable control buttons
            DisableControls();
            // Hide pause panel
            pausePanel.gameObject.SetActive(false);
            // Resume time
            Time.timeScale = 1f;
        }

        // Called from Unity Editor (GUI)
        public void RestartGame()
        {

        }

        // Called from Unity Editor (GUI)
        public void QuitGame()
        {

        }
    }
}
