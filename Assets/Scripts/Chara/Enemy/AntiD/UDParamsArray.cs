using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDParamsArray : System.Object
{

    [SerializeField, Tooltip("単位弾幕のパラメータの配列")]
    public UDParams[] m_UDParametersArray;
}