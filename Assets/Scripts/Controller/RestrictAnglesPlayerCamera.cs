using UnityEngine;

namespace Autophobia.PlayerComponents
{
    public class RestrictAnglesPlayerCamera : MonoBehaviour
    {
        [SerializeField] private Vector2 _minAngles;
        [SerializeField] private Vector2 _maxAngles;
        [SerializeField] private bool _wrapped;

        private PlayerSystem _playerSystem;

        private void Awake()
        {
            _playerSystem = FindObjectOfType<PlayerSystem>();
        }

        [ContextMenu("Restrict")]
        public void OnRestrict()
        {
            if (_playerSystem == null) return;
            _playerSystem.SetRestrictXPositionCameraAngle(_minAngles, _maxAngles, _wrapped);
        }
    }
}