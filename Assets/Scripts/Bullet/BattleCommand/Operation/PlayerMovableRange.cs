#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングの画面で自機が動ける範囲の値を取得するためのクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/readonly/playerMovableRange", fileName = "PlayerMovableRange", order = 0)]
[System.Serializable]
public class PlayerMovableRange : OperationFloatBase
{

    /// <summary>
    /// x座標の最小値
    /// </summary>
    public static float XMin { get; set; } = -0.8F;

    /// <summary>
    /// x座標の最大値
    /// </summary>
    public static float XMax { get; set; } = 0.8F;

    /// <summary>
    /// y座標の最小値
    /// </summary>
    public static float YMin { get; set; } = -0.99F;

    /// <summary>
    /// y座標の最大値
    /// </summary>
    public static float YMax { get; set; } = 0.99F;

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
