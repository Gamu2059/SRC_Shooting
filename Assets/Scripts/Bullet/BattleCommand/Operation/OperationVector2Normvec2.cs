#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルを正規化した値を演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/normvec2", fileName = "OperationVector2Normvec2", order = 0)]
[System.Serializable]
public class OperationVector2Normvec2 : OperationVector2Base
{

    /// <summary>
    /// 正規化した値を求めるベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override Vector2 GetResultVector2()
    {
        return m_Vector2.GetResultVector2().normalized;
    }
}