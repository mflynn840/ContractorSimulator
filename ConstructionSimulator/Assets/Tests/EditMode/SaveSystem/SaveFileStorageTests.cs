using System;
using System.IO;
using ContractorSimulator.SaveSystem;
using ContractorSimulator.Tests;
using ContractorSimulator.Tests.Utilities;
using NUnit.Framework;

namespace ContractorSimulator.Tests.EditMode.SaveSystem
{
    [Category(TestCategories.Unit)]
    public class SaveFileStorageTests
    {
        private string _tempPath;

        [SetUp]
        public void SetUp()
        {
            _tempPath = TestFileUtility.CreateTempJsonPath("storage");
        }

        [TearDown]
        public void TearDown()
        {
            TestFileUtility.DeleteIfExists(_tempPath);
        }

        [Test]
        public void GetSavePath_ValidFileName_CombinesWithSaveDirectory()
        {
            var path = SaveFileStorage.GetSavePath("unit_test.json");
            Assert.That(path, Does.EndWith("unit_test.json"));
            Assert.That(path, Does.StartWith(SaveFileStorage.SaveDirectory));
        }

        [Test]
        public void GetSavePath_NullOrWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() => SaveFileStorage.GetSavePath(null));
            Assert.Throws<ArgumentException>(() => SaveFileStorage.GetSavePath(""));
            Assert.Throws<ArgumentException>(() => SaveFileStorage.GetSavePath("   "));
        }

        [Test]
        public void WriteJson_ThenReadJson_ReturnsSameContent()
        {
            const string payload = "{\"Credits\":99}";

            Assert.That(SaveFileStorage.WriteJson(_tempPath, payload), Is.True);
            Assert.That(SaveFileStorage.SaveFileExists(_tempPath), Is.True);
            Assert.That(SaveFileStorage.ReadJson(_tempPath), Is.EqualTo(payload));
        }

        [Test]
        public void ReadJson_MissingFile_ReturnsEmpty()
        {
            var missing = Path.Combine(TestFileUtility.TestsRoot, "missing_file.json");
            TestFileUtility.DeleteIfExists(missing);

            Assert.That(SaveFileStorage.ReadJson(missing), Is.EqualTo(string.Empty));
        }
    }
}
