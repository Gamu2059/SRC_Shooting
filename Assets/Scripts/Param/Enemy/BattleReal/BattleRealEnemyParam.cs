#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵のパラメータ。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyParam", fileName = "param.battle_real_enemy.asset")]
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

    [SerializeField, Tooltip("移動、攻撃の具体的な処理を決定するパラメータ Noneの場合、何もしない敵になる")]
    private BattleRealEnemyBehaviorUnit m_Behavior;
    public BattleRealEnemyBehaviorUnit Behavior => m_Behavior;

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
}
