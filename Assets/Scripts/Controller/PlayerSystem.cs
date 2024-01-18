using UnityEngine;

namespace Autophobia.PlayerComponents
{
    public class PlayerSystem : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Camera _visionCamera;

        private Rigidbody _rigidbody;
        private Vector2 _direction;

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
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
            Debug.DrawRay(_visionCamera.transform.position, _visionCamera.transform.forward * 3, Color.green);
#endif
        }
    }
}