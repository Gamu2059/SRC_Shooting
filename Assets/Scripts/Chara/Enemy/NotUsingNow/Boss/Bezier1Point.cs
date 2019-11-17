using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bezier1Point : System.Object
{
    [SerializeField, Tooltip("このベジェ曲線の所要時間")]
    public float m_Time;

    [SerializeField, Tooltip("アンカーポイントの位置ベクトル")]
    public Vector3 m_AnchorPosition;

    [SerializeField, Tooltip("アンカーポイントでの速度ベクトル")]
    public Vector3 m_AnchorVelocity;
}
