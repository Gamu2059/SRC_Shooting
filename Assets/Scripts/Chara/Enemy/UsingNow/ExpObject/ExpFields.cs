using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある単位弾幕のパラメータ全体。
/// </summary>
[System.Serializable]
public class ExpFields : object
{
    [SerializeField, Tooltip("boolパラメータ")]
    public ExpBool[] expBools;

    [SerializeField, Tooltip("intパラメータ")]
    public ExpInt[] expInts;

    [SerializeField, Tooltip("flaotパラメータ")]
    public ExpFloat[] expFloats;

    [SerializeField, Tooltip("Vector3パラメータ")]
    public ExpVector3[] expVector3s;


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


    public bool[] GetBoolArray()
    {
        bool[] boolArray = new bool[expBools.Length];

        for (int i = 0; i < expBools.Length; i++)
        {
            boolArray[i] = expBools[i].m_Bool;
        }
        return boolArray;
    }


    public int[] GetintArray()
    {
        int[] intArray = new int[expInts.Length];

        for (int i = 0; i < expInts.Length; i++)
        {
            intArray[i] = expInts[i].m_Int;
        }
        return intArray;
    }


    public float[] GetFloatArray()
    {
        float[] floatArray = new float[expFloats.Length];

        for (int i = 0; i < expFloats.Length; i++)
        {
            floatArray[i] = expFloats[i].m_Float;
        }
        return floatArray;
    }


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