#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : BattleRealPlayerController
{
    private const float INVINSIBLE_DURATION = 5f;
    private const string INVINSIBLE_KEY = "Invinsible";

    #region Field Inspector

    [SerializeField]
    private Transform[] m_MainShotPosition;

    [SerializeField, Range(0f, 1f)]
    private float m_ShotInterval;

    [SerializeField]
    private Animator m_ShieldAnimator;

    [SerializeField]
    private Transform m_Critical;

    [SerializeField]
    private Transform m_Shield;

    #endregion

    #region Field

    private float shotDelay;

    private BattleRealEffectController m_ChargeEffect;
    private BulletController m_Laser;
    private BulletController m_Bomb;

    #endregion

    public override void OnInitialize()
    {
        base.OnInitialize();

        SetEnableCollider(true);
        m_Shield.gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        shotDelay += Time.deltaTime;
    }

    public override void ShotBullet()
    {
        base.ShotBullet();

        if (shotDelay >= m_ShotInterval)
        {
            var level = DataManager.Instance.BattleData.Level;

            for (int i = 0; i < m_MainShotPosition.Length; i++)
            {
                var shotParam = new BulletShotParam(this);
                shotParam.Position = m_MainShotPosition[i].transform.position;
                var bullet = BulletController.ShotBullet(shotParam);
                // 現状は、レベルの値を攻撃力にしてみる
                bullet.SetNowDamage(level + 1, E_RELATIVE.ABSOLUTE);
            }
            shotDelay = 0;
        }
    }

    public override void ChargeStart()
    {
        base.ChargeStart();

        if (m_ChargeEffect == null || m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.POOLED)
        {
            var paramSet = BattleRealPlayerManager.Instance.ParamSet;
            m_ChargeEffect = BattleRealEffectManager.Instance.GetPoolingBullet(paramSet.ChargePrefab, transform);
            if (m_ChargeEffect != null)
            {
                m_ChargeEffect.IsAllowOwner = true;
                m_ChargeEffect.RelatedAllowPos = paramSet.ChargeRelatedPos;
            }
        }
    }

    public override void ChargeRelease()
    {
        base.ChargeRelease();
        BattleRealManager.Instance.RequestChangeState(E_BATTLE_REAL_STATE.CHARGE_SHOT_PERFORMANCE);
    }

    public override void ShotLaser()
    {
        base.ShotLaser();

        if (m_Laser != null && m_Laser.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED)
        {
            return;
        }

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestoryEffect(true);
        }

        var param = new BulletShotParam(this);
        param.Position = m_MainShotPosition[0].transform.position;
        m_Laser = BulletController.ShotBullet(param, true);

        // 現状は、レベルの値を攻撃力にしてみる
        var level = DataManager.Instance.BattleData.Level;
        m_Laser.SetNowDamage(level + 1, E_RELATIVE.ABSOLUTE);
    }

    public override void ShotBomb()
    {
        base.ShotBomb();

        if (m_Bomb != null && m_Bomb.GetCycle() != E_POOLED_OBJECT_CYCLE.POOLED)
        {
            return;
        }

        if (m_ChargeEffect != null && m_ChargeEffect.Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            m_ChargeEffect.DestoryEffect(true);
        }

        var param = new BulletShotParam(this);
        param.BulletIndex = 1;
        param.BulletParamIndex = 1;
        m_Bomb = BulletController.ShotBullet(param, true);

        // 現状は、レベルの値を攻撃力にしてみる
        var level = DataManager.Instance.BattleData.Level;
        m_Bomb.SetNowDamage((level + 1) * 100, E_RELATIVE.ABSOLUTE);
    }

    public override void SetInvinsible()
    {
        DestroyTimer(INVINSIBLE_KEY);
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, INVINSIBLE_DURATION);
        timer.SetTimeoutCallBack(() =>
        {
            timer = null;
            SetEnableCollider(true);
            m_Shield.gameObject.SetActive(false);
        });
        RegistTimer(INVINSIBLE_KEY, timer);

        m_Shield.gameObject.SetActive(true);
        m_ShieldAnimator.Play("battle_real_player_shield", 0);
        SetEnableCollider(false);
    }

    private void SetEnableCollider(bool isEnable)
    {
        var c = GetCollider();

        // 被弾判定と無敵判定は反対の関係
        c.SetEnableCollider(m_Critical, isEnable);
        c.SetEnableCollider(m_Shield, !isEnable);
    }
}
