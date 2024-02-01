using UnityEngine.Events;
using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using System;

namespace Autophobia.InvokeEvents
{
    public class InvokeDelayedEvents : MonoBehaviour
    {
        [SerializeField] private List<DelayedEvent> _delayedEvent = new List<DelayedEvent>();

        [Space] [SerializeField] private bool _invokeOnStart;
        
        private const byte _minimumTimersArrayToReset = 1;

        private void Start()
        {
            if(_invokeOnStart)
                OnInvokeEvents();
        }

        public void OnInvokeEvents()
        {
            ResetTimers();
            StartCoroutine(InvokeEvents());
        }

        private IEnumerator InvokeEvents()
        {
            foreach (var delayedEventOne in _delayedEvent)
            {
                yield return new WaitForSeconds(delayedEventOne.delay);
                delayedEventOne.action.Invoke();
            }
        }

        private void ResetTimers()
        {
            if (_delayedEvent.Count < _minimumTimersArrayToReset) return;
            
            for (var i = 1; i < _delayedEvent.Count; i++)
            {
                if (!_delayedEvent[i].isTimersConsistent || _delayedEvent[i].previousTimerIsCounted) continue;
                
                _delayedEvent[i].previousTimerIsCounted = true;
                _delayedEvent[i].delay += _delayedEvent[i - 1].delay;
            }
        }

        [Serializable]
        public class DelayedEvent
        {
            public UnityEvent action;

            // Якщо isTimerConsistent то delay зміниться враховуючи
            // попередній таймер зі списку _delayedEvent
            public float delay;
            public bool isTimersConsistent;

            [HideInInspector]
            // Перевірка чи врахувався час попереднього таймеру
            public bool previousTimerIsCounted;
        }
    }
}