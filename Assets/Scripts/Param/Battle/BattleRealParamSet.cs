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
    private EventTriggerParamSet m_EventTriggerParamSet;
    public EventTriggerParamSet EventTriggerParamSet => m_EventTriggerParamSet;
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