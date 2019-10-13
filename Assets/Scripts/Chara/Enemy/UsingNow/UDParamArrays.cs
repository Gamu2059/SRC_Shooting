using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDParamArrays : object
{
    [SerializeField, Tooltip("bool型のパラメータの配列")]
    public bool[] m_BoolParams;

    [SerializeField, Tooltip("int型のパラメータの配列")]
    public int[] m_IntParams;

    [SerializeField, Tooltip("float型のパラメータの配列")]
    public float[] m_FloatParams;

    [SerializeField, Tooltip("Vector3型のパラメータの配列")]
    public Vector3[] m_Vector3Params;


    public UDParamArrays(bool[] boolParams, int[] intParams, float[] floatParams, Vector3[] Vector3Params)
    {
        m_BoolParams = boolParams;
        m_IntParams = intParams;
        m_FloatParams = floatParams;
        m_Vector3Params = Vector3Params;
    }
}
