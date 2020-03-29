using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度によって条件分岐させ、int型の値を返す演算を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/junction/difficulty", fileName = "OperationIntJuncDifficulty", order = 0)]
[System.Serializable]
public class OperationIntJuncDifficulty : OperationIntBase
{

    /// <summary>
    /// 難易度がEasyだった場合の値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Easy;

    /// <summary>
    /// 難易度がNormalだった場合の値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Normal;

    /// <summary>
    /// 難易度がHardだった場合の値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Hard;

    /// <summary>
    /// 難易度がHadesだった場合の値
    /// </summary>
    [SerializeField]
    private OperationIntBase m_Hades;


    public override float GetResultFloat()
    {
        return GetResultInt();
    }


    public override int GetResultInt()
    {
        // 現在の難易度を取得する
        E_DIFFICULTY difficulty = DataManager.Instance.BattleData.Difficulty;

        switch (difficulty)
        {
            case E_DIFFICULTY.EASY:
                return m_Easy.GetResultInt();

            case E_DIFFICULTY.NORMAL:
                return m_Normal.GetResultInt();

            case E_DIFFICULTY.HARD:
                return m_Hard.GetResultInt();

            case E_DIFFICULTY.HADES:
                return m_Hades.GetResultInt();

            default:
                return 0;
        }
    }
}