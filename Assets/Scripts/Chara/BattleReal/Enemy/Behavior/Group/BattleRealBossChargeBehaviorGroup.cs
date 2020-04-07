#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスのチャージ攻撃を行うための処理機構。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/Behavior/Group/Charge", fileName = "param.behavior_group.asset")]
public class BattleRealBossChargeBehaviorGroup : BattleRealEnemyBehaviorGroupBase
{
    #region Field Inspector

    [Header("Charge Parameter")]

    [SerializeField, Tooltip("チャージ時間")]
    private float m_ChargeDuration;

    [SerializeField, Tooltip("チャージを失敗させるために与えなければならないダメージ量")]
    private float m_DamageUntilChargeFailure;

    [SerializeField]
    private EffectParamSet m_ChargeEffect;

    [Header("Charge Behavior Parameter")]

    [SerializeField, Tooltip("チャージに成功した時に実行する振る舞いのリスト")]
    private List<BattleRealEnemyBehaviorElement> m_ChargeSuccessElements;

    [SerializeField, Tooltip("チャージに失敗した時に実行する振る舞いのリスト")]
    private List<BattleRealEnemyBehaviorElement> m_ChargeFailureElements;

    #endregion

    #region Field

    private bool m_IsInvalid;
    private BattleRealBossController m_Boss;

    private bool m_IsFirstLoop;
    private bool m_IsChargeSuccess;

    #endregion

    protected override void OnStart()
    {
        base.OnStart();

        m_IsInvalid = false;
        m_Boss = Enemy as BattleRealBossController;
        if (m_Boss == null)
        {
            m_IsInvalid = true;
            Debug.LogErrorFormat(
                "[{0}] : このBehaviorGroupは、{1}を継承した敵にしか使用することができません。 Enemy : {2}",
                GetType().Name,
                typeof(BattleRealBossController).Name,
                Enemy.name
                );
            return;
        }

        m_IsFirstLoop = true;
        m_IsChargeSuccess = false;

        m_Boss.StartCharge(m_ChargeDuration, m_DamageUntilChargeFailure, m_ChargeEffect);
    }

    protected override void OnLooped()
    {
        base.OnLooped();

        if (m_IsFirstLoop)
        {
            m_IsFirstLoop = false;
            m_IsChargeSuccess = m_Boss.IsChargeSuccess;
        }
    }

    protected override List<BattleRealEnemyBehaviorElement> GetCurrentElements()
    {
        // 無効な時は無条件でnullを返す
        if (m_IsInvalid)
        {
            return null;
        }

        if (m_IsFirstLoop)
        {
            return base.GetCurrentElements();
        }

        return m_IsChargeSuccess ? m_ChargeSuccessElements : m_ChargeFailureElements;
    }

    public override bool IsEndGroup()
    {
        // 無効な時は無条件で終了させる
        if (m_IsInvalid)
        {
            return true;
        }

        // 1ループ目は必ず終了しないでチャージの成功判定を見て2ループ目へつなげる
        if (m_IsFirstLoop)
        {
            return false;
        }

        return true;
    }
}
