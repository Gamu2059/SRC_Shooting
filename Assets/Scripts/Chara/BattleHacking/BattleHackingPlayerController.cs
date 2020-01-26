using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ハッキングモードのプレイヤーコントローラ
/// </summary>
public class BattleHackingPlayerController : CommandCharaController
{
    public const string OUT_WALL_COLLIDE_NAME = "OUT WALL COLLIDE";
    public const string CRITICAL_COLLIDE_NAME = "CRITICAL COLLIDE";

    [SerializeField, Tooltip("弾を撃つ間隔")]
    private float m_ShotInterval = default;

    [SerializeField, Tooltip("弾を撃つ基準点")]
    private Transform[] m_ShotPositions = default;

    private float m_ShotTimeCount;

    public override void OnStart()
    {
        base.OnStart();

        m_ShotTimeCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_ShotTimeCount -= Time.deltaTime;
    }

    /// <summary>
    /// 通常弾を発射する。
    /// </summary>
    public void ShotBullet()
    {
        if (m_ShotTimeCount > 0)
        {
            return;
        }

        if (m_ShotPositions == null)
        {
            return;
        }

        AudioManager.Instance.Play(BattleHackingPlayerManager.Instance.ParamSet.ShotSe);

        var shotParam = new CommandBulletShotParam(this);
        for (int i = 0; i < m_ShotPositions.Length; i++)
        {
            shotParam.Position = m_ShotPositions[i].position - transform.parent.position;
            // 結構適当
            BattleHackingFreeTrajectoryBulletController.ShotBullet(
                shotParam,
                new ConstAcceleLinearTrajectory(
                    new SimpleTrajectory(
                        new TransformSimple(new Vector2(transform.localPosition.x, transform.localPosition.z), Calc.HALF_PI, 2),
                        0.5f),
                    5),
                0,
                new TrajectoryBasis(
                    new TransformSimple(new Vector2(transform.localPosition.x, transform.localPosition.z), Calc.HALF_PI, 2),
                    0.5F
                    ),
                null,
                true
                );
        }

        m_ShotTimeCount = m_ShotInterval;
    }

    protected override void OnEnterSufferBullet(HitSufferData<BattleHackingFreeTrajectoryBulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<CommandCharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);

        var selfColliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (selfColliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            Damage(1);
        }
    }

    public override void Dead()
    {
        if (BattleManager.Instance.m_PlayerNotDead)
        {
            return;
        }

        base.Dead();

        var paramSet = BattleHackingPlayerManager.Instance.ParamSet;

        AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER);
        BattleHackingEffectManager.Instance.CreateEffect(paramSet.DeadEffectParam, transform);
        BattleHackingManager.Instance.DeadPlayer();

        gameObject.SetActive(false);
    }
}
