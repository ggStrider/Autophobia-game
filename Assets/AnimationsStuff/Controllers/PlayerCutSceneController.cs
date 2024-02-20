using UnityEngine;

using Cinemachine;

namespace Autophobia.Animations
{
    public class PlayerCutSceneController : MonoBehaviour
    {
        [SerializeField] private bool _cutSceneOnStart;
        
        private Rigidbody _rigidbody;
        private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
            _cinemachineBrain = GetComponentInChildren<CinemachineBrain>();
            
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
    }
}