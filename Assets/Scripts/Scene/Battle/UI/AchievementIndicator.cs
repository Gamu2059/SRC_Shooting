#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Achievementの項目を表示する。
/// </summary>
public class AchievementIndicator : ControllableMonoBehavior
{
    #region Define

    private enum E_ACHIEVEMENT_TYPE
    {
        LEVEL,
        MAX_CHAIN,
        BULLET_REMOVE,
        SECRET_ITEM,
        RESCUE,
    }

    #endregion

    #region Field Inspector

    [SerializeField]
    private E_ACHIEVEMENT_TYPE m_Type;

    [SerializeField]
    private Text m_LabelText;

    [SerializeField]
    private Text m_CurrentValueText;

    [SerializeField]
    private Text m_MaxValueText;

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private string m_AchievedAnimationName = "battle_real_achieved";

    #endregion

    #region Field

    private ulong m_PreValue;
    private ulong m_MaxValue;
    private bool m_Achieved;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        InitializeLabel();

        m_PreValue = GetCurrentValue();
        m_MaxValue = GetMaxValue();
        SetValue(m_PreValue);
        m_MaxValueText.text = m_MaxValue.ToString();
        m_Achieved = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        var value = GetCurrentValue();
        if (value != m_PreValue)
        {
            m_PreValue = value;
            SetValue(m_PreValue);
        }

        if (value >= m_MaxValue && !m_Achieved)
        {
            m_Achieved = true;
            PlayAchievedAnimation();
        }
    }

    #endregion

    private void InitializeLabel()
    {
        if (m_LabelText == null)
        {
            return;
        }

        switch (m_Type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                m_LabelText.text = "Level";
                break;
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                m_LabelText.text = "Max Chain";
                break;
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                m_LabelText.text = "Bullet Remove";
                break;
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                m_LabelText.text = "Secret Item";
                break;
            case E_ACHIEVEMENT_TYPE.RESCUE:
                m_LabelText.text = "Rescue";
                break;
        }
    }

    private ulong GetCurrentValue()
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return 0;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return (ulong)battleData.Level;
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return battleData.MaxChain;
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return battleData.BulletRemoveCount;
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return battleData.SecretItemCount;
            case E_ACHIEVEMENT_TYPE.RESCUE:
                var rescueName = BattleRealEventManager.Instance.GetGeneralIntName(E_GENERAL_INT_VARIABLE.BOSS_RESCUE);
                var rescueNum = BattleRealEventManager.Instance.GetInt(rescueName, 0);
                return (ulong) rescueNum;
        }

        return 0;
    }

    private ulong GetMaxValue()
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return 0;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return battleData.GetAchievementTargetLevel();
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return battleData.GetAchievementTargetMaxChain();
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return battleData.GetAchievementTargetBulletRemove();
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return battleData.GetAchievementTargetSecretItem();
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return battleData.GetAchievementTargetRescue();
        }

        return 0;
    }

    public void SetValue(ulong value)
    {
        if (m_CurrentValueText != null)
        {
            m_CurrentValueText.text = value.ToString();
        }
    }

    private void PlayAchievedAnimation()
    {
        m_Animator?.Play(m_AchievedAnimationName);
    }
}
