#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scoreを表示する
/// </summary>
public class ScoreIndicator : ControllableMonoBehavior
{
    [SerializeField, Tooltip("スコアを表示させるテキスト")]
    private Text m_OutText;

    [SerializeField, Tooltip("trueの場合、BestScoreを参照する")]
    private bool m_IsShowBestScore;

    [SerializeField, Tooltip("trueの場合、コンソールにも表示する")]
    private bool m_IsShowOnConsole;

    private double m_PreScore;

    #region Game Cycle

    public override void OnStart()
    {
        base.OnStart();

        m_PreScore = GetScore();
        Show((int)m_PreScore);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var score = GetScore();
        if (m_PreScore != score)
        {
            m_PreScore = score;
            Show((int)m_PreScore);
        }
    }

    #endregion

    private double GetScore()
    {
        var battleData = DataManager.Instance.BattleData;
        return m_IsShowBestScore ? battleData.BestScore : battleData.Score;
    }

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
            if (m_IsShowBestScore)
            {
                Debug.LogFormat("BestScore : {0}", score);
            }
            else
            {
                Debug.LogFormat("Score : {0}", score);
            }
        }
    }
}
