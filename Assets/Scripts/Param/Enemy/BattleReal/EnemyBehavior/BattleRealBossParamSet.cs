using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの基本的なパラメータのセット
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Boss/BaseBehavior", fileName = "param.boss_base_behavior.asset")]
public class BattleRealBossParamSet : BattleRealEnemyBehaviorParamSet
{
    [SerializeField, Tooltip("攻撃の行動パターンの配列")]
    private BattleRealBossBehaviorParamSet[] m_AttackParamSets;
    public BattleRealBossBehaviorParamSet[] AttackParamSets => m_AttackParamSets;

    [SerializeField, Tooltip("ダウンの行動パターンの配列")]
    private BattleRealBossBehaviorParamSet[] m_DownParamSets;
    public BattleRealBossBehaviorParamSet[] DownParamSets => m_DownParamSets;

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
