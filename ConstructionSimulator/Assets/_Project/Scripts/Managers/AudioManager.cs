using UnityEngine;

namespace ContractorSimulator.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }

            _audioSource.playOnAwake = false;
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioManager: No clip provided for PlayOneShot.");
                return;
            }

            _audioSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void SetMasterVolume(float volume)
        {
            _audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}
