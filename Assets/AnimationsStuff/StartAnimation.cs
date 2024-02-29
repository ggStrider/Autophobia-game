using UnityEngine;

namespace AnimationsStuff
{
    public class StartAnimation : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int FloatingKey = Animator.StringToHash("floating");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        [ContextMenu("Floating")]
        public void OnStartFloating()
        {
            _animator.SetBool(FloatingKey, true);
        }
    }
}