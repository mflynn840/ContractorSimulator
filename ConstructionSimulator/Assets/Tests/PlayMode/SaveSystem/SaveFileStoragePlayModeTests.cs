using System.Collections;
using ContractorSimulator.SaveSystem;
using ContractorSimulator.SaveSystem.Data;
using ContractorSimulator.Tests;
using ContractorSimulator.Tests.Utilities;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ContractorSimulator.Tests.PlayMode.SaveSystem
{
    [Category(TestCategories.Integration)]
    public class SaveFileStoragePlayModeTests
    {
        private string _tempPath;

        [SetUp]
        public void SetUp()
        {
            _tempPath = TestFileUtility.CreateTempJsonPath("playmode_save");
        }

        [TearDown]
        public void TearDown()
        {
            TestFileUtility.DeleteIfExists(_tempPath);
        }

        [UnityTest]
        public IEnumerator WriteAndRead_UsesPersistentDataPath_InPlayMode()
        {
            var save = new SaveData { Credits = 500, LastScene = "PlayModeTest" };
            var json = SaveDataSerializer.Serialize(save);

            Assert.That(SaveFileStorage.WriteJson(_tempPath, json), Is.True);

            yield return null;

            var readJson = SaveFileStorage.ReadJson(_tempPath);
            var restored = SaveDataSerializer.Deserialize<SaveData>(readJson);

            Assert.That(restored, Is.Not.Null);
            Assert.That(restored.Credits, Is.EqualTo(500));
            Assert.That(restored.LastScene, Is.EqualTo("PlayModeTest"));
        }
    }
}
