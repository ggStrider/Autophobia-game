using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Autophobia.LevelDesign
{
    public class FloatingObjectMove : MonoBehaviour
    {
        [SerializeField] private List<FloatingObjectInfo> _floatingObjects;

        public void SetMinOffsetToCurrent(int index)
        {
            var currentInfo = _floatingObjects[index];
            currentInfo.minOffset = currentInfo.floatingObject.transform.position;
        }

#if UNITY_EDITOR
        [ContextMenu("Apply")]
        private void ApplyToAll()
        {
            var children = gameObject.GetComponentsInChildren<Transform>();
            var count = children.Length;

            print(count);
            for (var i = 0; i < count; i++)
            {
                _floatingObjects[i].floatingObject = children[i].gameObject;
            }
        }

        [ContextMenu("Add to vector")]
        private void AddDeltaToVector()
        {
            var maxDelta = new Vector3(1.3f,1.3f,1.3f);
            foreach (var floatingObjectInfo in _floatingObjects)
            {
                floatingObjectInfo.minOffset = floatingObjectInfo.floatingObject.transform.localPosition;

                floatingObjectInfo.maxOffset = floatingObjectInfo.minOffset;
                floatingObjectInfo.maxOffset += maxDelta;
            }
        }
#endif

        [ContextMenu("unit")]
        private void Unit()
        {
            for (var i = 0; i < _floatingObjects.Count; i++)
            {
                StartDoFloatingEffect(i);
            }
        }
        
        public void StartDoFloatingEffect(int index)
        {
            _floatingObjects[index].isFloating = true;
            StartCoroutine(DoFloatingEffect(index));
        }

        private IEnumerator DoFloatingEffect(int index)
        {
            var currentObject = _floatingObjects[index].floatingObject.transform;
            var elapsedTime = 0.0f;
            var updateTime = 0.15f;
            
            while (_floatingObjects[index].isFloating)
            {
                elapsedTime += Time.deltaTime;
                print(elapsedTime);

                if (elapsedTime >= updateTime)
                {
                    _floatingObjects[index].newPosition = GenerateNewPosition(index);
                    elapsedTime = 0; 
                }

                currentObject.localPosition = Vector3.Lerp(currentObject.localPosition,
                    _floatingObjects[index].newPosition, elapsedTime / 10);

                yield return null;
            }
        }

        private Vector3 GenerateNewPosition(int index)
        {
            var currentInfos = _floatingObjects[index];
            var x = Random.Range(currentInfos.minOffset.x, currentInfos.maxOffset.x);
            var y = Random.Range(currentInfos.minOffset.y, currentInfos.maxOffset.y);
            var z = Random.Range(currentInfos.minOffset.z, currentInfos.maxOffset.z);

            return new Vector3(x, y, z);
        }

        [Serializable]
        public class FloatingObjectInfo
        {
            public GameObject floatingObject;
            
            public Vector3 maxOffset;
            public Vector3 minOffset;
            
            public bool isFloating;

            public Vector3 newPosition;
        }
    }
}