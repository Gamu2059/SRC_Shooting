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

    [HideInInspector()]
    public BattleRealBossController Boss;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreCount = GetValueCount() - 1;
        ShowCountOnForce(m_PreCount);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var count = GetValueCount() - 1;
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
                return battleData.PlayerLife.Value;
            case E_COUNT_TYPE.LEVEL:
                return battleData.LevelInChapter.Value;
            case E_COUNT_TYPE.ENERGY:
                return battleData.EnergyStock.Value;
            case E_COUNT_TYPE.BOSS_HACKING:
                if (Boss == null)
                {
                    return -1;
                }
                return Boss.HackingCompleteNum - Boss.HackingSuccessCount;
        }

        return -1;
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
