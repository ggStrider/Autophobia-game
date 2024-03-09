using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Threading.Tasks;

namespace Autophobia.UIs
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;
        [SerializeField] private float _fadeSpeed;
        [SerializeField] private float _unFadeSpeed;

        [Space] [SerializeField] private bool _unFadeOnStart;

        [SerializeField] private UnityEvent _faded;
        [SerializeField] private UnityEvent _unFaded;

        private async void Start()
        {
            if (_unFadeOnStart) await UnFade();
        }

        public async void _OnFade()
        {
            await Fade();
        }
        
        public async void _OnUnFade()
        {
            await UnFade();
        }
        
        public async Task UnFade()
        {
            var newColor = _blackScreen.color;
            while (_blackScreen.color.a > 0)
            {
                if (_blackScreen == null) return;
                newColor.a = Mathf.Clamp01(newColor.a - _unFadeSpeed);
                _blackScreen.color = newColor;

                await Task.Yield();
            }
            
            _unFaded?.Invoke();
        }
        
        public async Task Fade()
        {
            var newColor = _blackScreen.color;
            while (_blackScreen.color.a < 1)
            {
                newColor.a = Mathf.Clamp01(newColor.a + _fadeSpeed);
                _blackScreen.color = newColor;

                await Task.Yield();
            }
            
            _faded?.Invoke();
        }
    }
}