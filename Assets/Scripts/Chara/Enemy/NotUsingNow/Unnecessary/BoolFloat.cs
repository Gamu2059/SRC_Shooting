using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolFloat : System.Object
{

    [SerializeField, Tooltip("bool型")]
    public bool m_Bool;

    [SerializeField, Tooltip("float型")]
    public float m_Float;
}