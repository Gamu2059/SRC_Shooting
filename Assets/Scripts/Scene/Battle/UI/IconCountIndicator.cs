#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 数があるアイコンを制御する
/// </summary>
public class IconCountIndicator : ControllableMonoBehavior
{
    [Serializable]
    private enum E_COUNT_TYPE
    {
        LIFE,
        LEVEL,
        ENERGY,
        BOSS_HACKING,
    }

    [SerializeField]
    private IconIndicator[] m_Icons;

    [SerializeField]
    private E_COUNT_TYPE m_CountType;

    private int m_PreCount;

    private BattleRealBoss m_Boss;

    private BattleRealBoss GetBoss()
    {
        if(m_Boss != null && m_Boss.GetCycle() == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            return m_Boss;
        }

        m_Boss = null;
        var enemies = BattleRealEnemyManager.Instance.Enemies;
        foreach(var e in enemies)
        {
            if(e.IsBoss && e is BattleRealBoss boss)
            {
                m_Boss = boss;
            }
        }
        return m_Boss;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreCount = GetValueCount();
        ShowCountOnForce(m_PreCount);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var count = GetValueCount();
        if (m_PreCount != count)
        {
            ShowCount(count);
            m_PreCount = count;
        }
    }

    #endregion

    private int GetValueCount()
    {
        var battleData = DataManager.Instance.BattleData;

        switch (m_CountType)
        {
            case E_COUNT_TYPE.LIFE:
                // 残機は1の時に1個目が表示されてほしいので、-1
                return battleData.PlayerLife - 1;
            case E_COUNT_TYPE.LEVEL:
                // レベルは0の時に1個目が表示されてほしいので、そのまま
                return battleData.Level;
            case E_COUNT_TYPE.ENERGY:
                // エナジーは1の時に1個目が表示されてほしいので、-1
                return battleData.EnergyCount - 1;
            case E_COUNT_TYPE.BOSS_HACKING:
                var boss = GetBoss();
                if(boss == null)
                {
                    return -1;
                }
                Debug.Log("HackingCompleteNum = " + boss.HackingCompleteNum);
                Debug.Log("HackingSucceedCount = " + boss.m_HackingSuccessCount);
                return (boss.HackingCompleteNum - boss.m_HackingSuccessCount) - 1;
        }

        return 0;
    }
    public void ShowCountOnForce(int count)
    {
        for (int i = 0; i < m_Icons.Length; i++)
        {
            var icon = m_Icons[i];
            icon.SetEnable(i <= count);
        }
    }

    public void ShowCount(int count)
    {
        for (int i = 0; i < m_Icons.Length; i++)
        {
            var icon = m_Icons[i];

            if (icon.IsEnable)
            {
                if (i > count)
                {
                    icon.SetEnable(false);
                }
            }
            else
            {
                if (i <= count)
                {
                    icon.SetEnable(true);
                    icon.PlayAnimation();
                }
            }
        }
    }
}
