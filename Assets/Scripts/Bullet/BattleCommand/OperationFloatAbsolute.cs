#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// float値の絶対値を求める演算を表すクラス。（未使用）
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/absolute", fileName = "OperationFloatAbsolute", order = 0)]
[System.Serializable]
public class OperationFloatAbsolute : OperationFloatBase
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Float;


    public override float GetResultFloat()
    {
        return Mathf.Abs(m_Float.GetResultFloat());
    }
}