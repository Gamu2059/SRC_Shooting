#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの基本的なパラメータのセット
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Boss/BaseBehavior", fileName = "param.real_boss_base_behavior.asset")]
public class BattleRealBossBehaviorParamSet : BattleRealEnemyBehaviorParamSet
{
    [SerializeField, Tooltip("攻撃の行動パターンの配列")]
    private BattleRealBossBehaviorUnitParamSet[] m_AttackParamSets;
    public BattleRealBossBehaviorUnitParamSet[] AttackParamSets => m_AttackParamSets;

    [SerializeField, Tooltip("ダウンの行動パターンの配列")]
    private BattleRealBossBehaviorUnitParamSet[] m_DownParamSets;
    public BattleRealBossBehaviorUnitParamSet[] DownParamSets => m_DownParamSets;

    [SerializeField]
    private float m_DownHp;
    public float DownHp => m_DownHp;

    [SerializeField, Tooltip("何回ハッキングに成功すれば\"救出\"になるのか")]
    private int m_HackingCompleteNum;
    public int HackingCompleteNum => m_HackingCompleteNum;

    [SerializeField, Tooltip("攻撃の行動パターンが変化するHPの割合")]
    private List<float> m_ChangeAttackHpRates;
    public List<float> ChangeAttackHpRates => m_ChangeAttackHpRates;

    [SerializeField, Tooltip("ハッキング成功時にばらまくアイテムのパラメータ")]
    private ItemCreateParam m_ItemParam;
    public ItemCreateParam ItemParam => m_ItemParam;
}
