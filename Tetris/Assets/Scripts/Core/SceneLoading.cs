using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.Core
{
    public class SceneLoading : MonoBehaviour
    {
        public void LoadScene(int sceneToLoad)
        {
            StartCoroutine(LoadAsyncOperation(sceneToLoad));
        }

        private IEnumerator LoadAsyncOperation(int sceneToLoad)
        {
            // Start scene loading
            AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneToLoad);
            // Wait for scene to load
            while (gameLevel.progress < 1.0f)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}