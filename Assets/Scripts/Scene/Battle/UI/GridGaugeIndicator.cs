#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridGaugeIndicator : ControllableMonoBehavior
{
    [Serializable]
    private enum E_VALUE_TYPE
    {
        BOSS_HP,
    }

    [SerializeField]
    private GameObject[] m_Grids;

    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    private float m_PreRate;
    
    public BattleHackingEnemyController Boss;

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
            SetValue(m_PreRate);
        }
    }

    #endregion

    private float GetValueRate()
    {
        switch (m_ValueType)
        {
            //case E_VALUE_TYPE.TIME:
            //    if (hackingBoss != null)
            //    {
            //        var nowTime = hackingBoss.CurrentRemainTime;
            //        var maxTime = hackingBoss.MaxRemainTime;
            //        if (maxTime <= 0)
            //        {
            //            return 0;
            //        }
            //        return nowTime / maxTime;

            //    }
            //    break;

            //case E_VALUE_TYPE.BONUS_TIME:
            //    if (hackingBoss != null)
            //    {
            //        var nowBonusTime = hackingBoss.CurrentRemainBonusTime;
            //        var maxBonusTime = hackingBoss.MaxRemainBonusTime;
            //        if (maxBonusTime <= 0)
            //        {
            //            return 0;
            //        }
            //        return nowBonusTime / maxBonusTime;
            //    }
            //    break;

            case E_VALUE_TYPE.BOSS_HP:
                return GetBossHpValue();
        }
        return 0;
    }

    private float GetBossHpValue()
    {
        if (Boss == null || Boss.MaxHp <= 0)
        {
            return 0;
        }

        return Boss.NowHp / Boss.MaxHp;
    }

    public void SetValue(float rate)
    {
        rate = Mathf.Clamp01(rate);
        var enableCount = Mathf.CeilToInt(rate * m_Grids.Length);
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
