using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDReference : System.Object
{

    [SerializeField, Tooltip("単位弾幕の種類")]
    public E_U_D m_EUD;

    [SerializeField, Tooltip("その種類の単位弾幕のインデックス")]
    public int m_Index;
}