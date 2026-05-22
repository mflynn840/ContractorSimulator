using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContractorSimulator.Managers
{
    public class SceneLoader : PersistentSingleton<SceneLoader>
    {
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning("SceneLoader: sceneName is null or empty.");
                return;
            }

            SceneManager.LoadScene(sceneName);
        }

        public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogWarning("SceneLoader: sceneName is null or empty.");
                return null;
            }

            return SceneManager.LoadSceneAsync(sceneName, loadMode);
        }
    }
}
