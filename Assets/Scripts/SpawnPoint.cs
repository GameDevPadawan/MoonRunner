using System;
using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
    
    [Range(0f, 10f)]
    [SerializeField] private float lineLength;

    private Transform[] _pathNodes;

    private void Start()
    {
        _pathNodes = gameObject.GetComponentsInChildren<Transform>();
    }

    public Transform[] GetPath()
    {
        return _pathNodes;
    }

    #region Gizmos
    private void OnDrawGizmos() 
    {
        ShowRotationInEditor(Color.magenta);
    }

    private void ShowRotationInEditor(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, Vector3.forward * lineLength);
    }
    #endregion
}