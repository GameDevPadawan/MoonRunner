using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCentroidGizmo : MonoBehaviour
{
    public bool DrawAlways = true;
    public bool DrawSelected = false;
    public Color ShapeColor = Color.magenta;
    public Vector3 ShapeSize = new Vector3(1,1,1);
    public Shapes Shape = Shapes.Cross;

    public enum Shapes
    {
        Cube,
        Cross,
        Sphere,
    }

    private void OnDrawGizmos()
    {
        if (DrawAlways)
        {
            DrawShape(Shape);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (DrawAlways || DrawSelected)
        {
            DrawShape(Shape);
        }
    }

    private void DrawShape(Shapes selectedShape)
    {
        switch (selectedShape)
        {
            case Shapes.Cube:
                DrawCube();
                break;
            case Shapes.Cross:
                DrawCross();
                break;
            case Shapes.Sphere:
                DrawSphere();
                break;
            default:
                break;
        }
    }

    private void DrawCube()
    {
        Gizmos.color = ShapeColor;
        Gizmos.DrawCube(transform.position, ShapeSize);
    }

    private void DrawCross()
    {
        // TODO refactor this to only draw one line offset to be in the center.
        //   At present this will draw twice the size of the other shapes.
        Gizmos.color = ShapeColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * ShapeSize.x);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.forward * ShapeSize.x);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * ShapeSize.y);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.up * ShapeSize.y);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * ShapeSize.z);
        Gizmos.DrawLine(transform.position, transform.position + -1 * transform.right * ShapeSize.z);
    }

    private void DrawSphere()
    {
        Gizmos.color = ShapeColor;
        Gizmos.DrawSphere(transform.position, ShapeSize.magnitude);
    }
}
