using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 線形補間する演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/linearinterpo", fileName = "OperationFloatLinearinterpo", order = 0)]
[System.Serializable]
public class OperationFloatLinearinterpo : OperationFloatBase
{

    /// <summary>
    /// 独立変数
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_T;

    /// <summary>
    /// 独立変数の最小値より小さい範囲での傾き（nullなら隣の範囲と同じ傾き）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_MinSlope;

    /// <summary>
    /// 独立変数の最小値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_TMin;

    /// <summary>
    /// 値と、値同士の隙間の長さを表すオブジェクトの配列
    /// </summary>
    [SerializeField]
    private ValueGapFloatFloat[] m_ValueGapArray;

    /// <summary>
    /// 最後の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_LastValue;

    /// <summary>
    /// 独立変数の最大値より大きい範囲での傾き（nullなら隣の範囲と同じ傾き）
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_MaxSlope;


    public override float GetResultFloat()
    {
        // 独立変数
        float t = m_T.GetResultFloat();

        // 独立変数の最小値
        float tMin = m_TMin.GetResultFloat();

        // もし最小値より小さかったら
        if (t <= tMin)
        {
            // 独立変数の相対的な値
            float relativeT = t - tMin;
        }

        //// 配列のインデックス
        //int i = 0;

        //while (i < m_ValueGapArray.Length - 1 && tempTime + m_ValueGapArray[i].T.GetResultFloat() < t)
        //{
        //    tempTime += m_ValueGapArray[i].T.GetResultFloat();

        //    i++;
        //}

        //// 最後の区間以外の場合
        //if (i < m_ValueGapArray.Length - 1)
        //{
        //    // 始点の位置ベクトル
        //    Vector2 positionPrev = m_ValueGapArray[i].Position.GetResultVector2();

        //    // 始点の速度ベクトル
        //    Vector2 velocityPrev = m_ValueGapArray[i].Velocity.GetResultVector2();

        //    // 所要時間
        //    float timeLength = m_ValueGapArray[i].T.GetResultFloat();

        //    // 終点の位置ベクトル
        //    Vector2 position = m_ValueGapArray[i + 1].Position.GetResultVector2();

        //    // 終点の速度ベクトル
        //    Vector2 velocity = m_ValueGapArray[i + 1].Velocity.GetResultVector2();

        //    return GetBezier(
        //        positionPrev,
        //        positionPrev + velocityPrev * timeLength,
        //        position - velocity * timeLength,
        //        position,
        //        (t - tempTime) / timeLength
        //        );
        //}
        //// 最後の区間の場合
        //else
        //{
        //    // 始点の位置ベクトル
        //    Vector2 positionPrev = m_ValueGapArray[m_ValueGapArray.Length - 1].Position.GetResultVector2();

        //    // 始点の速度ベクトル
        //    Vector2 velocityPrev = m_ValueGapArray[m_ValueGapArray.Length - 1].Velocity.GetResultVector2();

        //    // 所要時間
        //    float timeLength = m_ValueGapArray[m_ValueGapArray.Length - 1].T.GetResultFloat();

        //    // 終点の位置ベクトル
        //    Vector2 position = m_LastValue.GetResultVector2();

        //    // 終点の速度ベクトル
        //    Vector2 velocity = m_EndVelocity.GetResultVector2();

        //    return GetBezier(
        //        positionPrev,
        //        positionPrev + velocityPrev * timeLength,
        //        position - velocity * timeLength,
        //        position,
        //        (t - tempTime) / timeLength
        //        );
        //}

        return 0;
    }
}