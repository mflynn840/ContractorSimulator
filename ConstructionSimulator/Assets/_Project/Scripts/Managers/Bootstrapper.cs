using ContractorSimulator.Networking;
using UnityEngine;

namespace ContractorSimulator.Managers
{
    [DefaultExecutionOrder(-1000)]
    public class Bootstrapper : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (FindObjectOfType<Bootstrapper>() != null)
            {
                return;
            }

            var bootstrapObject = new GameObject("[Bootstrap]");
            bootstrapObject.AddComponent<Bootstrapper>();
            bootstrapObject.AddComponent<GameManager>();
            bootstrapObject.AddComponent<SceneLoader>();
            bootstrapObject.AddComponent<SaveManager>();
            bootstrapObject.AddComponent<AudioManager>();
            bootstrapObject.AddComponent<SettingsManager>();
            DontDestroyOnLoad(bootstrapObject);

            NetworkBootstrap.EnsureInitialized();
        }
    }
}
