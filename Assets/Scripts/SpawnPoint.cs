using System;
using UnityEngine;
[ExecuteInEditMode]
public class SpawnPoint : MonoBehaviour
{
    [Range(0f, 10f)]
    [SerializeField] private float lineLength;

    [SerializeField] private Color lineColor;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;
        Gizmos.DrawLine(transform.position, Vector3.forward * lineLength);
    }
}
