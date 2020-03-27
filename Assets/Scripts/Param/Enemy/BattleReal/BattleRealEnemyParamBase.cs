#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードの敵のパラメータの基底クラス。
/// </summary>
public abstract class BattleRealEnemyParamBase
{
    [Header("基本パラメータ")]

    [SerializeField, Tooltip("体力")]
    private int m_Hp;
    public int Hp => m_Hp;

    [SerializeField, Tooltip("扱う弾")]
    private BulletSetParam m_BulletSetParam;
    public BulletSetParam BulletSetParam => m_BulletSetParam;

    [SerializeField, Tooltip("見た目")]
    private BattleRealEnemyLookParamSet m_EnemyLookParamSet;
    public BattleRealEnemyLookParamSet EnemyLookParamSet => m_EnemyLookParamSet;

    [Header("被弾パラメータ")]

    [SerializeField]
    private Material m_DamageEffectMaterial;
    public Material DamageEffectMaterial => m_DamageEffectMaterial;

    [SerializeField]
    private float m_DamageEffectDuration;
    public float DamageEffectDuration => m_DamageEffectDuration;

    public abstract int DefeatScore { get; }
    public abstract ItemCreateParam DefeatItemParam { get; }
    public abstract BattleRealEventContent[] DefeatEvents { get; }
    public abstract SequentialEffectParamSet DefeatSequentialEffect { get; }
    public abstract float DefeatHideTime { get; }
    public abstract BattleRealEventContent[] RetireEvents { get; }
}
