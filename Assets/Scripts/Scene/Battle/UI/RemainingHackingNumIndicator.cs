#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainingHackingNumIndicator : ControllableMonoBehavior
{
    [SerializeField, Tooltip("スコアを表示させるテキスト")]
    private Text m_OutText;

    [SerializeField, Tooltip("trueの場合、コンソールにも表示する")]
    private bool m_IsShowOnConsole;

    private int m_PreRemainingHackingNum;

    [HideInInspector()]
    public BattleRealBossController Boss;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_PreRemainingHackingNum = GetRemainingHackingNum();
        Show(m_PreRemainingHackingNum);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var remaining = GetRemainingHackingNum();
        if(m_PreRemainingHackingNum != remaining)
        {
            m_PreRemainingHackingNum = remaining;
            Show(remaining);
        }
    }

    #endregion

    private int GetRemainingHackingNum()
    {
        if(Boss == null)
        {
            return m_PreRemainingHackingNum;
        }
        return Boss.HackingCompleteNum - Boss.HackingSuccessCount;
    }

    public void Show(int remaining)
    {
        var showText = string.Format("×" + remaining.ToString());

        if (m_OutText != null)
        {
            m_OutText.text = showText;
        }

        if (m_IsShowOnConsole)
        {
            Debug.LogFormat("RemainingHackingNum : {0}", showText);
        }
    }
}
