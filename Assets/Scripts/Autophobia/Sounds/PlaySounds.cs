using UnityEngine;

using System;
using System.Collections.Generic;

namespace Autophobia.Sounds
{
    public class PlaySounds : MonoBehaviour
    {
        [SerializeField] private List<SoundsSettings> _sounds;

        public void _PlayAllSounds()
        {
            foreach (var sound in _sounds)
            {
                sound.audioSource.volume = sound.volume;
                sound.audioSource.clip = sound.clip;
                
                sound.audioSource.Play();
            }
        }

        public void _StopAllSounds()
        {
            foreach (var sound in _sounds)
            {
                sound.audioSource.Stop();
            }
        }
        
        [Serializable]
        public class SoundsSettings
        {
            public AudioSource audioSource;

            [field: SerializeField] public AudioClip clip { get; private set; }
            [field: SerializeField] public float volume { get; private set; }
        }
    }
}