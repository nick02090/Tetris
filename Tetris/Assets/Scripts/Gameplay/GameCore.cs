using Tetris.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    [RequireComponent(typeof(SceneLoading))]
    public class GameCore : MonoBehaviour
    {
        // Scene number of the main menu
        [SerializeField]
        private int mainMenuScene;

        // Component for scene loading
        private SceneLoading sceneLoading;

        // Setting properties
        private bool isLeftHanded = false;
        private bool isMusicON = true;
        private bool isSFXON = true;

        // Sprites for the music and SFX ON/OFF
        [SerializeField]
        private Sprite musicONSprite;
        [SerializeField]
        private Sprite musicOFFSprite;
        [SerializeField]
        private Sprite SFXONSprite;
        [SerializeField]
        private Sprite SFXOFFSprite;

        // Toggle buttons for music and SFX
        [SerializeField]
        private Button toggleMusicButton;
        [SerializeField]
        private Button toggleSFXButton;

        // Players for music and SFX
        [SerializeField]
        private AudioSource musicPlayer;
        [SerializeField]
        private AudioSource[] SFXPlayers;

        [SerializeField]
        // Control buttons (disabled upon pause)
        private RectTransform controlButtonsParent;
        [SerializeField]
        // Overlay panel for paused game
        private RectTransform pausePanel;
        [SerializeField]
        // Overlay panel for end game
        private RectTransform endPanel;
        [SerializeField]
        // Overlay panel for scene loading
        private RectTransform loadingPanel;

        [SerializeField]
        // Grid on which the game is played
        private TetrisGrid tetrisGrid;
        [SerializeField]
        // Spawner for the tetrominos
        private TetrominoSpawner tetrominoSpawner;
        [SerializeField]
        // Score manager
        private ScoreManager scoreManager;
        [SerializeField]
        // Level manager
        private LevelManager levelManager;

        // Buttons that are hidden/shown based on isLeftHanded property
        [SerializeField]
        private RectTransform leftPauseButton;
        [SerializeField]
        private RectTransform rightPauseButton;
        [SerializeField]
        private RectTransform leftRotateButton;
        [SerializeField]
        private RectTransform rightRotateButton;

        private void Start()
        {
            sceneLoading = GetComponent<SceneLoading>();
            // Subscribe to grids end game
            tetrisGrid.endGameDelegate += EndGame;
            // Setup initial button positions
            SetButtonsAccordingToHand();
            // Setup music and SFX sprites
            toggleMusicButton.GetComponent<Image>().sprite = isMusicON ? musicONSprite : musicOFFSprite;
            toggleSFXButton.GetComponent<Image>().sprite = isSFXON ? SFXONSprite : SFXOFFSprite;
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
            levelManager.Restart();
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
            pausePanel.gameObject.SetActive(false);
            endPanel.gameObject.SetActive(false);
            loadingPanel.gameObject.SetActive(true);
            sceneLoading.LoadScene(mainMenuScene);
            Time.timeScale = 1f;
        }

        // Called from Unity Editor (GUI)
        public void ToggleMusic()
        {
            isMusicON = !isMusicON;
            toggleMusicButton.GetComponent<Image>().sprite = isMusicON ? musicONSprite : musicOFFSprite;
            musicPlayer.mute = !isMusicON;
        }

        // Called from Unity Editor (GUI)
        public void ToggleSFX()
        {
            isSFXON = !isSFXON;
            toggleSFXButton.GetComponent<Image>().sprite = isSFXON ? SFXONSprite : SFXOFFSprite;
            foreach (AudioSource SFXPlayer in SFXPlayers)
                SFXPlayer.mute = !isSFXON;
        }
    }
}
