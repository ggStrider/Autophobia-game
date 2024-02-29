using UnityEngine;

using Cinemachine;

namespace Autophobia.Animations
{
    public class PlayerCutSceneController : MonoBehaviour
    {
        [SerializeField] private bool _cutSceneOnStart;
        private GameObject _playerSystemObject;
        
        private Rigidbody _rigidbody;
        private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _cinemachineBrain = GetComponentInChildren<CinemachineBrain>();
            _playerSystemObject = GameObject.FindGameObjectWithTag("Player");
            
            if(!_cutSceneOnStart) return;
            OnCutSceneStarts();
        }

        private void OnCutSceneStarts()
        {
            _rigidbody.isKinematic = true;
            _cinemachineBrain.enabled = false;
        }

        private void OnCutSceneEnds()
        {
            _rigidbody.isKinematic = false;
            _cinemachineBrain.enabled = true;
        }

        private void OnCutSceneInThisPosition()
        {
            var offsetObjectPosition = transform.localPosition;
            var playerPosition = _playerSystemObject.transform.localPosition;

            var parent = new GameObject("TempForCutScene")
            {
                transform =
                {
                    position = offsetObjectPosition + playerPosition
                }
            };
            
            transform.parent = parent.transform;
            
            transform.localPosition = Vector3.zero;
            _playerSystemObject.transform.localPosition = Vector3.zero;
        }

        private void OnCurrentPositionCutSceneEnds()
        {
            var position = transform.parent.position;
            var parent = transform.parent;
            transform.parent = null;
            
            transform.localPosition = position;
            _playerSystemObject.transform.localPosition = Vector3.zero;
            
            Destroy(parent.gameObject);
        }
    }
}