using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoScript : MonoBehaviour
{
    public Color color1;

    public bool Cube, wireCube, wireSphere;

    public float size = 1;
    

    private void OnDrawGizmos()
    {
        Gizmos.color = color1;
        if (Cube == true)
        {
            Gizmos.DrawCube(transform.position, new Vector3(size,size,size));
        }
        if (wireCube == true)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(size, size, size));
        }
        if (wireSphere == true)
        {
            Gizmos.DrawWireSphere(transform.position, size);
        }
    
    }
}
