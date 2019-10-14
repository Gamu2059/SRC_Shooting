using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitSet
{
    public Transform Hit { get; private set; }
    public Transform Suffer { get; private set; }

    public HitSet(Transform hit, Transform suffer)
    {
        Hit = hit;
        Suffer = suffer;
    }
}
