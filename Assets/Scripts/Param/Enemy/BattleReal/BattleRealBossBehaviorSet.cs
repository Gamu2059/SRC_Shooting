#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスの振る舞いのセットを保持するクラス。
/// </summary>
[Serializable]
public class BattleRealBossBehaviorSet
{
    [SerializeField, Tooltip("ボスの通常の振る舞い")]
    private BattleRealEnemyBehaviorUnit m_Behavior;
    public BattleRealEnemyBehaviorUnit Behavior => m_Behavior;

    [SerializeField, Tooltip("ボスのダウン時の振る舞い")]
    private BattleRealEnemyBehaviorUnit m_DownBehavior;
    public BattleRealEnemyBehaviorUnit DownBehavior => m_DownBehavior;

    [SerializeField, Tooltip("ハッキングを開始した時に読み込まれるレベルデータ")]
    private BattleHackingLevelParamSet m_HackingLevelParamSet;
    public BattleHackingLevelParamSet HackingLevelParamSet => m_HackingLevelParamSet;

    [SerializeField, Tooltip("この振る舞いでのダウンHP")]
    private int m_DownHp;
    public int DownHp => m_DownHp;

    [SerializeField, Tooltip("ダウンから復帰するまでの時間")]
    private float m_DownHealTime;
    public float DownHealTime => m_DownHealTime;

    [SerializeField, Tooltip("次の振る舞いセットへと遷移するHPの割合 これを下回ると次の振る舞いへと遷移する"), Range(0, 1)]
    private float m_HpRateThresholdNextBehavior;
    public float HpRateThresholdNextBehavior => m_HpRateThresholdNextBehavior;
}
