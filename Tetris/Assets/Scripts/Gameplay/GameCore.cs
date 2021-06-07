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

        // Buttons that are hidden/shown based on isLeftHanded property
        public RectTransform leftPauseButton;
        public RectTransform rightPauseButton;
        public RectTransform leftRotateButton;
        public RectTransform rightRotateButton;

        private void Start()
        {
            SetButtonsAccordingToHand();
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
            foreach (Transform button in controlButtonsParent)
            {
                button.GetComponent<Button>().enabled = false;
            }
            // Show pause panel
            pausePanel.gameObject.SetActive(true);
            // Stop time
            Time.timeScale = 0f;
        }

        // Called from Unity Editor (GUI)
        public void ResumeGame()
        {
            // Disable control buttons
            foreach (Transform button in controlButtonsParent)
            {
                button.GetComponent<Button>().enabled = true;
            }
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
