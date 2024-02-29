using UnityEngine;

using System;
using System.Collections.Generic;

namespace Autophobia.Sounds
{
    public class PlaySounds : MonoBehaviour
    {
        [SerializeField] private List<SoundsSettings> _sounds;

        public void PlayAllSounds()
        {
            foreach (var sound in _sounds)
            {
                sound.audioSource.volume = sound.volume;
                sound.audioSource.Play();
            }
        }

        public void StopAllSounds()
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

            [SerializeField] [Range(0, 1)] private float _volume = 1f;
            public float volume => _volume;
        }
    }
}