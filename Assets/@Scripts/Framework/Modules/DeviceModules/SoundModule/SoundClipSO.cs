using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules.SoundModule
{
    [Serializable]
    public class SoundData
    {
        public SoundClipName Soundtype;
        public AudioClip Clip;
    }
    
    [CreateAssetMenu(fileName = "ClipSO", menuName = "DataContainer/Sound")]
    public class SoundClipSO : DescriptionSO
    {
        [SerializeField] [SerializedDictionary("(Enum)SoundClipName","(Clip)AudioClip")]
        private SerializedDictionary<SoundClipName, AudioClip> soundDictionary;
        
        public AudioClip GetClip(SoundClipName clipName)
        {
            if (soundDictionary.TryGetValue(clipName, out AudioClip clip)) return clip;
            throw new NullReferenceException(clipName.ToString());
        }
    }
}