using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/Danmaku/UD Params Array Array", fileName = "UDPAA", order = 0)]
[System.Serializable]
public class UDParamsArrayArray : ScriptableObject
{

    [SerializeField, Tooltip("単位弾幕のパラメータの配列")]
    public UDParamsArray[] m_UDParametersArrayArray;
}