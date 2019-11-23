#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridGageIndicator : ControllableMonoBehavior
{
    [Serializable]
    private enum E_VALUE_TYPE
    {
        TIME,
        BONUS_TIME,
        BOSS_HP,
    }

    [SerializeField]
    private GameObject[] m_Grids;

    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    private float m_PreRate;
    private BattleHackingEnemyController m_Boss;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var rate = GetValueRate();
        if (m_PreRate != rate)
        {
            m_PreRate = rate;
            ShowRate(m_PreRate);
        }
    }

    #endregion

    private BattleHackingEnemyController GetBoss()
    {
        if (m_Boss != null && m_Boss.GetCycle() == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            return m_Boss;
        }

        m_Boss = null;
        var enemies = BattleHackingEnemyManager.Instance.Enemies;
        foreach (var e in enemies)
        {
            if (e.IsBoss)
            {
                m_Boss = e;
            }
        }

        return m_Boss;
    }

    private float GetValueRate()
    {
        var hackingManager = BattleHackingManager.Instance;
        switch (m_ValueType)
        {
            case E_VALUE_TYPE.TIME:
                var nowTime = hackingManager.CurrentRemainTime;
                var maxTime = hackingManager.MaxRemainTime;
                if (maxTime <= 0)
                {
                    return 0;
                }
                return nowTime / maxTime;

            case E_VALUE_TYPE.BONUS_TIME:
                var nowBonusTime = hackingManager.CurrentRemainBonusTime;
                var maxBonusTime = hackingManager.MaxRemainBonusTime;
                if (maxBonusTime <= 0)
                {
                    return 0;
                }
                return nowBonusTime / maxBonusTime;

            case E_VALUE_TYPE.BOSS_HP:
                var boss = GetBoss();
                if (boss == null)
                {
                    return 0;
                }
                return boss.NowHp / boss.MaxHp;
        }
        return 0;
    }

    public void ShowRate(float rate)
    {
        rate = Mathf.Clamp01(rate);
        var enableCount = Mathf.CeilToInt(rate * m_Grids.Length);
        Debug.Log(m_ValueType + ", " + enableCount);
        for (int i = 0; i < m_Grids.Length; i++)
        {
            var grid = m_Grids[i];
            if (grid.activeSelf)
            {
                if (i >= enableCount)
                {
                    grid.SetActive(false);
                }
            }
            else
            {
                if (i < enableCount)
                {
                    grid.SetActive(true);
                }
            }
        }
    }
}
