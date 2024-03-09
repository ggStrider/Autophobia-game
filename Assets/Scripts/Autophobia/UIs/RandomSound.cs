using UnityEngine;

using Autophobia.Utilities;
using System.Collections.Generic;

namespace Autophobia.UIs
{
    public class RandomSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<AudioClip> _clips;

        [Header("Pitch")]
        [SerializeField] private bool _randomPitch;

        [SerializeField, Range(-3, 3)] private float _minimumPitch; 
        [SerializeField, Range(-3, 3)] private float _maximumPitch; 

        public void _Play()
        {
            var randomNumber = DoRandom.RandomInt(0, _clips.Count);
            _source.clip = _clips[randomNumber];

            if (_randomPitch) 
                _source.pitch = DoRandom.RandomFloat(_minimumPitch, _maximumPitch);
            
            _source.Play();
        }
    }
}