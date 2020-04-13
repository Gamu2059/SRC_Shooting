#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のパラメータ。
/// </summary>
[Serializable]
public class BattleRealEnemyParam : BattleRealEnemyParamBase
{
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

    [SerializeField, Tooltip("移動、攻撃のパラメータのタイプ")]
    private E_ENEMY_BEHAVIOR_TYPE m_BehaviorType;
    public E_ENEMY_BEHAVIOR_TYPE BehaviorType => m_BehaviorType;

    [SerializeField, Tooltip("移動、攻撃の具体的な処理を決定するパラメータ Noneの場合、何もしない敵になる")]
    private BattleRealEnemyBehaviorUnitBase m_Behavior;
    public BattleRealEnemyBehaviorUnitBase Behavior => m_Behavior;

    [SerializeField, Tooltip("移動、攻撃の具体的な処理を決定するパラメータのグループ")]
    private BattleRealEnemyBehaviorGroupBase m_BehaviorGroup;
    public BattleRealEnemyBehaviorGroupBase BehaviorGroup => m_BehaviorGroup;

    [Header("撃破パラメータ")]

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
}
