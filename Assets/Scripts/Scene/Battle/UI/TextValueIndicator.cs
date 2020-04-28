#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// テキストに数値として表示する。
/// </summary>
public class TextValueIndicator : ControllableMonoBehavior
{
    #region Define

    [Serializable]
    private enum E_TYPE
    {
        BEST_SCORE,
        SCORE,
        CHAIN,
    }

    #endregion

    [SerializeField, Tooltip("表示させるテキスト")]
    private Text m_OutText;

    [SerializeField, Tooltip("表示するもののタイプ")]
    private E_TYPE m_Type;

    private ulong m_PreValue;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreValue = GetValue();
        SetValue((int)m_PreValue);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var score = GetValue();
        if (m_PreValue != score)
        {
            m_PreValue = score;
            SetValue((int)m_PreValue);
        }
    }

    #endregion

    private ulong GetValue()
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return 0;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_TYPE.BEST_SCORE:
                return battleData.BestScore;
            case E_TYPE.SCORE:
                return battleData.Score;
            case E_TYPE.CHAIN:
                return battleData.Chain;
        }

        return 0;
    }

    /// <summary>
    /// 指定した値を表示する。
    /// </summary>
    public void SetValue(int value)
    {
        if (m_OutText != null)
        {
            m_OutText.text = value.ToString();
        }
    }
}
