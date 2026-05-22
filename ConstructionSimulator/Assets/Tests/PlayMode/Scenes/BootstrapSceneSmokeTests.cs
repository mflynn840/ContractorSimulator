using System.Collections;
using ContractorSimulator.Tests;
using ContractorSimulator.Tests.PlayMode.Scenes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace ContractorSimulator.Tests.PlayMode.Scenes
{
    [Category(TestCategories.Scene)]
    public class BootstrapSceneSmokeTests
    {
        [UnityTest]
        public IEnumerator Bootstrap_Loads_WhenInBuildSettings()
        {
            if (!SceneTestHelper.IsSceneInBuildSettings(TestSceneNames.Bootstrap))
            {
                Assert.Ignore(
                    $"Scene '{TestSceneNames.Bootstrap}' is not in Editor Build Settings. " +
                    "Add Assets/Scenes/Bootstrap/Bootstrap.unity to run this test.");
            }

            var loadOp = SceneManager.LoadSceneAsync(TestSceneNames.Bootstrap, LoadSceneMode.Single);
            Assert.That(loadOp, Is.Not.Null);

            while (!loadOp.isDone)
                yield return null;

            yield return null;

            var scene = SceneManager.GetActiveScene();
            Assert.That(scene.name, Is.EqualTo(TestSceneNames.Bootstrap));
            Assert.That(scene.isLoaded, Is.True);
        }
    }
}
