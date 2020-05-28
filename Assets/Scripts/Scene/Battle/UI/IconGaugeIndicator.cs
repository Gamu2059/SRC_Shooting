#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// LevelやEnergyのゲージを制御する
/// </summary>
public class IconGaugeIndicator : ControllableMonoBehavior
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

    [HideInInspector()]
    public BattleRealBossController Boss;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreRate = GetValueRate();
        SetValue(m_PreRate);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var count = GetValueRate();
        if (m_PreRate != count)
        {
            SetValue(count);
            m_PreRate = count;
        }
    }

    #endregion

    private float GetValueRate()
    {
        switch (m_ValueType)
        {
            case E_VALUE_TYPE.LEVEL:
                return GetLevel();
            case E_VALUE_TYPE.ENERGY:
                return GetEnergy();
            case E_VALUE_TYPE.BOSS_HP:
                return GetBossHp();
            case E_VALUE_TYPE.BOSS_DOWN:
                return GetBossDownHp();
        }

        return 0;
    }

    private float GetLevel()
    {
        var battleData = DataManager.Instance.BattleData;
        float exp = battleData.ExpInChapter.Value;
        float necessaryExp = battleData.GetCurrentNecessaryExp();
        return necessaryExp > 0 ? exp / necessaryExp : 0;
    }

    private float GetEnergy()
    {
        var battleData = DataManager.Instance.BattleData;
        float energyCharge = battleData.EnergyCharge.Value;
        float necessaryEnergyCharge = battleData.GetCurrentNecessaryEnergyCharge();
        return necessaryEnergyCharge > 0 ? energyCharge / necessaryEnergyCharge : 0;
    }

    private float GetBossHp()
    {
        if (Boss == null)
        {
            return 0;
        }
        return Boss.MaxHp > 0 ? Boss.NowHp / Boss.MaxHp : 0;
    }

    private float GetBossDownHp()
    {
        if (Boss == null)
        {
            return 0;
        }
        return Boss.MaxDownHp > 0 ? Boss.NowDownHp / Boss.MaxDownHp : 0;
    }

    public void SetValue(float rate)
    {
        rate = Mathf.Clamp01(rate);
        m_Slider.value = rate;
    }
}
