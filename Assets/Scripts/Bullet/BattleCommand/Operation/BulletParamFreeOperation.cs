#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾が持つ、bool,int,float,Vector2型のパラメータの演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/bulletParamFree", fileName = "BulletParamFree", order = 0)]
[System.Serializable]
public class BulletParamFreeOperation : ScriptableObject
{
    /// <summary>
    /// bool型の配列
    /// </summary>
    [SerializeField]
    private OperationBoolBase[] m_Bool;

    /// <summary>
    /// int型の配列
    /// </summary>
    [SerializeField]
    private OperationIntBase[] m_Int;

    /// <summary>
    /// float型の配列
    /// </summary>
    [SerializeField]
    private OperationFloatBase[] m_Float;

    /// <summary>
    /// Vector2型の配列
    /// </summary>
    [SerializeField]
    private OperationVector2Base[] m_Vector2;


    /// <summary>
    /// 演算結果を取得する
    /// </summary>
    public BulletParamFree GetResultBulletParamFree()
    {
        if (m_Bool == null) Debug.Log("m_Bool");

        bool[] boolArray = new bool[m_Bool.Length];

        for (int i = 0;i < m_Bool.Length;i++)
        {
            boolArray[i] = m_Bool[i].GetResultBool();
        }

        int[] intArray = new int[m_Int.Length];

        for (int i = 0; i < m_Int.Length; i++)
        {
            intArray[i] = m_Int[i].GetResultInt();
        }

        float[] floatArray = new float[m_Float.Length];

        for (int i = 0; i < m_Float.Length; i++)
        {
            floatArray[i] = m_Float[i].GetResultFloat();
        }

        Vector2[] vector2Array = new Vector2[m_Vector2.Length];

        for (int i = 0; i < m_Vector2.Length; i++)
        {
            vector2Array[i] = m_Vector2[i].GetResultVector2();
        }

        return new BulletParamFree(boolArray, intArray, floatArray, vector2Array);
    }
}
