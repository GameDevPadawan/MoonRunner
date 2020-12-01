using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSpawnEffectToSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform startPoint;
    private Vector3 them;
    [SerializeField]
    private GameObject[] spawnPoints;
    //private GameObject selectedSpawnPoint;
    [SerializeField]
    private int spawnIndex;
    [SerializeField]
    private GameObject spawnSphere;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        them = spawnPoints[spawnIndex].transform.position;
        spawnSphere.transform.position = them;
        Vector3 midpoint = (them + startPoint.position) / 2;
        this.transform.position = midpoint;
        transform.LookAt(startPoint);
        transform.Rotate(new Vector3(1, 0, 0), 90);
        float yScale = Vector3.Distance(startPoint.position, them) / 2;
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPoint.position, them);
    }
}
