#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyGenerate", fileName = "param.battle_real_enemy_generate.asset")]
public class BattleRealEnemyGenerateParamSet : ScriptableObject
{
    [SerializeField, Tooltip("敵の体力")]
    private int m_Hp;
    public int Hp => m_Hp;

    [Header("被弾")]

    [SerializeField]
    private Material m_DamageEffectMaterial;
    public Material DamageEffectMaterial => m_DamageEffectMaterial;

    [SerializeField]
    private float m_DamageEffectDuration;
    public float DamageEffectDuration => m_DamageEffectDuration;

    [Header("撃破")]

    [SerializeField, Tooltip("撃破時の獲得スコア")]
    private int m_DefeatScore;
    public int DefeatScore => m_DefeatScore;

    [SerializeField, Tooltip("ドロップアイテム")]
    private ItemCreateParam m_DefeatItemParam;
    public ItemCreateParam DefeatItemParam => m_DefeatItemParam;

    [SerializeField, Tooltip("撃破時のイベント")]
    private BattleRealEventContent[] m_DefeatEvents;
    public BattleRealEventContent[] DefeatEvents => m_DefeatEvents;

    [SerializeField, Tooltip("撃破時の一連のエフェクト")]
    private SequentialEffectParamSet m_DefeatSequentialEffect;
    public SequentialEffectParamSet DefeatSequentialEffect => m_DefeatSequentialEffect;

    [SerializeField, Tooltip("敵非表示タイミング")]
    private float m_DefeatHideTime;
    public float DefeatHideTime => m_DefeatHideTime;
}