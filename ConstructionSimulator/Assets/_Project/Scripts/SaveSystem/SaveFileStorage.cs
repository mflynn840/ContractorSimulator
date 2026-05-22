using System;
using System.IO;
using UnityEngine;

namespace ContractorSimulator.SaveSystem
{
    public static class SaveFileStorage
    {
        public static string SaveDirectory => Application.persistentDataPath;

        public static string GetSavePath(string saveFileName)
        {
            if (string.IsNullOrWhiteSpace(saveFileName))
            {
                throw new ArgumentException("Save file name cannot be null or empty.", nameof(saveFileName));
            }

            return Path.Combine(SaveDirectory, saveFileName);
        }

        public static bool EnsureDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(SaveDirectory))
                {
                    Directory.CreateDirectory(SaveDirectory);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveFileStorage: Failed to ensure save directory exists. {ex.Message}");
                return false;
            }
        }

        public static bool WriteJson(string filePath, string json)
        {
            try
            {
                if (!EnsureDirectoryExists())
                    return false;

                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveFileStorage: Failed to write save file. {ex.Message}");
                return false;
            }
        }

        public static string ReadJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return string.Empty;

                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveFileStorage: Failed to read save file. {ex.Message}");
                return string.Empty;
            }
        }

        public static bool SaveFileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
