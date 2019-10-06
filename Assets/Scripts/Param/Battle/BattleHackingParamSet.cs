using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BattleHackingParamSet
{
    [SerializeField]
    private BattleHackingPlayerManagerParamSet m_PlayerManagerParamSet;
    public BattleHackingPlayerManagerParamSet PlayerManagerParamSet => m_PlayerManagerParamSet;
}

[Serializable]
public class BattleHackingPlayerManagerParamSet
{
    [SerializeField, Tooltip("プレイヤーのプレハブ")]
    private BattleHackingPlayerController m_PlayerPrefab;
    public BattleHackingPlayerController PlayerPrefab => m_PlayerPrefab;

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