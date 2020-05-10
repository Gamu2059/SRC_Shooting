#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって条件分岐させ、float型の定数を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/difficulty/constant", fileName = "OperationFloatConstantJuncDifficulty", order = 0)]
[System.Serializable]
public class OperationFloatDifficultyConstant : OperationFloatDifficultyBase
{

    /// <summary>
    /// 現在の難易度に基づいた値
    /// </summary>
    private float m_Value;

    /// <summary>
    /// 難易度がEasyだった場合の値
    /// </summary>
    [SerializeField]
    private float m_Easy;

    /// <summary>
    /// 難易度がNormalだった場合の値
    /// </summary>
    [SerializeField]
    private float m_Normal;

    /// <summary>
    /// 難易度がHardだった場合の値
    /// </summary>
    [SerializeField]
    private float m_Hard;

    /// <summary>
    /// 難易度がHadesだった場合の値
    /// </summary>
    [SerializeField]
    private float m_Hades;


    public override void Setup()
    {
        switch (DanmakuDifficulty.Difficulty)
        {
            case E_DIFFICULTY.EASY:
                m_Value = m_Easy;
                break;

            case E_DIFFICULTY.NORMAL:
                m_Value = m_Normal;
                break;

            case E_DIFFICULTY.HARD:
                m_Value = m_Hard;
                break;

            case E_DIFFICULTY.HADES:
                m_Value = m_Hades;
                break;
        }
    }


    public override float GetResultFloat()
    {
        return m_Value;
    }
}