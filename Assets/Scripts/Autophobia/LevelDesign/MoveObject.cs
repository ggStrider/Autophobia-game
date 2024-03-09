using UnityEngine;
using Autophobia.PlayerComponents;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Autophobia.LevelDesign
{
    public class MoveObject : MonoBehaviour
    {
        [SerializeField] private List<MoveInfo> _moveInfos; 
        private CameraShake _cameraShake;

        private void Start()
        {
            _cameraShake = FindObjectOfType<CameraShake>();
        }
        
        public void OnMove(int index)
        {
            StartCoroutine(Move(index));
        }

        private IEnumerator Move(int index)
        {
            var currentInfo = _moveInfos[index];

            if(currentInfo.shakeOnMoving)
                _cameraShake.Shake(true);
            
            var newPosition = currentInfo.objectToMove.transform.position + currentInfo.moveDelta;
            var elapsedTime = 0.0f;
            
            while (elapsedTime < currentInfo.waitDelta)
            {
                currentInfo.objectToMove.transform.position = Vector3.Lerp(currentInfo.objectToMove.transform.position, newPosition,
                    elapsedTime / currentInfo.speedDelta);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            currentInfo.objectToMove.transform.position = newPosition;
            
            currentInfo.movedEvent?.Invoke();
            
            if(currentInfo.shakeOnMoving)
                _cameraShake.Shake(false);
        }

        [Serializable]
        public class MoveInfo
        {
            public GameObject objectToMove;
            
            [SerializeField] private Vector3 _moveDelta;
            public Vector3 moveDelta => _moveDelta;

            [SerializeField] private float _speedDelta;
            public float speedDelta => _speedDelta;
            
            [SerializeField] private float _waitDelta;
            public float waitDelta => _waitDelta;

            [SerializeField] private UnityEvent _movedEvent;
            public UnityEvent movedEvent => _movedEvent;

            [SerializeField] private bool _shakeOnMoving;
            public bool shakeOnMoving => _shakeOnMoving;
        }
    }
}