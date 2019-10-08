using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BattleRealParamSet
{
    [SerializeField]
    private BattleRealPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleRealPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;

    [SerializeField]
    private BattleRealEnemyManagerParamSet m_EnemyManagerParamSet;
    public BattleRealEnemyManagerParamSet EnemyManagerParamSet => m_EnemyManagerParamSet;

    [SerializeField]
    private BattleRealBulletManagerParamSet m_BulletManagerParamSet;
    public BattleRealBulletManagerParamSet BulletManagerParamSet => m_BulletManagerParamSet;

    [SerializeField]
    private BattleRealEventTriggerParamSet m_EventTriggerParamSet;
    public BattleRealEventTriggerParamSet EventTriggerParamSet => m_EventTriggerParamSet;
}

[Serializable]
public class BattleRealPlayerManagerParamSet
{
    [SerializeField, Tooltip("プレイヤーのプレハブ")]
    private BattleRealPlayerController m_PlayerPrefab;
    public BattleRealPlayerController PlayerPrefab => m_PlayerPrefab;

    [SerializeField, Tooltip("ゲーム開始時のプレイヤーの最初の位置")]
    private Vector2 m_InitAppearViewportPosition;
    public Vector2 InitAppearViewportPosition => m_InitAppearViewportPosition;

    [SerializeField, Tooltip("プレイヤーの基本移動速度")]
    private float m_PlayerBaseMoveSpeed;
    public float PlayerBaseMoveSpeed => m_PlayerBaseMoveSpeed;

    [SerializeField, Tooltip("プレイヤーの低速移動速度")]
    private float m_PlayerSlowMoveSpeed;
    public float PlayerSlowMoveSpeed => m_PlayerSlowMoveSpeed;
}

[Serializable]
public class BattleRealEnemyManagerParamSet
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;

    [SerializeField, Tooltip("敵グループの生成パラメータ")]
    private BattleRealEnemyGroupGenerator m_Generator;
    public BattleRealEnemyGroupGenerator Generator => m_Generator;

    [SerializeField, Tooltip("このステージで登場する敵データ")]
    private StageEnemyParam m_StageEnemyParam;
    public StageEnemyParam StageEnemyParam => m_StageEnemyParam;

    [SerializeField, Tooltip("このステージで使用する敵出現データ")]
    private XlBattleMainEnemyParam m_EnemyParam;
    public XlBattleMainEnemyParam EnemyParam => m_EnemyParam;
}

[Serializable]
public class BattleRealBulletManagerParamSet
{
    [SerializeField, Tooltip("左下のオフセットフィールド")]
    private Vector2 m_MinOffsetFieldPosition;
    public Vector2 MinOffsetFieldPosition => m_MinOffsetFieldPosition;

    [SerializeField, Tooltip("右上のオフセットフィールド")]
    private Vector2 m_MaxOffsetFieldPosition;
    public Vector2 MaxOffsetFieldPosition => m_MaxOffsetFieldPosition;
}
