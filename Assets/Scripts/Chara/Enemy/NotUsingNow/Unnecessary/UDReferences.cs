using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDReferences : System.Object
{

    [SerializeField, Tooltip("単位弾幕のインスタンスへの参照の配列")]
    public UDReference[] m_UDReference;
}