using UnityEngine;

public class MakeChildrenObjectSizeRandom : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject;

    [Space] [SerializeField] private Vector3 _minSize;
    [SerializeField] private Vector3 _maxSize;
    
    [ContextMenu("Make scale random")]
    private void MakeObjectsScaleRandom()
    {
        var children = _parentObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.localScale = GenerateValues();
        }
    }

    private Vector3 GenerateValues()
    {
        var x = Random.Range(_minSize.x, _maxSize.x);
        var y = Random.Range(_minSize.y, _maxSize.y);
        var z = Random.Range(_minSize.z, _maxSize.z);

        return new Vector3(x, y, z);
    }
}
