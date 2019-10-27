using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アセット用単位弾幕パラメータのパラメータ配列のクラス。
/// </summary>
[System.Serializable]
public class ExpFields : object
{
    /// <summary>
    /// 説明付きのbool型のパラメータの配列。
    /// </summary>
    [SerializeField, Tooltip("説明付きのbool型のパラメータの配列。")]
    public ExpBool[] expBools;

    /// <summary>
    /// 説明付きのint型のパラメータの配列。
    /// </summary>
    [SerializeField, Tooltip("説明付きのint型のパラメータの配列。")]
    public ExpInt[] expInts;

    /// <summary>
    /// 説明付きのfloat型のパラメータの配列。
    /// </summary>
    [SerializeField, Tooltip("説明付きのfloat型のパラメータの配列。")]
    public ExpFloat[] expFloats;

    /// <summary>
    /// 説明付きのVector3型のパラメータの配列。
    /// </summary>
    [SerializeField, Tooltip("説明付きのVector3型のパラメータの配列。")]
    public ExpVector3[] expVector3s;


    /// <summary>
    /// それぞれの型の各インデックスの説明の配列から、説明付きの単位弾幕パラメータのパラメータ配列を生成する。
    /// </summary>
    /// <param name="boolExps"   >説明付きのbool型パラメータの配列</param>
    /// <param name="intExps"    >説明付きのint型パラメータの配列</param>
    /// <param name="floatExps"  >説明付きのfloat型パラメータの配列</param>
    /// <param name="Vector3Exps">説明付きのVector3型パラメータの配列</param>
    public ExpFields(string[] boolExps, string[] intExps, string[] floatExps, string[] Vector3Exps)
    {
        expBools = new ExpBool[boolExps.Length];
        expInts = new ExpInt[intExps.Length];
        expFloats = new ExpFloat[floatExps.Length];
        expVector3s = new ExpVector3[Vector3Exps.Length];

        for (int i = 0;i < boolExps.Length;i++)
        {
            expBools[i] = new ExpBool(boolExps[i]);
        }

        for (int i = 0; i < intExps.Length; i++)
        {
            expInts[i] = new ExpInt(intExps[i]);
        }

        for (int i = 0; i < floatExps.Length; i++)
        {
            expFloats[i] = new ExpFloat(floatExps[i]);
        }

        for (int i = 0; i < Vector3Exps.Length; i++)
        {
            expVector3s[i] = new ExpVector3(Vector3Exps[i]);
        }
    }


    /// <summary>
    /// bool型のパラメータのみの配列を取得する。
    /// </summary>
    public bool[] GetBoolArray()
    {
        bool[] boolArray = new bool[expBools.Length];

        for (int i = 0; i < expBools.Length; i++)
        {
            boolArray[i] = expBools[i].m_Bool;
        }
        return boolArray;
    }


    /// <summary>
    /// int型のパラメータのみの配列を取得する。
    /// </summary>
    public int[] GetintArray()
    {
        int[] intArray = new int[expInts.Length];

        for (int i = 0; i < expInts.Length; i++)
        {
            intArray[i] = expInts[i].m_Int;
        }
        return intArray;
    }


    /// <summary>
    /// float型のパラメータのみの配列を取得する。
    /// </summary>
    public float[] GetFloatArray()
    {
        float[] floatArray = new float[expFloats.Length];

        for (int i = 0; i < expFloats.Length; i++)
        {
            floatArray[i] = expFloats[i].m_Float;
        }
        return floatArray;
    }


    /// <summary>
    /// Vector3型のパラメータのみの配列を取得する。
    /// </summary>
    public Vector3[] GetVector3Array()
    {
        Vector3[] Vector3Array = new Vector3[expVector3s.Length];

        for (int i = 0; i < expVector3s.Length; i++)
        {
            Vector3Array[i] = expVector3s[i].m_Vector3;
        }
        return Vector3Array;
    }
}