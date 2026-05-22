#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ContractorSimulator.Tests.PlayMode.Scenes
{
    public static class SceneTestHelper
    {
        public static bool IsSceneInBuildSettings(string sceneName)
        {
#if UNITY_EDITOR
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;

                if (scene.path.EndsWith($"/{sceneName}.unity") || scene.path.EndsWith($"\\{sceneName}.unity"))
                    return true;
            }
#endif
            return false;
        }
    }
}
