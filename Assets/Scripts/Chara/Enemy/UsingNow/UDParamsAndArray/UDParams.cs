using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 純粋な単位弾幕パラメータのクラス。
/// </summary>
public class UDParams : Object
{

    /// <summary>
    /// 単位弾幕の種類。
    /// </summary>
    private E_U_D m_EUD;

    /// <summary>
    /// bool型のパラメータの配列。
    /// </summary>
    private bool[] m_BoolParams;

    /// <summary>
    /// int型のパラメータの配列。
    /// </summary>
    private int[] m_IntParams;

    /// <summary>
    /// float型のパラメータの配列。
    /// </summary>
    private float[] m_FloatParams;

    /// <summary>
    /// Vector3型のパラメータの配列。
    /// </summary>
    private Vector3[] m_Vector3Params;


    /// <summary>
    /// 純粋な単位弾幕パラメータのインスタンスを生成する。
    /// </summary>
    public UDParams(E_U_D eUD,bool[] boolParams, int[] intParams, float[] floatParams, Vector3[] Vector3Params)
    {
        m_EUD = eUD;
        m_BoolParams = boolParams;
        m_IntParams = intParams;
        m_FloatParams = floatParams;
        m_Vector3Params = Vector3Params;
    }


    /// <summary>
    /// 単位弾幕の種類を取得する。
    /// </summary>
    public E_U_D GetEUD()
    {
        return m_EUD;
    }


    /// <summary>
    /// bool型のパラメータの配列を取得する。
    /// </summary>
    public bool[] GetBoolParams()
    {
        return m_BoolParams;
    }


    /// <summary>
    /// int型のパラメータの配列を取得する。
    /// </summary>
    public int[] GetIntParams()
    {
        return m_IntParams;
    }


    /// <summary>
    /// float型のパラメータの配列を取得する。
    /// </summary>
    public float[] GetFloatParams()
    {
        return m_FloatParams;
    }


    /// <summary>
    /// Vector3型のパラメータの配列を取得する。
    /// </summary>
    public Vector3[] GetVector3Params()
    {
        return m_Vector3Params;
    }
}




//[CreateAssetMenu(menuName = "Param/Danmaku/UD Params", fileName = "UD Params", order = 0)]


//public int GetNumParams(string typeString)
//{
//    switch (typeString)
//    {
//        case "int":
//            return m_IntParams.Length;
//    }

//    return m_FloatParams.Length;
//}


//public void SetParamsArray<T>(T[] uDParams)
//{
//    if (typeof(T) == typeof(int))
//    {
//        for (int i = 0; i < uDParams.Length; i++)
//        {
//            uDParams[i] = (T[])m_IntParams[i];
//        }
//    }
//}


////intパラメータを代入する
//public void SetIntParams(Dictionary<string, int> uDIntParams,params string[] keys)
//{
//    for (int i = 0;i < keys.Length;i++)
//    {
//        uDIntParams.Add(keys[i],m_IntParams[i]);
//    }
//}


////floatパラメータを代入する
//public void SetFloatParams(Dictionary<string, float> uDFloatParams, params string[] keys)
//{
//    for (int i = 0; i < keys.Length; i++)
//    {
//        uDFloatParams.Add(keys[i], m_FloatParams[i]);
//    }
//}


////Vector3パラメータを代入する
//public void SetVector3Params(Dictionary<string, Vector3> uDVector3Params, params string[] keys)
//{
//    for (int i = 0; i < keys.Length; i++)
//    {
//        uDVector3Params.Add(keys[i], m_Vector3Params[i]);
//    }
//}


////intパラメータの数を返す
//public int GetNumIntParams()
//{
//    return m_IntParams.Length;
//}


//public void SetIntParamsArray(int[] uDIntParams)
//{
//    for (int i = 0; i < uDIntParams.Length; i++)
//    {
//        uDIntParams[i] = m_IntParams[i];
//    }
//}


//public int GetNumFloatParams()
//{
//    return m_FloatParams.Length;
//}


//public void SetFloatParamsArray(float[] uDFloatParams)
//{
//    for (int i = 0; i < uDFloatParams.Length; i++)
//    {
//        uDFloatParams[i] = m_FloatParams[i];
//    }
//}


//public int GetNumVector3Params()
//{
//    return m_Vector3Params.Length;
//}


//public void SetVector3ParamsArray(Vector3[] uDVector3Params)
//{
//    for (int i = 0; i < uDVector3Params.Length; i++)
//    {
//        uDVector3Params[i] = m_Vector3Params[i];
//    }
//}


//public int GetNumBoolParams()
//{
//    return m_BoolParams.Length;
//}


//public void SetBoolParamsArray(bool[] uDBoolParams)
//{
//    for (int i = 0; i < uDBoolParams.Length; i++)
//    {
//        uDBoolParams[i] = m_BoolParams[i];
//    }
//}