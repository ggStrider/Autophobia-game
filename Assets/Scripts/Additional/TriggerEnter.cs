using UnityEngine;
using UnityEngine.Events;

namespace Autophobia.Additional
{
    public class TriggerEnter : MonoBehaviour
    {
        [SerializeField] private string _tag = "Player";
        [SerializeField] private UnityEvent _action;
        [SerializeField] private bool _deactiavateOnEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_tag)) return;
            _action.Invoke();

            if (!_deactiavateOnEnter) return;
            var checkCollider = gameObject.GetComponent<Collider>();
            checkCollider.enabled = false;
        }
    }
}