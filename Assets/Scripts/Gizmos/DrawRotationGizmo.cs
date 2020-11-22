using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRotationGizmo : MonoBehaviour
{
    public bool DrawAlways = true;
    public bool DrawSelected;
    [Header("X = Red, Y = Green, Z = Blue")]
    public Vector3 LenthAlongEachAxis = new Vector3(1,1,1);

    private void OnDrawGizmos()
    {
        if (DrawAlways)
        {
            DrawVectors();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (DrawAlways || DrawSelected)
        {
            DrawVectors();
        }
    }

    private void DrawVectors()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * LenthAlongEachAxis.x);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.forward * LenthAlongEachAxis.x);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * LenthAlongEachAxis.y);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.up * LenthAlongEachAxis.y);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * LenthAlongEachAxis.z);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.right * LenthAlongEachAxis.z);
    }
}
