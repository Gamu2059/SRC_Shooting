#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングの画面で弾が見える範囲の値を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/bulletVisibleRange", fileName = "BulletVisibleRange", order = 0)]
[System.Serializable]
public class BulletVisibleRange : OperationFloatBase
{

    /// <summary>
    /// x座標の最小値
    /// </summary>
    public static float XMin { get; set; } = -0.91F;

    /// <summary>
    /// x座標の最大値
    /// </summary>
    public static float XMax { get; set; } = 0.91F;

    /// <summary>
    /// y座標の最小値
    /// </summary>
    public static float YMin { get; set; } = -1.1F;

    /// <summary>
    /// y座標の最大値
    /// </summary>
    public static float YMax { get; set; } = 1.1F;

    /// <summary>
    /// falseならx座標、trueならy座標の値を取得する
    /// </summary>
    [SerializeField]
    private bool m_IsYAxis;

    /// <summary>
    /// falseなら最小値、trueなら最大値を取得する
    /// </summary>
    [SerializeField]
    private bool m_IsMax;


    public override float GetResultFloat()
    {
        // x
        if (!m_IsYAxis)
        {
            // min
            if (!m_IsMax)
            {
                return XMin;
            }
            // max
            else
            {
                return XMax;
            }
        }
        // y
        else
        {
            // min
            if (!m_IsMax)
            {
                return YMin;
            }
            // max
            else
            {
                return YMax;
            }
        }
    }
}
