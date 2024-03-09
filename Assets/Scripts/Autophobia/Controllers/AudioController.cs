using UnityEngine;
using UnityEngine.Audio;
using Autophobia.Model;

namespace Autophobia.Controllers
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private float _multiplyDelta;
        
        private GameSession _session;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            CalculateNewMasterVolume();
        }

        public void CalculateNewMasterVolume()
        {
            var volume = _session.Data.MasterVolume;

            if (volume <= 0f)
            {
                volume = -80f;
            }
            else
            {
                volume = _multiplyDelta * Mathf.Log10(volume);
            }

            _audioMixer.SetFloat("Master", volume);
        }

        [ContextMenu("print")]
        private void GetMasterVolume()
        {
            _audioMixer.GetFloat("Master", out var volume);
            Debug.Log(Mathf.Pow(10, volume / 20));
        }
    }
}