using Tetris.Control;
using Tetris.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.UI
{
    [RequireComponent(typeof(TouchControl))]
    [RequireComponent(typeof(SceneLoading))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        // Text that fades in/out at the beginning of the main menu
        private Text tapText;

        [SerializeField]
        // Scene number for the game
        private int gameScene;

        [SerializeField]
        // Menu buttons displayed on screen
        private RectTransform menu;

        [SerializeField]
        // Text for loading
        private Text loadingText;

        private TouchControl touchControl;
        private bool hadInitialTouch;

        private SceneLoading sceneLoading;

        private void Start()
        {
            hadInitialTouch = false;
            touchControl = GetComponent<TouchControl>();
            sceneLoading = GetComponent<SceneLoading>();
            tapText.gameObject.SetActive(true);
            menu.gameObject.SetActive(false);
            loadingText.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (touchControl.HasActiveTouch && !hadInitialTouch)
            {
                hadInitialTouch = true;
                tapText.gameObject.SetActive(false);
                menu.gameObject.SetActive(true);
            }
        }

        public void PlayGame()
        {
            menu.gameObject.SetActive(false);
            loadingText.gameObject.SetActive(true);
            sceneLoading.LoadScene(gameScene);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
