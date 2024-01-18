using UnityEngine;
using UnityEngine.InputSystem;

namespace Autophobia.PlayerComponents
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private PlayerSystem _playerSystem;
        private PlayerMap _playerMap;

        private void Awake()
        {
            _playerMap = new PlayerMap();

            _playerMap.PlayerActionMap.Movement.performed += OnMovement;
            _playerMap.PlayerActionMap.Movement.canceled += OnMovement;

            _playerMap.PlayerActionMap.Interact.started += OnInteract;

            _playerMap.Enable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _playerSystem.SetDirection(direction);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            _playerSystem.Interact();
        }

        public void SetLock(bool isLocked)
        {
            if (isLocked)
            {
                _playerMap.Disable();
            }

            else
            {
                _playerMap.Enable();
            }
        }
    }
}