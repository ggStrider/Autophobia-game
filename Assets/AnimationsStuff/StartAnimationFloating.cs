using System;
using UnityEngine;

namespace AnimationsStuff
{
    public class StartAnimationFloating : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int FloatingKey = Animator.StringToHash("floating");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        [ContextMenu("Floating")]
        public void OnStartAnimation()
        {
            _animator.SetBool(FloatingKey, true);
        }
    }
}