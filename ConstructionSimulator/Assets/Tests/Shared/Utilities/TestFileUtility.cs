using System;
using System.IO;
using UnityEngine;

namespace ContractorSimulator.Tests.Utilities
{
    /// <summary>
    /// Isolated file paths for save/load and IO tests.
    /// </summary>
    public static class TestFileUtility
    {
        public static string TestsRoot =>
            Path.Combine(Application.persistentDataPath, "Tests");

        public static string CreateTempJsonPath(string prefix = "test")
        {
            EnsureTestsRootExists();
            return Path.Combine(TestsRoot, $"{prefix}_{Guid.NewGuid():N}.json");
        }

        public static void EnsureTestsRootExists()
        {
            if (!Directory.Exists(TestsRoot))
            {
                Directory.CreateDirectory(TestsRoot);
            }
        }

        public static void DeleteIfExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return;

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"TestFileUtility: Could not delete {filePath}. {ex.Message}");
            }
        }

        public static void CleanupTestsRoot()
        {
            if (!Directory.Exists(TestsRoot))
                return;

            try
            {
                Directory.Delete(TestsRoot, recursive: true);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"TestFileUtility: Could not cleanup tests root. {ex.Message}");
            }
        }
    }
}
