using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bezier3Points : System.Object
{
    [SerializeField, Tooltip("第1制御点")]
    public Vector3 m_ControlPoint1;

    [SerializeField, Tooltip("第2制御点")]
    public Vector3 m_ControlPoint2;

    // 終点
    [SerializeField]
    public Vector3 m_EndPoint;


    [SerializeField, Tooltip("所要時間")]
    public float m_Time;


    //    [SerializeField, Tooltip("位置ベクトル")]
    //    public Vector3 m_Position;

    //    [SerializeField, Tooltip("速度ベクトル")]
    //    public Vector3 m_Velocity;
}
