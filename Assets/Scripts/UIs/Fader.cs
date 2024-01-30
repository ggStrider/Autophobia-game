using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;

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

        private void Start()
        {
            if (_unFadeOnStart) StartCoroutine(UnFade());
        }

        public void OnUnFade()
        {
            StartCoroutine(UnFade());
        }

        public void OnFade()
        {
            StartCoroutine(Fade());
        }
        
        private IEnumerator UnFade()
        {
            var newColor = _blackScreen.color;
            while (_blackScreen.color.a > 0)
            {
                newColor.a = Mathf.Clamp01(newColor.a - _unFadeSpeed);
                _blackScreen.color = newColor;

                yield return null;
            }
            
            _unFaded?.Invoke();
        }
        
        private IEnumerator Fade()
        {
            var newColor = _blackScreen.color;
            while (_blackScreen.color.a < 1)
            {
                newColor.a = Mathf.Clamp01(newColor.a + _fadeSpeed);
                _blackScreen.color = newColor;

                yield return null;
            }
            
            _faded?.Invoke();
        }
    }
}