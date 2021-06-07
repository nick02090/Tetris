using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    public class GameCore : MonoBehaviour
    {
        private bool isLeftHanded = false;

        public RectTransform controlButtonsParent;
        public RectTransform pausePanel;

        public RectTransform leftPauseButton;
        public RectTransform rightPauseButton;
        public RectTransform leftRotateButton;
        public RectTransform rightRotateButton;

        private void Start()
        {
            SetButtonsAccordingToHand();
        }

        private void SetButtonsAccordingToHand()
        {
            leftPauseButton.gameObject.SetActive(!isLeftHanded);
            rightPauseButton.gameObject.SetActive(isLeftHanded);
            leftRotateButton.gameObject.SetActive(isLeftHanded);
            rightRotateButton.gameObject.SetActive(!isLeftHanded);
        }

        public void ChangeHandSide()
        {
            isLeftHanded = !isLeftHanded;
            SetButtonsAccordingToHand();
        }

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

        public void ResumeGame()
        {
            // Disable control buttons
            foreach (Transform button in controlButtonsParent)
            {
                button.GetComponent<Button>().enabled = true;
            }
            // Hid pause panel
            pausePanel.gameObject.SetActive(false);
            // Resume time
            Time.timeScale = 1f;
        }

        public void RestartGame()
        {

        }

        public void QuitGame()
        {

        }
    }
}
