using System;

namespace ContractorSimulator.SaveSystem.Data
{
    [Serializable]
    public class SaveData
    {
        public int Credits;
        public string LastScene = string.Empty;

        public float MasterVolume = 1f;
        public float MusicVolume = 0.8f;
        public float SfxVolume = 0.9f;

        public PlayerSaveData Player = new PlayerSaveData();
        public ContractSaveData Contract = new ContractSaveData();
    }

    [Serializable]
    public class PlayerSaveData
    {
        public string PlayerId = string.Empty;
        public Vector3Serializable LastPosition = new Vector3Serializable();
    }

    [Serializable]
    public class ContractSaveData
    {
        public string ActiveContractId = string.Empty;
        public bool ContractCompleted;
    }

    [Serializable]
    public class Vector3Serializable
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3Serializable() { }

        public Vector3Serializable(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3Serializable FromUnityVector(UnityEngine.Vector3 vector)
        {
            return new Vector3Serializable(vector.x, vector.y, vector.z);
        }

        public UnityEngine.Vector3 ToUnityVector()
        {
            return new UnityEngine.Vector3(X, Y, Z);
        }
    }
}
