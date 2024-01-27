using UnityEngine;

namespace Autophobia.PlayerComponents.Interact
{
    public class CheckObjectsInRay : MonoBehaviour
    {
        public GameObject Check(Vector3 position, Vector3 direction, float distance)
        {
            return Physics.Raycast(position, direction, out var hitInfo, distance) ? hitInfo.collider.gameObject : null;
        }
    }
}