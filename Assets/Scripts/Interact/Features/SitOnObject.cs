using Autophobia.PlayerComponents;
using UnityEngine;
using UnityEngine.Events;

namespace Autophobia.Interact.Features
{
    public class SitOnObject : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        [Space] [SerializeField] private Transform _place;
        [SerializeField] private Vector3 _sitOffset;

        [Space(5)] [SerializeField] private Transform _getUpPlace;
        [SerializeField] private Vector3 _getUpOffset;

        [Space] [SerializeField] private bool _canSit;
        [SerializeField] private bool _canGetUp;

        [Space] [SerializeField] private UnityEvent _satEvent;
        [SerializeField] private UnityEvent _gotUpEvent;

        public void Sit()
        {
            if (!_canSit) return;


            _canSit = false;

            ChangePlayerSettings(true);

            var placeWithOffset = _place.transform.position + _sitOffset;
            _player.position = placeWithOffset;

            _satEvent?.Invoke();
        }

        public void GetUp()
        {
            if (!_canGetUp) return;

            _canGetUp = false;

            ChangePlayerSettings(false);

            var getUpPlaceWithOffset = _getUpPlace.transform.position + _getUpOffset;
            _player.position = getUpPlaceWithOffset;

            _gotUpEvent?.Invoke();
        }

        private void ChangePlayerSettings(bool isSitting)
        {
            _canGetUp = false;
            
            var playerMap = _player.GetComponent<PlayerInputReader>();
            playerMap.RestrictMainMove(isSitting);

            var playerRigidbody = _player.GetComponent<Rigidbody>();
            playerRigidbody.isKinematic = isSitting;

            var playerSystem = _player.GetComponent<PlayerSystem>();
            playerSystem.GetCurrentSitComponent(isSitting ? this : null);
            
            playerSystem.SetDirection(Vector2.zero);
        }

        public void CanGetUp(bool canGetUp)
        {
            _canGetUp = canGetUp;
        }

        public void CanSit(bool canSit)
        {
            _canSit = canSit;
        }
    }
}