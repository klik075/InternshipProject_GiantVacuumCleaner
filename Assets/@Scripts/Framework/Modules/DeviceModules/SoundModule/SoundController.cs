using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules.SoundModule
{
    public enum PitchType { Increase, Decrease, Stay }
    public class SoundController : DeviceController
    {
        [SerializeField]
        private AudioSource _source;
        [Range(-3f, 3f)] private float _currentPitch = 1f;

        protected override void Init()
        {
            base.Init();
            if (_init == true)
            { 
                _source = gameObject.AddComponent<AudioSource>();
                DefaultSet();
            }
        }
        
        public void PlayBGM(SoundClipName clipName, float volume = 1f, bool loop = false)
        {
            Init();
            if (DeviceManager.IsBGMMuted) return;
            _source.clip = Manager.GetClip(clipName);
            _source.volume = volume;
            _source.loop = loop;
            _source.Play();
        }
        
        public void PlayClip(SoundClipName clipName, float volume = 1f, bool loop = false)
        {
            Init();
            if (DeviceManager.IsSfxMuted) return;
            _source.clip = Manager.GetClip(clipName);
            _source.volume = volume;
            _source.loop = loop;
            _source.Play();
        }
        
        public void PlayClip(SoundClipName clipName, float volume = 1f, PitchType pitchType = PitchType.Stay, float pitchRange = 0f, bool loop = false)
        {
            if (DeviceManager.IsSfxMuted) return;
            PlayClip(clipName, volume, loop);
            if (pitchType is PitchType.Increase) _currentPitch = Mathf.Clamp(_currentPitch + pitchRange, -3f, 3f);
            if (pitchType is PitchType.Decrease) _currentPitch = Mathf.Clamp(_currentPitch - pitchRange, -3f, 3f);
            _source.pitch = _currentPitch;
        }
        
        public void DefaultSet(bool playing = false)
        {
            if (_source == null && playing) return;
            if (_source.isPlaying) _source.Stop();
            _source.clip = null;
            _source.loop = false;
            _source.pitch = 1f;
            _source.volume = 1f;
            _currentPitch = 1f;
        }

        private void OnDestroy()
        {
            Manager = null;
            _source = null;
        }
    }
}