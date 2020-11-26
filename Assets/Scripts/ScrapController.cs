using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapController : MonoBehaviour, ICollectable
{
    public int Value;

    int ICollectable.Collect()
    {
        Destroy(this.gameObject);
        return Value;
    }
}
