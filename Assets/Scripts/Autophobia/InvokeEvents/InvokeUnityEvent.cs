using UnityEngine;
using UnityEngine.Events;

namespace Autophobia.InvokeEvents
{
    public class InvokeUnityEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _canInvoke;
        [SerializeField] private bool _canInvokeOnce;

        [Space] [SerializeField] private bool _invokeOnStart;

        private void Start()
        {
            if(_invokeOnStart)
                InvokeEvent();
        }

        public void InvokeEvent()
        {
            if (!_canInvoke) return;

            _action?.Invoke();
            if(_canInvokeOnce) 
            { 
                _canInvoke = false; 
            }
        }

        public void ChangeBoolCanInvoke(bool canInvoke)
        {
            _canInvoke = canInvoke;
        }
    }
}