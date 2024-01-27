using Autophobia.Dialogues;
using Autophobia.Events;
using Autophobia.Interact.Features;
using Autophobia.PlayerComponents.Interact;
using UnityEngine;
using Cinemachine;

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
        private CinemachinePOV _playerCinemachinePov;
        private Rigidbody _rigidbody;
        private Vector2 _direction;
        private DialogueTalk _dialogue;

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Interact()
        {
            if (!_canInteract) return;

            var playerCamera = _visionCamera.transform;
            var result = _checkInRay.Check(playerCamera.position,
                playerCamera.forward, _rayCheckDistance);

            if (result == null) return;
            var objectInvokeComponent = result.GetComponent<InvokeUnityEvent>();

            if (objectInvokeComponent == null) return;
            objectInvokeComponent.InvokeEvent();
        }

        public void GetCurrentSitComponent(SitOnObject currentSitComponent)
        {
            _currentSitting = currentSitComponent;
        }

        public void GetCurrentDialogue(DialogueTalk dialogue)
        {
            _dialogue = dialogue;
        }

        public void OnRebuildDialogue()
        {
            if (_dialogue == null) return;
            _dialogue.ReBuildDialogue();
        }

        public void OnGetUp()
        {
            if (_currentSitting != null)
                _currentSitting.GetUp();
        }

        public void SetRestrictXPositionCameraAngle(Vector2 maxAngles)
        {
            _playerCinemachinePov.m_HorizontalAxis.m_MinValue = maxAngles.x;
            _playerCinemachinePov.m_HorizontalAxis.m_MinValue = maxAngles.y;
        }

        public void SetCameraRotation(Vector2 lookAngle)
        {
            _playerCinemachinePov.m_HorizontalAxis.Value = lookAngle.x;
            _playerCinemachinePov.m_VerticalAxis.Value = lookAngle.y;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _playerCinemachinePov = _playerVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
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
            Debug.DrawRay(_visionCamera.transform.position, _visionCamera.transform.forward * _rayCheckDistance,
                Color.green);
#endif
        }
    }
}