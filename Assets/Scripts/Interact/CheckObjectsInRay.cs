using UnityEngine;

namespace Autophobia.PlayerComponents.Interact
{
    public class CheckObjectsInRay : MonoBehaviour
    {
        public GameObject Check(Vector3 position, Vector3 direction, float distance)
        {
            RaycastHit hitInfo;

            if(Physics.Raycast(position, direction, out hitInfo, distance))
                return hitInfo.collider.gameObject;

            return null;
        }
    }
}