#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスのパラメータ。
/// </summary>
[Serializable]
public class BattleRealBossParam : BattleRealEnemyParamBase
{
    [SerializeField, Tooltip("表示用に用いる名前")]
    private string m_BossName;
    public string BossName => m_BossName;

    [Header("振る舞いパラメータ")]

    [SerializeField, Tooltip("振る舞いパラメータ")]
    private BattleRealBossBehaviorSet[] m_BehaviorSets;
    public BattleRealBossBehaviorSet[] BehaviorSets => m_BehaviorSets;

    [Header("初期化時パラメータ")]

    [SerializeField, Tooltip("初期化時に発行するイベント 個数0の場合、何もしない")]
    private BattleRealEventContent[] m_OnInitializeEvents;
    public BattleRealEventContent[] OnInitializeEvents => m_OnInitializeEvents;

    [SerializeField, Tooltip("初期化時に行うシーケンス Noneの場合、何もせず、すぐに移動や攻撃に遷移する")]
    private SequenceGroup m_OnInitializeSequence;
    public SequenceGroup OnInitializeSequence => m_OnInitializeSequence;

    [Header("移動、攻撃時パラメータ")]

    [SerializeField, Tooltip("移動、攻撃を開始した時に発行するイベント 個数0の場合、何もしない")]
    private BattleRealEventContent[] m_OnStartBehaviorEvents;
    public BattleRealEventContent[] OnStartBehaviorEvents => m_OnStartBehaviorEvents;

    #region Down

    [Header("ダウン時パラメータ")]

    [SerializeField, Tooltip("ダウンした瞬間のエフェクト")]
    private EffectParamSet m_DownEffectParam;
    public EffectParamSet DownEffectParam => m_DownEffectParam;

    [SerializeField, Tooltip("ダウン中にプレイヤー側につくエフェクト")]
    private EffectParamSet m_PlayerTriangleEffectParam;
    public EffectParamSet PlayerTriangleEffectParam => m_PlayerTriangleEffectParam;

    [SerializeField, Tooltip("ハッキング失敗時のエフェクト")]
    private EffectParamSet m_HackingFailureEffectParam;
    public EffectParamSet HackingFailureEffectParam => m_HackingFailureEffectParam;

    [SerializeField, Tooltip("ハッキング成功時のエフェクト")]
    private EffectParamSet m_HackingSuccessEffectParam;
    public EffectParamSet HackingSuccessEffectParam => m_HackingSuccessEffectParam;

    [SerializeField, Tooltip("ハッキングに失敗した時に回復するダウンHPの割合")]
    private float m_HackingFailureRecoverDownHpRate;
    public float HackingFailureRecoverDownHpRate => m_HackingFailureRecoverDownHpRate;

    #endregion

    #region Defeat

    [Header("撃破時パラメータ")]

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_DefeatScore;
    public override int DefeatScore => m_DefeatScore;

    [SerializeField, Tooltip("撃破時のドロップアイテム")]
    private ItemCreateParam m_DefeatItemParam;
    public override ItemCreateParam DefeatItemParam => m_DefeatItemParam;

    [SerializeField, Tooltip("撃破時のイベント")]
    private BattleRealEventContent[] m_DefeatEvents;
    public override BattleRealEventContent[] DefeatEvents => m_DefeatEvents;

    [SerializeField, Tooltip("撃破時の一連のエフェクト")]
    private SequentialEffectParamSet m_DefeatSequentialEffect;
    public override SequentialEffectParamSet DefeatSequentialEffect => m_DefeatSequentialEffect;

    [SerializeField, Tooltip("撃破時の敵非表示タイミング")]
    private float m_DefeatHideTime;
    public override float DefeatHideTime => m_DefeatHideTime;

    #endregion

    #region Rescue

    [Header("救出時パラメータ")]

    [SerializeField, Tooltip("救出時の獲得スコア")]
    private int m_RescueScore;
    public int RescueScore => m_RescueScore;

    [SerializeField, Tooltip("救出時のドロップアイテム")]
    private ItemCreateParam m_RescueItemParam;
    public ItemCreateParam RescueItemParam => m_RescueItemParam;

    [SerializeField, Tooltip("救出時のイベント")]
    private BattleRealEventContent[] m_RescueEvents;
    public BattleRealEventContent[] RescueEvents => m_RescueEvents;

    [SerializeField, Tooltip("救出時の一連のエフェクト")]
    private SequentialEffectParamSet m_RescueSequentialEffect;
    public SequentialEffectParamSet RescueSequentialEffect => m_RescueSequentialEffect;

    [SerializeField, Tooltip("救出時の敵非表示タイミング")]
    private float m_RescueHideTime;
    public float RescueHideTime => m_RescueHideTime;

    #endregion

    #region Retire

    [Header("退場パラメータ")]

    [SerializeField, Tooltip("退場時のイベント")]
    private BattleRealEventContent[] m_RetireEvents;
    public override BattleRealEventContent[] RetireEvents => m_RetireEvents;

    [SerializeField, Tooltip("退場時の一連のエフェクト")]
    private SequentialEffectParamSet m_RetireSequentialEffect;
    public override SequentialEffectParamSet RetireSequentialEffect => m_RetireSequentialEffect;

    [SerializeField, Tooltip("退場時の敵非表示タイミング")]
    private float m_RetireHideTime;
    public override float RetireHideTime => m_RetireHideTime;

    #endregion
}
