using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnterable
{
    void Enter(GameObject passenger);

    void Exit(GameObject passenger);
}
