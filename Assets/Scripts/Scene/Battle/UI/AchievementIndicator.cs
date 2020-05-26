using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// Achievementの項目を表示する。
/// </summary>
public class AchievementIndicator : ControllableMonoBehavior
{
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

    private IDisposable m_OnValueChange;
    private int m_MaxValue;

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        InitializeLabel();
        m_OnValueChange = SubscribeAchievement();
        m_MaxValue = GetMaxValue();
        SetValue(GetCurrentValue());
        m_MaxValueText.text = m_MaxValue.ToString();
    }

    public override void OnFinalize()
    {
        m_OnValueChange?.Dispose();
        base.OnFinalize();
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

    private IDisposable SubscribeAchievement()
    {
        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return battleData.LevelInChapter.Subscribe(x => OnChangeValue(x));
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return battleData.MaxChainInChapter.Subscribe(x => OnChangeValue(x));
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return battleData.BulletRemoveInChapter.Subscribe(x => OnChangeValue(x));
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return battleData.SecretItemInChapter.Subscribe(x => OnChangeValue(x));
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return battleData.BossRescueCountInChapter.Subscribe(x => OnChangeValue(x));
        }

        return null;
    }

    private int GetCurrentValue()
    {
        if (DataManager.Instance == null || DataManager.Instance.BattleData == null)
        {
            return 0;
        }

        var battleData = DataManager.Instance.BattleData;
        switch (m_Type)
        {
            case E_ACHIEVEMENT_TYPE.LEVEL:
                return battleData.LevelInChapter.Value;
            case E_ACHIEVEMENT_TYPE.MAX_CHAIN:
                return battleData.MaxChainInChapter.Value;
            case E_ACHIEVEMENT_TYPE.BULLET_REMOVE:
                return battleData.BulletRemoveInChapter.Value;
            case E_ACHIEVEMENT_TYPE.SECRET_ITEM:
                return battleData.SecretItemInChapter.Value;
            case E_ACHIEVEMENT_TYPE.RESCUE:
                return battleData.BossRescueCountInChapter.Value;
        }

        return 0;
    }

    private int GetMaxValue()
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

    private void OnChangeValue(int value)
    {
        SetValue(value);

        if (value >= m_MaxValue)
        {
            PlayAchievedAnimation();
            m_OnValueChange?.Dispose();
        }
    }

    public void SetValue(int value)
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
