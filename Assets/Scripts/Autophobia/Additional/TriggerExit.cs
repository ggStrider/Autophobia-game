using UnityEngine;
using UnityEngine.Events;

namespace Autophobia.Additional
{
    public class TriggerExit : MonoBehaviour
    {
        [SerializeField] private string _tag = "Player";
        [SerializeField] private UnityEvent _action;

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_tag)) return;
            _action?.Invoke();
        }
    }
}