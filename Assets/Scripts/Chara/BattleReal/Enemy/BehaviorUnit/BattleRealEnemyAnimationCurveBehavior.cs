#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// アニメーションカーブによって動く敵の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/EnemySequence/Unit/AnimationCurve", fileName = "animation_curve.behavior_unit.asset", order = 0)]
public class BattleRealEnemyAnimationCurveBehavior : BattleRealEnemyBehaviorUnit
{
    #region Field Inspector

    [Header("Move Param")]

    [SerializeField]
    private AnimationCurve m_SpeedCurve;
    public AnimationCurve SpeedCurve => m_SpeedCurve;

    [SerializeField, Tooltip("SpeedCurveにスケール補正を掛ける。判定させたい場合は-1等を指定するとよい。")]
    private float m_SpeedCurveScale = 1f;
    public float SpeedCurveScale => m_SpeedCurveScale;

    [SerializeField]
    private AnimationCurve m_AngleSpeedCurve;
    public AnimationCurve AngleSpeedCurve => m_AngleSpeedCurve;

    [SerializeField, Tooltip("AngleSpeedCurveにスケール補正を掛ける。判定させたい場合は-1等を指定するとよい。")]
    private float m_AngleSpeedCurveScale = 1f;
    public float AngleSpeedCurveScale => m_AngleSpeedCurveScale;

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

    protected float m_MoveTimeCount;
    protected float m_ShotTimeCount;
    protected float m_ShotStopTimeCount;
    protected bool m_IsCountOffset;
    protected float m_NowSpeed;
    protected float m_NowAngleSpeed;
    protected float m_Duration;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_IsCountOffset = true;
        m_ShotTimeCount = 0;
        m_ShotStopTimeCount = 0;
        m_MoveTimeCount = 0;

        m_NowSpeed = 0;
        m_NowAngleSpeed = 0;

        // 速度アニメーションの経過時間をキャッシュする
        m_Duration = SpeedCurve.Duration();

        // 移動方向へと向きが補正されるのでfalseにする
        Enemy.IsLookMoveDir = false;
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        Move();
        Shot();

        m_ShotTimeCount += deltaTime;
        m_MoveTimeCount += deltaTime;

        if (!m_IsCountOffset)
        {
            m_ShotStopTimeCount += deltaTime;
        }
    }

    protected override bool IsEnd()
    {
        return m_MoveTimeCount >= m_Duration;
    }

    #endregion

    protected virtual void Move()
    {
        m_NowSpeed = SpeedCurveScale * SpeedCurve.Evaluate(m_MoveTimeCount);
        m_NowAngleSpeed = AngleSpeedCurveScale * AngleSpeedCurve.Evaluate(m_MoveTimeCount);

        var transform = Enemy.transform;
        transform.position = transform.forward * m_NowSpeed * Time.deltaTime + transform.position;
        var angle = transform.eulerAngles;
        angle.y += m_NowAngleSpeed * Time.deltaTime;
        transform.eulerAngles = angle;
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
