using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathDrawer : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        List<Transform> pathNodes = pathNodes = new List<Transform>();
        foreach (Transform child in this.transform) pathNodes.Add(child);
        for (int index = 1; index < pathNodes.Count; index++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pathNodes[index - 1].position, pathNodes[index].position);
        }
    }
}
