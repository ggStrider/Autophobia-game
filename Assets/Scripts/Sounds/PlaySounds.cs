using UnityEngine;

using System;
using System.Collections.Generic;

namespace Autophobia.Sounds
{
    public class PlaySounds : MonoBehaviour
    {
        [SerializeField] private List<SoundsSettings> _sounds;

        public void PlaySound()
        {
            foreach (var sound in _sounds)
            {
                sound.audioSource.volume = sound.volume;
                sound.audioSource.Play();
            }
        }
        
        [Serializable]
        public class SoundsSettings
        {
            [SerializeField] private AudioClip _audioToPlay;
            public AudioClip audioToPlay => _audioToPlay;
            
            public AudioSource audioSource;

            [SerializeField] [Range(0, 1)] private float _volume;
            public float volume => _volume;
        }
    }
}