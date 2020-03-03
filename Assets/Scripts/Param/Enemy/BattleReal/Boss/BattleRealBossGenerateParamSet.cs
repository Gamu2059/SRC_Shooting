#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Boss/BossGenerate", fileName = "param.battle_real_boss_generate.asset")]
public class BattleRealBossGenerateParamSet : BattleRealEnemyGenerateParamSet
{
    [Header("ダウン")]

    [SerializeField, Tooltip("何回ハッキングに成功すれば\"救出\"になるのか")]
    private int m_HackingCompleteNum;
    public int HackingCompleteNum => m_HackingCompleteNum;

    [SerializeField, Tooltip("ダウンHP ハッキング成功回数に応じて変わってくる可能性があるので配列化")]
    private float[] m_DownHpArray;
    public float[] DownHpArray => m_DownHpArray;

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

    [SerializeField, Tooltip("攻撃の行動パターンが変化するHPの割合")]
    private List<float> m_ChangeAttackHpRates;
    public List<float> ChangeAttackHpRates => m_ChangeAttackHpRates;

    [SerializeField, Tooltip("ハッキング成功時にばらまくアイテムのパラメータ")]
    private ItemCreateParam m_HackingSuccessItemParam;
    public ItemCreateParam HackingSuccessItemParam => m_HackingSuccessItemParam;

    [Header("救出")]

    [SerializeField, Tooltip("救出時の獲得スコア")]
    private int m_RescueScore;
    public int RescueScore => m_RescueScore;

    [SerializeField, Tooltip("ドロップアイテム")]
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
}
