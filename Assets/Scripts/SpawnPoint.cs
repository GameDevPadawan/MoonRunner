using System;
using System.Linq;
using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
    
    [Range(0f, 10f)]
    [SerializeField] private float lineLength;

    private Vector3[] _pathNodes;

    private void Start()
    {
        _pathNodes = gameObject.GetComponentsInChildren<WaypointNode>().Select(x => x.gameObject.transform.position).ToArray();
    }

    public Transform GetSpawnLocation()
    {
        return this.transform;
    }

    public Vector3[] GetPath()
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