using UnityEngine;

namespace ContractorSimulator.Managers
{
    public class SettingsManager : PersistentSingleton<SettingsManager>
    {
        public float MasterVolume { get; private set; } = 1f;
        public float MusicVolume { get; private set; } = 0.8f;
        public float SfxVolume { get; private set; } = 0.9f;
        public bool IsInvertY { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ApplyAudioSettings();
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume = Mathf.Clamp01(volume);
            ApplyAudioSettings();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = Mathf.Clamp01(volume);
        }

        public void SetSfxVolume(float volume)
        {
            SfxVolume = Mathf.Clamp01(volume);
        }

        public void SetInvertY(bool invert)
        {
            IsInvertY = invert;
        }

        public void ApplyAudioSettings()
        {
            AudioListener.volume = MasterVolume;
        }
    }
}
