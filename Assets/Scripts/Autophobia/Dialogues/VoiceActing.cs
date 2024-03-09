using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Autophobia.Dialogues
{
    public class VoiceActing : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<AudioClip> _clips;

        [ContextMenu("play")]
        public void OnPlaySequentially()
        {
            StartCoroutine(PlaySequentially());
        }
        
        private IEnumerator PlaySequentially()
        {
            foreach (var clip in _clips)
            {
                _source.clip = clip;
                _source.Play();

                yield return new WaitForSeconds(clip.length);
            }
        }
    }
}