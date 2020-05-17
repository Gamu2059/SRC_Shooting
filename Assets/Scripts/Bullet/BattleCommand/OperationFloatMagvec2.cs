#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルの長さを求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/magvec2", fileName = "OperationFloatMagvec2", order = 0)]
[System.Serializable]
public class OperationFloatMagvec2 : OperationFloatBase
{

    /// <summary>
    /// 長さを計算する2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override float GetResultFloat()
    {
        return m_Vector2.GetResultVector2().magnitude;
    }
}