﻿#pragma warning disable 0649

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
    private int m_Score;
    public int Score => m_Score;

    [SerializeField, Tooltip("ドロップアイテム")]
    private ItemCreateParam m_ItemCreateParam;
    public ItemCreateParam ItemCreateParam => m_ItemCreateParam;

    [SerializeField, Tooltip("撃破時のイベント")]
    private BattleRealEventContent[] m_DefeatEvents;
    public BattleRealEventContent[] DefeatEvents => m_DefeatEvents;

    [SerializeField, Tooltip("撃破時エフェクト")]
    private ParticleController m_DefeatEffect;
    public ParticleController DefeatEffect => m_DefeatEffect;
}