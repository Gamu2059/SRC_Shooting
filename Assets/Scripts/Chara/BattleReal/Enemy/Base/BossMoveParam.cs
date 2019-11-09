using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossMoveParam
{
    public enum E_COORD{
        ABSOLUTE,
        RELATIVE,
    }

    public Vector3 Destination;

    public E_COORD Coordinate;

    public float NextMoveWaitTime;

    public float MoveDuration;
}
