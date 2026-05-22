using UnityEngine;

namespace ContractorSimulator.Networking.PlayerSync
{
    public class NetworkPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private AudioListener audioListener;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponent<Camera>();
            }

            if (audioListener == null)
            {
                audioListener = GetComponent<AudioListener>();
            }
        }

        public void SetLocalOwner(bool isLocalOwner)
        {
            if (playerCamera != null)
            {
                playerCamera.enabled = isLocalOwner;
                playerCamera.tag = isLocalOwner ? "MainCamera" : "Untagged";
            }

            if (audioListener != null)
            {
                audioListener.enabled = isLocalOwner;
            }

        }
    }
}
