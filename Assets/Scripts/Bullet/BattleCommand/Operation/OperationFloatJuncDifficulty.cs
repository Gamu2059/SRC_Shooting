#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって条件分岐させ、float型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/float/junction/difficulty", fileName = "OperationFloatJuncDifficulty", order = 0)]
[System.Serializable]
public class OperationFloatJuncDifficulty : OperationFloatBase
{

    /// <summary>
    /// 難易度
    /// </summary>
    private static E_DIFFICULTY Difficulty = DanmakuDifficulty.Difficulty;

    /// <summary>
    /// 難易度がEasyだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Easy;

    /// <summary>
    /// 難易度がNormalだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Normal;

    /// <summary>
    /// 難易度がHardだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Hard;

    /// <summary>
    /// 難易度がHadesだった場合の値
    /// </summary>
    [SerializeField]
    private OperationFloatBase m_Hades;


    public override float GetResultFloat()
    {
        switch (Difficulty)
        {
            case E_DIFFICULTY.EASY:
                return m_Easy.GetResultFloat();

            case E_DIFFICULTY.NORMAL:
                return m_Normal.GetResultFloat();

            case E_DIFFICULTY.HARD:
                return m_Hard.GetResultFloat();

            case E_DIFFICULTY.HADES:
                return m_Hades.GetResultFloat();

            default:
                return 0;
        }
    }
}





// 現在の難易度を取得する
//E_DIFFICULTY difficulty = DataManager.Instance.BattleData.Difficulty;
//E_DIFFICULTY difficulty = DanmakuDifficulty.Difficulty;
