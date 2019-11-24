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
    }

    [SerializeField]
    private Slider m_Slider;

    [SerializeField]
    private E_VALUE_TYPE m_ValueType;

    private float m_PreRate;

    private BattleRealEnemyController m_Boss;

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

    private BattleRealEnemyController GetBoss(){
        if(m_Boss != null && m_Boss.GetCycle() == E_POOLED_OBJECT_CYCLE.UPDATE){
            return m_Boss;
        }

        m_Boss = null;
        var enemies = BattleRealEnemyManager.Instance.Enemies;
        foreach (var e in enemies)
        {
            m_Boss = e;            
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
                float necessaryExp = battleData.GetCurrentLevelParam().NecessaryExpToLevelUpNextLevel;
                return exp / necessaryExp;
            case E_VALUE_TYPE.ENERGY:
                float energy = battleData.EnergyCharge;
                float maxEnergy = battleData.MaxEnergyCharge;
                return energy / maxEnergy;
            case E_VALUE_TYPE.BOSS_HP:
                var boss = GetBoss();
                if(boss == null){
                    return 0;
                }
                return boss.NowHp / boss.MaxHp;       
        }

        return 0;
    }

    public void ShowRate(float rate)
    {
        rate = Mathf.Clamp01(rate);
        m_Slider.value = rate;
    }
}
