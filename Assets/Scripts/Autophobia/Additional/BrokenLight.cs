using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Autophobia.Additional
{
    public class BrokenLight : MonoBehaviour
    {
        [SerializeField] private List<LightSettings> _lightSettings;
        private List<Coroutine> _activeCoroutines = new List<Coroutine>();

        private void Start()
        {
            for (var i = 0; i < _lightSettings.Count; i++)
            {
                _activeCoroutines.Add(null);
            }
        }

        public void Break(int lightIndex)
        {
            if (_activeCoroutines[lightIndex] != null) return;
            
            var currentLight = _lightSettings[lightIndex];
            currentLight.broken = true;
            currentLight.actionOnBreak.Invoke();
            
            _activeCoroutines[lightIndex] = StartCoroutine(FlickeringLight(currentLight, lightIndex));
        }

        private IEnumerator FlickeringLight(LightSettings currentLight, int coroutineIndex)
        {
            while (currentLight.broken)
            {
                currentLight.light.intensity = Random.Range(currentLight.minIntensity, currentLight.maxIntensity);
                yield return new WaitForSeconds(currentLight.delayBetweenFlickering);
                
                yield return null;
            }

            _activeCoroutines[coroutineIndex] = null;
        }

        public void BreakAllLights()
        {
            for (var i = 0; i < _lightSettings.Count; i++)
            {
                Break(i);
            }
        }

        public void FixLight(int lightIndex)
        {
            var currentLight = _lightSettings[lightIndex];
            currentLight.broken = false;
            currentLight.light.intensity = currentLight.fixedIntensity;
        }

        [Serializable]
        public class LightSettings
        {
            public Light light;
            public bool broken;

            [SerializeField] private float _minIntensity;
            public float minIntensity => _minIntensity;
            
            [SerializeField] private float _maxIntensity;
            public float maxIntensity => _maxIntensity;

            [SerializeField] private float _fixedIntensity;
            public float fixedIntensity => _fixedIntensity;
            
            [SerializeField] private float _delayBetweenFlickering;
            public float delayBetweenFlickering => _delayBetweenFlickering;

            [SerializeField] private UnityEvent _actionOnBreak;
            public UnityEvent actionOnBreak => _actionOnBreak;
        }
    }
}