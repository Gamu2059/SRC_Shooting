using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

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

    private IDisposable m_OnChangeValue;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        SubscribeChangeValue();
        SetValue(GetValue());
    }

    public override void OnFinalize()
    {
        m_OnChangeValue?.Dispose();
        base.OnFinalize();
    }

    #endregion

    private IDisposable SubscribeChangeValue()
    {
        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_TYPE.BEST_SCORE:
                return battleData.BestScore.Subscribe(SetValue);
            case E_TYPE.SCORE:
                return battleData.Score.Subscribe(SetValue);
            case E_TYPE.CHAIN:
                return battleData.ChainInChapter.Subscribe(x => SetValue((ulong)x));
        }

        return null;
    }

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
                return battleData.BestScore.Value;
            case E_TYPE.SCORE:
                return battleData.Score.Value;
            case E_TYPE.CHAIN:
                return (ulong) battleData.ChainInChapter.Value;
        }

        return 0;
    }

    /// <summary>
    /// 指定した値を表示する。
    /// </summary>
    public void SetValue(ulong value)
    {
        if (m_OutText != null)
        {
            m_OutText.text = value.ToString();
        }
    }
}
