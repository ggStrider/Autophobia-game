using UnityEngine;

using Autophobia.Dialogues;
using Autophobia.InvokeEvents;
using Autophobia.Interact.Features;
using Autophobia.PlayerComponents.Interact;

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

        private Rigidbody _rigidbody;
        private CinemachinePOV _playerCinemachinePov;
        private CinemachineBrain _cinemachineBrain;
        private SitOnObject _currentSitting;
        private DialogueTalk _dialogue;
        
        private Vector2 _direction;

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

        public void SetRestrictXPositionCameraAngle(Vector2 minAngles, Vector2 maxAngles, bool wrapped)
        {
            _playerCinemachinePov.m_HorizontalAxis.m_MinValue = minAngles.x;
            _playerCinemachinePov.m_VerticalAxis.m_MinValue = minAngles.y;
            
            _playerCinemachinePov.m_HorizontalAxis.m_MaxValue = maxAngles.x;
            _playerCinemachinePov.m_VerticalAxis.m_MaxValue = maxAngles.y;

            _playerCinemachinePov.m_HorizontalAxis.m_Wrap = wrapped;
        }

        public void SetCameraRotation(Vector2 lookAngle)
        {
            _playerCinemachinePov.m_HorizontalAxis.Value = lookAngle.x;
            _playerCinemachinePov.m_VerticalAxis.Value = lookAngle.y;
        }

        public void CanCameraRotate(bool canRotate)
        {
            _cinemachineBrain.enabled = canRotate;
        }

        public void CanInteract(bool canInteract)
        {
            _canInteract = canInteract;
        }

        public void OnCutSceneStarts(bool t)
        {
            _cinemachineBrain.enabled = false;
            _rigidbody.isKinematic = true;
        }

        public void OnCutSceneEnds()
        {
            _cinemachineBrain.enabled = true;
            _rigidbody.isKinematic = false;
        }

        private void Awake()
        {
            Debug.Log("TimeScale = " + Time.timeScale);
            _rigidbody = GetComponent<Rigidbody>();
            _playerCinemachinePov = _playerVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

            _cinemachineBrain = _visionCamera.GetComponent<CinemachineBrain>();
            CursorChangeState.SetState(true, false);
        }

        private void FixedUpdate()
        {
            var cameraForward = _visionCamera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            var movementDirection = cameraForward * _direction.y + _visionCamera.transform.right * _direction.x;
            movementDirection.Normalize();

            var velocity = movementDirection * _speed;
            _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);

#if UNITY_EDITOR
            Debug.DrawRay(_visionCamera.transform.position, _visionCamera.transform.forward * _rayCheckDistance,
                Color.green);
#endif
        }
    }
}