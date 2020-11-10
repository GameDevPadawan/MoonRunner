using System;
using UnityEngine;
public class SpawnPoint : MonoBehaviour
{
    
    [Range(0f, 10f)]
    [SerializeField] private float lineLength;
    
    private void OnDrawGizmos() 
    {
        ShowRotationInEditor(Color.magenta);
    }

    private void ShowRotationInEditor(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, Vector3.forward * lineLength);
    }
}
