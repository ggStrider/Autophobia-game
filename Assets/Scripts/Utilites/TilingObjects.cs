using UnityEngine;

namespace Autophobia.Utilites
{
    public class TilingObjects : MonoBehaviour
    {
        [SerializeField] private Vector2 _tiling;
        
        private void Start()
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null) return;

            var material = meshRenderer.material;
            if (material == null) return;

            material.mainTextureScale = _tiling;
        }
    }
}