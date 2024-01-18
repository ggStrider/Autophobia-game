using UnityEngine;
using UnityEngine.Events;

namespace Autophobia.Events
{
    public class InvokeUnityEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _canInvoke;
        [SerializeField] private bool _canInvokeOnce;

        public void InvokeEvent()
        {
            if (_canInvoke)
            {
                _action?.Invoke();
                
                if(_canInvokeOnce)
                {
                    _canInvoke = false;
                }
            }
        }

        public void ChangeBoolCanInvoke(bool canInvoke)
        {
            _canInvoke = canInvoke;
        }
    }
}