using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Kill();
    event EventHandler<GameObject> OnDeath;
}
