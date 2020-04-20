#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルのx座標を抽出する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/Vec2X", fileName = "OperationFloatVec2X", order = 0)]
[System.Serializable]
public class OperationFloatVec2X : OperationFloatBase
{

    /// <summary>
    /// 2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override float GetResultFloat()
    {
        return m_Vector2.GetResultVector2().x;
    }
}





//Vector2 vec2 = m_Vector2.GetResultVector2();
//Debug.Log(vec2.x.ToString() + ", " + vec2.y.ToString());
