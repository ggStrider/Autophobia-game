using UnityEngine;

namespace Autophobia.Utilities
{
    public class AddParentToObject : MonoBehaviour
    {
        [ContextMenu("Set parent")]
        private void SetParent()
        {
            var all = gameObject.GetComponentsInChildren<Transform>();
            foreach (var one in all)
            {
                var parentName = one.name + "Parent";
                var parent = new GameObject(parentName);
                
                one.transform.parent = parent.transform;
                parent.transform.parent = gameObject.transform;
            }
        }

        [ContextMenu("Set")]
        private void SetTransformPosition()
        {
            var childCount = transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                Debug.Log(child);

                var childOfChild = child.transform.GetChild(0);
                child.position = childOfChild.position;

                childOfChild.transform.position = new Vector3(0, 0, 0);
                Debug.Log(childOfChild);
            }
        }
    }
}