using UnityEngine;
using UnityEngine.Events;

using Autophobia.PlayerComponents;
using Autophobia.UIs;

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

        [SerializeField] private bool _fadeOnChangeState;
        [SerializeField] private Fader _fader;

        public async void Sit()
        {
            if (!_canSit) return;
            
            ChangePlayerSettings(true);
            _canSit = false;
            
            if (_fadeOnChangeState) await _fader.Fade();

            var placeWithOffset = _place.transform.position + _sitOffset;
            _player.position = placeWithOffset;

            _satEvent?.Invoke();
            await _fader.UnFade();
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

            Debug.Log(_player);
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