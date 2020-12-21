using loadingscreen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace contextswitcher.loader
{
    /// <summary>
    /// Load a level
    /// </summary>
    public class LevelLoader : Loader
    {
        [SerializeField] private LoadingScreen _loadingScreen = default;

        /// <summary>
        /// Load a level at sceneIndex
        /// </summary>
        /// <param name="sceneIndex"></param>
        public void LoadLevel(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        }

        public void LoadLevelAsync(int sceneIndex)
        {
            // Switch context
            _contextSwitcher.ActivateContext(_contextNameToSwitch);

            StartCoroutine(FakeLoadingAsync(sceneIndex));
        }

        /// <summary>
        /// Load a level at sceneIndex (asynchrone)
        /// </summary>
        /// <param name="sceneIndex"></param>
        public IEnumerator LoadAsync(int sceneIndex)
        {
            string title = "Loading level " + sceneIndex;
            ChangeLoadingScreenTitleText(title);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            float progress;
            while (!operation.isDone)
            {
                progress = Mathf.Clamp01(operation.progress / 0.9f);
                ChangeLoadingScreenSliderValue(progress);

                string newProgressText = (progress * 100) + "%";
                ChangeLoadingScreenProgressText(newProgressText);

                yield return null;
            }
        }

        /// <summary>
        /// Fake a level load
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <returns></returns>
        public IEnumerator FakeLoadingAsync(int sceneIndex)
        {
            string title = "Loading level " + sceneIndex;
            ChangeLoadingScreenTitleText(title);

            float progress = 0f;
            while (progress < 1f)
            {
                ChangeLoadingScreenSliderValue(progress);

                string newProgressText = (progress * 100) + "%";
                ChangeLoadingScreenProgressText(newProgressText);

                progress += 0.1f;

                yield return new WaitForSeconds(0.3f);
            }

            LoadLevel(sceneIndex);
        }

        public void ChangeLoadingScreenTitleText(string title)
        {
            _loadingScreen.ChangeTitleText(title);
        }

        public void ChangeLoadingScreenSliderValue(float value)
        {
            _loadingScreen.ChangeSliderValue(value);
        }

        public void ChangeLoadingScreenProgressText(string text)
        {
            _loadingScreen.ChangeProgressText(text);
        }
    }
}
