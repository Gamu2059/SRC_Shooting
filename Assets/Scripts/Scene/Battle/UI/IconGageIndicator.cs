#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// LevelやEnergyのゲージを制御する
/// </summary>
public class IconGageIndicator : ControllableMonoBehavior
{
    [Serializable]
    private enum E_VALUE_TYPE
    {
        LEVEL,
        ENERGY,
        BOSS_HP,
        BOSS_DOWN,
    }

    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    private float m_PreRate;

    private BattleRealBossController m_Boss;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreRate = GetValueRate();
        ShowRate(m_PreRate);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var count = GetValueRate();
        if (m_PreRate != count)
        {
            ShowRate(count);
            m_PreRate = count;
        }
    }

    #endregion

    private BattleRealBossController GetBoss()
    {
        if (m_Boss != null && m_Boss.GetCycle() == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            return m_Boss;
        }

        m_Boss = null;
        var enemies = BattleRealEnemyManager.Instance.Enemies;
        foreach (var e in enemies)
        {
            if (e.IsBoss && e is BattleRealBossController boss)
            {
                m_Boss = boss;
            }
        }
        return m_Boss;
    }

    private float GetValueRate()
    {
        var battleData = DataManager.Instance.BattleData;

        switch (m_ValueType)
        {
            case E_VALUE_TYPE.LEVEL:
                float exp = battleData.Exp;
                float necessaryExp = battleData.GetCurrentNecessaryExp();
                return necessaryExp > 0 ? exp / necessaryExp : 0;
            case E_VALUE_TYPE.ENERGY:
                float energyCharge = battleData.EnergyCharge;
                float necessaryEnergyCharge = battleData.GetCurrentNecessaryEnergyCharge();
                return necessaryEnergyCharge > 0 ? energyCharge / necessaryEnergyCharge : 0;
            case E_VALUE_TYPE.BOSS_HP:
                var boss = GetBoss();
                if (boss == null)
                {
                    return 0;
                }
                return boss.MaxHp > 0 ? boss.NowHp / boss.MaxHp : 0;
            case E_VALUE_TYPE.BOSS_DOWN:
                var b = GetBoss();
                if (b == null)
                {
                    return 0;
                }
                return b.MaxDownHp > 0 ? b.NowDownHp / b.MaxDownHp : 0;
        }

        return 0;
    }

    private void ShowRate(float rate)
    {
        rate = Mathf.Clamp01(rate);
        m_Slider.value = rate;
    }
}
