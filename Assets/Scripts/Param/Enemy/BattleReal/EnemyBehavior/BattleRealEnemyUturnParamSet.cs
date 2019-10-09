using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/Enemy/EnemyBehavior/Uturn", fileName = "behavior.battle_real_enemy_uturn.asset")]
public class BattleRealEnemyUturnParamSet : BattleRealEnemyBehaviorParamSet
{
    [Header("Move Param")]

    [SerializeField, Tooltip("初期出現地点に対して、相対座標で直進終了地点を定める")]
    private Vector3 m_RelativeStraightMoveEndPosition;
    public Vector3 RelativeStraightMoveEndPosition => m_RelativeStraightMoveEndPosition;

    [SerializeField, Tooltip("直進時の移動速度")]
    private float m_StraightMoveSpeed;
    public float StraightMoveSpeed => m_StraightMoveSpeed;

    [SerializeField, Tooltip("直進終点の相対座標で円の中心点を定める")]
    private Vector3 m_RelativeCircleCenterPosition;
    public Vector3 RelativeCircleCenterPosition => m_RelativeCircleCenterPosition;

    [SerializeField, Tooltip("直進から切り替わった時に円周上をどのくらいの角度移動するか(Degree)")]
    private float m_CircleMoveAngle;
    public float CircleMoveAngle => m_CircleMoveAngle;

    [SerializeField, Tooltip("円周移動時の移動角速度")]
    private float m_CircleMoveSpeed;
    public float CircleMoveSpeed => m_CircleMoveSpeed;

    [Header("Shot Param")]

    [SerializeField, Tooltip("画面の見える位置に入ってから何秒後に発射するか")]
    private float m_VisibleOffsetShotTime;
    public float VisibleOffsetShotTime => m_VisibleOffsetShotTime;

    [SerializeField, Tooltip("直進時の発射パラメータ")]
    private EnemyShotParam m_StraightMoveShotParam;
    public EnemyShotParam StraightMoveShotParam => m_StraightMoveShotParam;

    [SerializeField, Tooltip("カーブ時の発射パラメータ")]
    private EnemyShotParam m_CircleMoveShotParam;
    public EnemyShotParam CircleMoveShotParam => m_CircleMoveShotParam;
}
