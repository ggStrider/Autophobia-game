using UnityEngine;

namespace Autophobia.Utilites
{
    public class AddSlipperyToChildrenColliders : MonoBehaviour
    {
        [SerializeField] private PhysicMaterial _slipperyMaterial;
        [SerializeField] private GameObject _parent;

        [ContextMenu("add")]
        public void MakeChildrenObjectsSlippery()
        {
            var children = _parent.GetComponentsInChildren<Collider>();
            foreach (var child in children)
            {
                child.material = _slipperyMaterial;
            }
        }
    }
}