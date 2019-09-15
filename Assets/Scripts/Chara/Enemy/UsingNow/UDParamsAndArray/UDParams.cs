using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/Danmaku/UD Params", fileName = "UD Params", order = 0)]
[System.Serializable]
public class UDParams : ScriptableObject
{

    [SerializeField, Tooltip("単位弾幕の種類")]
    public E_U_D m_EUD;


    [SerializeField, Tooltip("bool型のパラメータの配列")]
    public bool[] m_BoolParams;

    [SerializeField, Tooltip("int型のパラメータの配列")]
    public int[] m_IntParams;

    [SerializeField, Tooltip("float型のパラメータの配列")]
    public float[] m_FloatParams;

    [SerializeField, Tooltip("Vector3型のパラメータの配列")]
    public Vector3[] m_Vector3Params;


    //intパラメータを代入する
    public void SetIntParams(Dictionary<string, int> uDIntParams,params string[] keys)
    {
        for (int i = 0;i < keys.Length;i++)
        {
            uDIntParams.Add(keys[i],m_IntParams[i]);
        }
    }


    //floatパラメータを代入する
    public void SetFloatParams(Dictionary<string, float> uDFloatParams, params string[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            uDFloatParams.Add(keys[i], m_FloatParams[i]);
        }
    }


    //Vector3パラメータを代入する
    public void SetVector3Params(Dictionary<string, Vector3> uDVector3Params, params string[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            uDVector3Params.Add(keys[i], m_Vector3Params[i]);
        }
    }


    //intパラメータの数を返す
    public int GetNumIntParams()
    {
        return m_IntParams.Length;
    }


    public void SetIntParamsArray(int[] uDIntParams)
    {
        for (int i = 0; i < uDIntParams.Length; i++)
        {
            uDIntParams[i] = m_IntParams[i];
        }
    }


    public int GetNumFloatParams()
    {
        return m_FloatParams.Length;
    }


    public void SetFloatParamsArray(float[] uDFloatParams)
    {
        for (int i = 0; i < uDFloatParams.Length; i++)
        {
            uDFloatParams[i] = m_FloatParams[i];
        }
    }


    public int GetNumVector3Params()
    {
        return m_Vector3Params.Length;
    }


    public void SetVector3ParamsArray(Vector3[] uDVector3Params)
    {
        for (int i = 0; i < uDVector3Params.Length; i++)
        {
            uDVector3Params[i] = m_Vector3Params[i];
        }
    }


    public int GetNumBoolParams()
    {
        return m_BoolParams.Length;
    }


    public void SetBoolParamsArray(bool[] uDBoolParams)
    {
        for (int i = 0; i < uDBoolParams.Length; i++)
        {
            uDBoolParams[i] = m_BoolParams[i];
        }
    }
}




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