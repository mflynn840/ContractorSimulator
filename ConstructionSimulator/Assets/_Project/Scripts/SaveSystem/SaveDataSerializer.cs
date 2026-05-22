using UnityEngine;

namespace ContractorSimulator.SaveSystem
{
    public static class SaveDataSerializer
    {
        public static string Serialize<T>(T saveObject)
        {
            return JsonUtility.ToJson(saveObject, true);
        }

        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonUtility.FromJson<T>(json);
        }
    }
}
