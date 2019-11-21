#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// Scoreを表示する
/// </summary>
public class ScoreIndicator : ControllableMonoBehavior
{
    [SerializeField]
    private Text m_OutText;

    [SerializeField]
    private bool m_IsShowOnConsole;

    private double m_PreScore;

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        var battleData = DataManager.Instance.BattleData;
        m_PreScore = battleData.Score;
        Show((int)m_PreScore);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var battleData = DataManager.Instance.BattleData;
        if (m_PreScore != battleData.Score)
        {
            m_PreScore = battleData.Score;
            Show((int)m_PreScore);
        }
    }

    #endregion

    /// <summary>
    /// 指定した値をスコアとして表示する。
    /// </summary>
    public void Show(int score)
    {
        if (m_OutText != null)
        {
            m_OutText.text = score.ToString();
        }

        if (m_IsShowOnConsole)
        {
            Debug.LogFormat("Score : {0}", score);
        }
    }
}
