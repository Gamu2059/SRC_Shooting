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

    private BattleRealBoss m_Boss;

    private int m_PreRemainingHackingNum;

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

    private BattleRealBoss GetBoss()
    {
        if (m_Boss != null && m_Boss.GetCycle() == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            return m_Boss;
        }

        m_Boss = null;
        var enemies = BattleRealEnemyManager.Instance.Enemies;
        foreach (var e in enemies)
        {
            if (e.IsBoss && e is BattleRealBoss boss)
                m_Boss = boss;
        }
        return m_Boss;
    }

    private int GetRemainingHackingNum()
    {
        var boss = GetBoss();
        if(boss == null)
        {
            return m_PreRemainingHackingNum;
        }
        return boss.HackingCompleteNum - boss.m_HackingSuccessCount;
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
