using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/Danmaku/UD Params Array", fileName = "UDPA", order = 0)]
[System.Serializable]
public class UDParamsArray : ScriptableObject
{

    [SerializeField, Tooltip("単位弾幕のパラメータの配列")]
    public UDParams[] m_UDParametersArray;

    [SerializeField, Tooltip("形態変化するかどうか")]
    public bool m_IsFormChangable;
}