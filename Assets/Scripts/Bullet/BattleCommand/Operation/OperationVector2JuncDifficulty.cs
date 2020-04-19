#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって条件分岐させ、Vector2型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/junction/difficulty", fileName = "OperationVector2JuncDifficulty", order = 0)]
[System.Serializable]
public class OperationVector2JuncDifficulty : OperationVector2Base
{

    /// <summary>
    /// 難易度
    /// </summary>
    private static E_DIFFICULTY Difficulty = DanmakuDifficulty.Difficulty;

    /// <summary>
    /// 難易度がEasyだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Easy;

    /// <summary>
    /// 難易度がNormalだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Normal;

    /// <summary>
    /// 難易度がHardだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Hard;

    /// <summary>
    /// 難易度がHadesだった場合の値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Hades;


    public override Vector2 GetResultVector2()
    {
        switch (Difficulty)
        {
            case E_DIFFICULTY.EASY:
                return m_Easy.GetResultVector2();

            case E_DIFFICULTY.NORMAL:
                return m_Normal.GetResultVector2();

            case E_DIFFICULTY.HARD:
                return m_Hard.GetResultVector2();

            case E_DIFFICULTY.HADES:
                return m_Hades.GetResultVector2();

            default:
                return Vector2.zero;
        }
    }
}