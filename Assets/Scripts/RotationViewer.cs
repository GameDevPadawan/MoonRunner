using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationViewer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        DrawAxis(transform.forward, Color.blue);
        DrawAxis(transform.right, Color.red);
        DrawAxis(transform.up, Color.green);
    }

    private void DrawAxis(Vector3 axis, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position, transform.position + axis * 10);
    }
}
