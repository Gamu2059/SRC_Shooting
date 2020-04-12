#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルのy座標を抽出する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/Vec2Y", fileName = "OperationFloatVec2Y", order = 0)]
[System.Serializable]
public class OperationFloatVec2Y : OperationFloatBase
{

    /// <summary>
    /// 2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override float GetResultFloat()
    {
        return m_Vector2.GetResultVector2().y;
    }
}





//Vector2 vec2 = m_Vector2.GetResultVector2();
//Debug.Log(vec2.x.ToString() + ", " + vec2.y.ToString());
