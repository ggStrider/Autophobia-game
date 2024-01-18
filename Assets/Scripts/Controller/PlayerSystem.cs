using UnityEngine;

using Autophobia.PlayerComponents.Interact;
using Autophobia.Events;

namespace Autophobia.PlayerComponents
{
    public class PlayerSystem : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rayCheckDistance;

        [SerializeField] private Camera _visionCamera;
        [SerializeField] private CheckObjectsInRay _checkInRay;

        [SerializeField] private bool _canInteract = true;

        private Rigidbody _rigidbody;
        private Vector2 _direction;

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Interact()
        {
            if (_canInteract)
            {
                var result = _checkInRay.Check(_visionCamera.transform.position,
                _visionCamera.transform.forward, _rayCheckDistance);

                var objectInvokeComponent = result?.GetComponent<InvokeUnityEvent>();
                objectInvokeComponent?.InvokeEvent();
            }
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            var cameraForward = _visionCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            var movementDirection = cameraForward * _direction.y + _visionCamera.transform.right * _direction.x;
            movementDirection.Normalize();

            _rigidbody.MovePosition(_rigidbody.position + movementDirection * _speed * Time.fixedDeltaTime);

#if UNITY_EDITOR
            Debug.DrawRay(_visionCamera.transform.position, _visionCamera.transform.forward * _rayCheckDistance, Color.green);
#endif
        }
    }
}