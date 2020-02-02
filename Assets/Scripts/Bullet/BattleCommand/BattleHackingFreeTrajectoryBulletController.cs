using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// コマンドイベントの全ての弾オブジェクトの基礎クラス。
/// </summary>
[System.Serializable]
public class BattleHackingFreeTrajectoryBulletController : BattleHackingBulletController
{
    #region Field Inspector

    #endregion



    #region Field

    #endregion



    #region Getter & Setter

    #endregion



    /// <summary>
    /// 指定したパラメータを用いて弾を発射する。
    /// </summary>
    /// <param name="shotParam">発射時のパラメータ</param>
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static BattleHackingFreeTrajectoryBulletController ShotBullet(CommandBulletShotParam shotParam, SimpleTrajectory trajectoryBase, float dTime, TrajectoryBasis trajectoryBasis, TrajectoryBase trajectory, bool isPlayers, bool isCheck = true)
    {

        var bulletOwner = shotParam.BulletOwner;

        if (bulletOwner == null)
        {
            //Debug.Log("bulletOwner == null");
            return null;
        }

        // プレハブを取得
        var bulletPrefab = bulletOwner.GetBulletPrefab(shotParam.BulletIndex);

        if (bulletPrefab == null)
        {
            //Debug.Log("bulletPrefab == null");
            return null;
        }

        // プールから弾を取得
        var bullet = BattleHackingBulletManager.Instance.GetPoolingBullet(bulletPrefab);

        if (bullet == null)
        {
            //Debug.Log("bullet == null");
            return null;
        }

        bullet.SetTroop(bulletOwner.GetTroop());

        //ここまでがCreateBulletメソッドでしていた部分

        //if (bullet == null)
        //{
        //    return null;
        //}

        // BulletParamを取得
        //var bulletParam = shotParam.BulletOwner.GetBulletParam(shotParam.BulletParamIndex);
        //bullet.m_BulletParamIndex = shotParam.BulletParamIndex;

        //if (bulletParam != null)
        //{
        // 軌道を設定
        //bullet.ChangeOrbital(bulletParam.GetOrbitalParam(shotParam.OrbitalIndex));
        //bullet.m_OrbitalParamIndex = shotParam.OrbitalIndex;
        //}

        //bullet.m_BulletParam = bulletParam;

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }


        // 付け加えた部分

        //bullet.m_BasePosition = position;
        //bullet.m_Speed = bulletParam.OrbitalParam.Speed;

        // 本来は、どのクラスで初期化したかはこのクラス内からは見えない（？）
        bullet.m_TrajectoryBase = trajectoryBase;

        //switch (bullet.trajectoryBase)
        //{
        //    case ConstAcceleLinearMotion constAcceleLinearMotion:
        //        break;
        //}

        bullet.m_Time = dTime;

        bullet.m_TrajectoryBasis = trajectoryBasis;

        bullet.m_Trajectory = trajectory;

        bullet.m_IsPlayers = isPlayers;

        Debug.Log(bullet.m_TrajectoryBasis.m_Transform.m_Position);


        return bullet;
    }


    /// <summary>
    /// この弾を破棄する。
    /// </summary>
    public virtual void DestroyBullet()
    {
        DestroyAllTimer();

        if (m_Cycle == E_POOLED_OBJECT_CYCLE.UPDATE)
        {
            BattleHackingBulletManager.Instance.CheckPoolBullet(this);
        }
    }


    #region Game Cycle

    /// <summary>
    /// 発射してからの経過時間
    /// </summary>
    public float m_Time;

    /// <summary>
    /// 弾の軌道
    /// </summary>
    public SimpleTrajectory m_TrajectoryBase;


    /// <summary>
    /// 弾の軌道の基礎情報
    /// </summary>
    public TrajectoryBasis m_TrajectoryBasis;

    /// <summary>
    /// 弾の軌道
    /// </summary>
    public TrajectoryBase m_Trajectory;

    /// <summary>
    /// プレイヤーの弾かどうか
    /// </summary>
    public bool m_IsPlayers;



    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CharaHit.OnEnter = OnEnterHitChara;
        m_CharaHit.OnStay = OnStayHitChara;
        m_CharaHit.OnExit = OnExitHitChara;
    }

    public override void OnUpdate()
    {
        m_Time += Time.deltaTime;

        //TransformSimple transformSimple = m_TrajectoryBase.GetTransform(m_Time);
        TransformSimple transformSimple;

        // プレイヤーの弾なら
        if (m_IsPlayers)
        {
            transformSimple = new TransformSimple(
                m_TrajectoryBasis.m_Transform.m_Position + (m_TrajectoryBasis.m_Speed * m_Time + 5 * m_Time * m_Time / 2) * Calc.RThetaToVec2(1, m_TrajectoryBasis.m_Transform.m_Angle),
                m_TrajectoryBasis.m_Transform.m_Angle,
                m_TrajectoryBasis.m_Transform.m_Scale
                );
        }
        // 敵の弾なら
        else
        {
            // 軌道が等速直線運動なら
            if (m_Trajectory == null)
            {
                transformSimple = new TransformSimple(
                    m_TrajectoryBasis.m_Transform.m_Position + (m_TrajectoryBasis.m_Speed * m_Time) * Calc.RThetaToVec2(1, m_TrajectoryBasis.m_Transform.m_Angle),
                    m_TrajectoryBasis.m_Transform.m_Angle,
                    m_TrajectoryBasis.m_Transform.m_Scale
                    );
            }
            // 軌道が等速直線運動以外なら
            else
            {
                transformSimple = m_Trajectory.GetTransform(m_TrajectoryBasis, m_Time);
            }
        }

        transform.localPosition = new Vector3(transformSimple.m_Position.x, 0, transformSimple.m_Position.y);
        //transform.localEulerAngles = Calc.CalcEulerAngles(Vector3.zero, transformSimple.m_Angle);
        transform.localEulerAngles = new Vector3(0, 90 - transformSimple.m_Angle * Mathf.Rad2Deg, 0);
        transform.localScale = Vector3.one * transformSimple.m_Scale;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (BattleHackingBulletManager.Instance.IsOutOfBulletField(this))
        {
            DestroyBullet();
        }
    }

    #endregion

    #region Impl IColliderProcess

    #endregion

    #region Hit Chara

    /// <summary>
    /// 他のキャラに当たった時の処理。
    /// </summary>
    /// <param name="targetChara">他のキャラ</param>
    /// <param name="attackData">この弾の衝突情報</param>
    /// <param name="targetData">他のキャラの衝突情報</param>
    /// <param name="hitPosList">衝突座標リスト</param>
    public void HitChara(CommandCharaController targetChara, ColliderData attackData, ColliderData targetData, List<Vector2> hitPosList)
    {
        m_CharaHit.Put(targetChara, attackData, targetData, hitPosList);
    }

    protected virtual void OnEnterHitChara(HitSufferData<CommandCharaController> hitData)
    {
        if (m_IsDestoryOnCollide)
        {
            DestroyBullet();
        }
    }

    protected virtual void OnStayHitChara(HitSufferData<CommandCharaController> hitData)
    {

    }

    protected virtual void OnExitHitChara(HitSufferData<CommandCharaController> hitData)
    {

    }

    #endregion
}





///// <summary>
///// 指定したパラメータを用いて弾を生成する。
///// </summary>
///// <param name="shotParam">弾を発射させるパラメータ</param>
//private static BattleHackingFreeTrajectoryBulletController CreateBullet(CommandBulletShotParam shotParam)
//{
//    var bulletOwner = shotParam.BulletOwner;

//    if (bulletOwner == null)
//    {
//        return null;
//    }

//    // プレハブを取得
//    var bulletPrefab = bulletOwner.GetBulletPrefab(shotParam.BulletIndex);

//    if (bulletPrefab == null)
//    {
//        return null;
//    }

//    // プールから弾を取得
//    var bullet = BattleHackingBulletManager.Instance.GetPoolingBullet(bulletPrefab);

//    if (bullet == null)
//    {
//        return null;
//    }

//    bullet.SetTroop(bulletOwner.GetTroop());

//    return bullet;
//}


//transform.localPosition = Calc.RThetaToVec3(m_Time * 0.1f, m_Time);

//transform.localPosition = m_BasePosition + m_Time * transform.forward;
