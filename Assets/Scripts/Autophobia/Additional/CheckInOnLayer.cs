using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Autophobia.Additional
{
    public class CheckInOnLayer : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerToCheck;
        [SerializeField] private float _radius;
        
        [SerializeField] private bool _isOnLayer;
        public bool IsColliderOnLayer => _isOnLayer;

        [FormerlySerializedAs("_eventOnTrue")] public UnityEvent EventOnTrue;
        [FormerlySerializedAs("_eventOnFalse")] public UnityEvent EventOnFalse;

        private void OnTriggerStay(Collider other)
        {
            _isOnLayer = Physics.CheckSphere(transform.position, _radius, _layerToCheck);
            EventOnTrue?.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            _isOnLayer = Physics.CheckSphere(transform.position, _radius, _layerToCheck);
            EventOnFalse?.Invoke();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _radius);
        }
#endif
    }
}