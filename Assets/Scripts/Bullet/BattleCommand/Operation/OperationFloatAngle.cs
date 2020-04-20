#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2次元ベクトルの偏角を求める演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/angle", fileName = "OperationFloatAngle", order = 0)]
[System.Serializable]
public class OperationFloatAngle : OperationFloatBase
{

    /// <summary>
    /// 偏角を計算する2次元ベクトル
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Vector2;


    public override float GetResultFloat()
    {
        return Mathf.Atan2(m_Vector2.GetResultVector2().y, m_Vector2.GetResultVector2().x);
    }
}





//Vector2 vec2 = m_Vector2.GetResultVector2();
//Debug.Log(vec2.x.ToString() + ", " + vec2.y.ToString());
