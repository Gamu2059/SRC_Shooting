#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleReal.EnemyGenerator;

/// <summary>
/// 敵グループの生成パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemyGroup/Param", fileName = "param.enemy_group.asset")]
public class BattleRealEnemyGroupParam : ScriptableObject
{
    [SerializeField, Tooltip("敵グループの生成ビューポート座標")]
    private Vector2 m_ViewPortPos;
    public Vector2 ViewPortPos => m_ViewPortPos;

    [SerializeField, Tooltip("敵グループのビュポート座標からのオフセット座標")]
    private Vector3 m_OffsetPosFromViewPort;
    public Vector3 OffsetPosFromViewPort => m_OffsetPosFromViewPort;

    [SerializeField, Tooltip("敵グループの生成角度")]
    private float m_GenerateAngle;
    public float GenerateAngle => m_GenerateAngle;

    [SerializeField, Tooltip("敵の生成の仕方")]
    private BattleRealEnemyGeneratorBase m_EnemyGenerator;
    public BattleRealEnemyGeneratorBase EnemyGenerator => m_EnemyGenerator;

    [SerializeField, Tooltip("敵グループの振る舞い")]
    private BattleRealEnemyGroupBehaviorUnitBase m_Behavior;
    public BattleRealEnemyGroupBehaviorUnitBase Behavior => m_Behavior;

    [SerializeField, Tooltip("敵グループが生成された時に実行されるイベント")]
    private BattleRealEventContent[] m_OnGenerateGroupEvents;
    public BattleRealEventContent[] OnGenerateGroupEvents => m_OnGenerateGroupEvents;
}
