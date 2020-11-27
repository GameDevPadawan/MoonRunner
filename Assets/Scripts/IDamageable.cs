﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    bool IsValidTarget();
    TargetTypes GetTargetType();
}

public enum TargetTypes
{
    Enemy,
    Friendly,
}
