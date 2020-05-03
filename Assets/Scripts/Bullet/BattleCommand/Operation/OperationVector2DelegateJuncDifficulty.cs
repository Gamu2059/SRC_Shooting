#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度ごとに異なるVector2値の演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/Vector2/Junc/DifficultyDelegate", fileName = "OperationVector2DelegateJuncDifficulty", order = 0)]
[System.Serializable]
public class OperationVector2DelegateJuncDifficulty : DifficultyVector2Base
{

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Easy;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Normal;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Hard;

    /// <summary>
    /// 値
    /// </summary>
    [SerializeField]
    private OperationVector2Base m_Hades;


    public override void Setup()
    {
        switch (DanmakuDifficulty.Difficulty)
        {
            case E_DIFFICULTY.EASY:
                Value = m_Easy.GetResultVector2();
                return;

            case E_DIFFICULTY.NORMAL:
                Value = m_Normal.GetResultVector2();
                return;

            case E_DIFFICULTY.HARD:
                Value = m_Hard.GetResultVector2();
                return;

            case E_DIFFICULTY.HADES:
                Value = m_Hades.GetResultVector2();
                return;
        }
    }
}