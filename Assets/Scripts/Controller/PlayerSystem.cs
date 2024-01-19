using UnityEngine;
using Cinemachine;

using Autophobia.PlayerComponents.Interact;
using Autophobia.Events;
using Autophobia.Interact.Features;

namespace Autophobia.PlayerComponents
{
    public class PlayerSystem : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rayCheckDistance;

        [SerializeField] private Camera _visionCamera;
        [SerializeField] private CinemachineVirtualCamera _playerVirtualCamera;
        [SerializeField] private CheckObjectsInRay _checkInRay;

        [SerializeField] private bool _canInteract = true;

        private SitOnObject _currentSitting;
        private CinemachinePOV _cinemachinePOV;
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

        public void GetCurrentSitComponent(SitOnObject currentSitComponent)
        {
            _currentSitting = currentSitComponent;
        }

        public void OnGetUp()
        {
            if(_currentSitting != null)
                _currentSitting.GetUp();
        }

        public void SetRestrictXPositionCameraAngle(Vector2 maxAngles)
        {
            _cinemachinePOV.m_HorizontalAxis.m_MinValue = maxAngles.x;
            _cinemachinePOV.m_HorizontalAxis.m_MinValue = maxAngles.y;
        }

        public void SetCameraRotation(Vector2 lookAngle)
        {
            _cinemachinePOV.m_HorizontalAxis.Value = lookAngle.x;
            _cinemachinePOV.m_VerticalAxis.Value = lookAngle.y;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _cinemachinePOV = _playerVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
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