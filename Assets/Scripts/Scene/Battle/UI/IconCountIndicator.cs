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
    }

    [SerializeField]
    private IconIndicator[] m_Icons;

    [SerializeField]
    private E_COUNT_TYPE m_CountType;

    private int m_PreCount;

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_PreCount = GetValueCount();
        ShowCount(m_PreCount, false);
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
                return battleData.PlayerLife;
            case E_COUNT_TYPE.LEVEL:
                return battleData.Level;
            case E_COUNT_TYPE.ENERGY:
                return battleData.EnergyCount;
        }

        return 0;
    }

    public void ShowCount(int count, bool withAnimation = true)
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
                    if (withAnimation)
                    {
                        icon.PlayAnimation();
                    }
                }
            }
        }
    }
}
