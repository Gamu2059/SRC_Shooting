#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Sinカーブで動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Unit/SinCurve", fileName = "sin_curve.enemy_behavior_unit.asset", order = 20)]
public class BattleRealEnemySinCurveBehavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector

    [Header("Move Param")]

    [SerializeField, Tooltip("カーブの軸となる角度(度数法)。 0なら上、90なら右、180なら下、270なら左")]
    private float m_AxisAngle;
    public float AxisAngle => m_AxisAngle;

    [SerializeField, Tooltip("振幅")]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField, Tooltip("周波数")]
    private float m_Frequency;
    public float Frequency => m_Frequency;

    [SerializeField, Tooltip("初期位相(度数法)")]
    private float m_InitPhaseAngle;
    public float InitPhaseAngle => m_InitPhaseAngle;

    [SerializeField, Tooltip("1周期で進む距離")]
    private float m_WaveLength;
    public float WaveLength => m_WaveLength;

    [SerializeField, Tooltip("継続時間")]
    private float m_Duration;
    public float Duration => m_Duration;

    [Header("Shot Param")]

    [SerializeField, Tooltip("出現から最初の発射までのオフセット")]
    private float m_ShotOffset;
    public float ShotOffset => m_ShotOffset;

    [SerializeField, Tooltip("「最初に発射してから」撃ち止めるまでの時間")]
    private float m_ShotStop;
    public float ShotStop => m_ShotStop;

    [SerializeField]
    private EnemyShotParam m_ShotParam;
    public EnemyShotParam ShotParam => m_ShotParam;

    #endregion

    #region Field

    protected Vector3 m_StartPosition;
    protected float m_InitPhase;
    protected float m_Sin;
    protected float m_Cos;
    protected float m_ShotStopTimeCount;
    protected float m_ShotTimeCount;
    protected bool m_IsCountOffset;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_StartPosition = Enemy.transform.position;
        m_InitPhase = InitPhaseAngle * Mathf.Deg2Rad;
        var rad = (-AxisAngle + 90) * Mathf.Deg2Rad;
        m_Sin = Mathf.Sin(rad);
        m_Cos = Mathf.Cos(rad);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        Move();
        Shot();

        m_ShotTimeCount += deltaTime;

        if (!m_IsCountOffset)
        {
            m_ShotStopTimeCount += deltaTime;
        }
    }

    protected override bool IsEnd()
    {
        return CurrentTime >= Duration;
    }

    #endregion

    protected virtual void Move()
    {
        var x = WaveLength * Frequency * CurrentTime;
        var y = Amplitude * Mathf.Sin(Frequency * CurrentTime + m_InitPhase);
        var deltaPos = Vector3.zero;
        deltaPos.x = x * m_Cos - y * m_Sin;
        deltaPos.z = x * m_Sin + y * m_Cos;
        Enemy.transform.position = m_StartPosition + deltaPos;
    }

    protected virtual void Shot()
    {
        if (m_IsCountOffset)
        {
            if (m_ShotTimeCount >= ShotOffset)
            {
                m_ShotTimeCount = ShotParam.Interval;
                m_IsCountOffset = false;
            }
        }
        else
        {
            if (m_ShotStopTimeCount < ShotStop && m_ShotTimeCount >= ShotParam.Interval)
            {
                m_ShotTimeCount = 0;
                OnShot(ShotParam);
            }
        }
    }

    protected virtual void OnShot(EnemyShotParam param)
    {
        var transform = Enemy.transform;
        int num = param.Num;
        float angle = param.Angle;
        var spreadAngles = BattleRealCharaController.GetBulletSpreadAngles(num, angle);
        var shotParam = new BulletShotParam(Enemy);
        shotParam.Position = transform.position;
        shotParam.Rotation = transform.eulerAngles;

        var correctAngle = 0f;
        if (param.IsPlayerLook)
        {
            var player = BattleRealPlayerManager.Instance.Player;
            var delta = player.transform.position - transform.position;
            correctAngle = Mathf.Atan2(delta.x, delta.z) * Mathf.Rad2Deg;
        }

        for (int i = 0; i < num; i++)
        {
            var bullet = BulletController.ShotBullet(shotParam);
            bullet.SetRotation(new Vector3(0, spreadAngles[i] + correctAngle, 0));
        }

        AudioManager.Instance.Play(BattleRealEnemyManager.Instance.ParamSet.Shot01Se);
    }
}
