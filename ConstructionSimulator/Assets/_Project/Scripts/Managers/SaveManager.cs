using System;
using ContractorSimulator.SaveSystem;
using ContractorSimulator.SaveSystem.Data;
using UnityEngine;

namespace ContractorSimulator.Managers
{
    public class SaveManager : PersistentSingleton<SaveManager>
    {
        public SaveData CurrentSave { get; private set; }

        public string SaveFileName => "save_data.json";
        public string SavePath => SaveFileStorage.GetSavePath(SaveFileName);

        public event Action<SaveData> OnSave;
        public event Action<SaveData> OnLoad;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        public void Save()
        {
            if (CurrentSave == null)
            {
                CurrentSave = new SaveData();
            }

            try
            {
                var json = SaveDataSerializer.Serialize(CurrentSave);
                if (SaveFileStorage.WriteJson(SavePath, json))
                {
                    Debug.Log($"SaveManager: Saved data to {SavePath}");
                    OnSave?.Invoke(CurrentSave);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: Failed to save data. {ex.Message}");
            }
        }

        public void Load()
        {
            try
            {
                if (!SaveFileStorage.SaveFileExists(SavePath))
                {
                    CurrentSave = new SaveData();
                    OnLoad?.Invoke(CurrentSave);
                    return;
                }

                var json = SaveFileStorage.ReadJson(SavePath);
                CurrentSave = SaveDataSerializer.Deserialize<SaveData>(json) ?? new SaveData();
                Debug.Log($"SaveManager: Loaded save data from {SavePath}");
                OnLoad?.Invoke(CurrentSave);
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveManager: Failed to load save data. {ex.Message}");
                CurrentSave = new SaveData();
                OnLoad?.Invoke(CurrentSave);
            }
        }
    }
}
