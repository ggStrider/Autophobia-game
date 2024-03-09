using UnityEngine;
using UnityEngine.Video;

using Autophobia.UIs;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Media
{
    public class EndVideoEvent : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;
        [SerializeField] private Fader _fader;
        [SerializeField] private GameObject _videoScreen;

        [SerializeField] private UnityEvent _event;

        private async Task Start()
        {
            var delay = _videoPlayer.clip.length;
            await Task.Delay((int)delay * 1000 + 1000);

            _videoScreen.SetActive(false);

            await Task.Delay(3000);
            
            _event?.Invoke();
            await _fader.UnFade();
        }
    }
}