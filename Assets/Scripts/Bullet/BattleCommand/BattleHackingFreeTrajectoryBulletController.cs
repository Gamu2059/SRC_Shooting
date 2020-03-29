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
    /// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
    public static BattleHackingFreeTrajectoryBulletController ShotBullet(
        CommandCharaController shotOwner,
        float dTime,
        ShotParamOperation shotParamOperation,
        BulletParamFreeOperation bulletParamFreeOperation,
        OperationFloatVariable timeOperation,
        ShotParamOperationVariable launchParam,
        TransformOperation transformOperation,
        bool isCheck = true
        )
    {

        var bulletOwner = new CommandBulletShotParam(shotOwner).BulletOwner;

        if (bulletOwner == null)
        {
            return null;
        }

        // プレハブを取得


        // 付け足した部分

        int bulletIndex;

        // 今まで通りなら
        if (shotParamOperation != null)
        {
            bulletIndex = shotParamOperation.BulletIndex.GetResultInt();
        }
        // 新しいやり方なら
        else
        {
            bulletIndex = 0;
        }


        var bulletPrefab = bulletOwner.GetBulletPrefab(bulletIndex);

        if (bulletPrefab == null)
        {
            return null;
        }

        // プールから弾を取得
        var bullet = BattleHackingBulletManager.Instance.GetPoolingBullet(bulletPrefab);

        if (bullet == null)
        {
            return null;
        }

        bullet.SetTroop(bulletOwner.GetTroop());

        //ここまでがCreateBulletメソッドでしていた部分

        if (isCheck)
        {
            BattleHackingBulletManager.Instance.CheckStandbyBullet(bullet);
        }


        bullet.m_ShotParam = new ShotParam(shotParamOperation);


        if (bulletParamFreeOperation == null) Debug.Log("bulletParamFreeOperation");

        bullet.m_BulletParamFree = bulletParamFreeOperation.GetResultBulletParamFree();


        bullet.m_Time = dTime;

        bullet.m_TimeOperation = timeOperation;

        bullet.m_LaunchParam = launchParam;

        bullet.m_TransformOperation = transformOperation;

        bullet.spriteRenderer = (SpriteRenderer)bullet.gameObject.GetComponentInChildren(typeof(SpriteRenderer));

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
    /// 発射してからの経過時間（保存用）
    /// </summary>
    private float m_Time;

    /// <summary>
    /// 発射からの時刻を表す変数（入力用）
    /// </summary>
    private OperationFloatVariable m_TimeOperation;

    /// <summary>
    /// 発射時の発射パラメータ（保存用）
    /// </summary>
    private ShotParam m_ShotParam;


    /// <summary>
    /// 弾のパラメータ（保存用）
    /// </summary>
    private BulletParamFree m_BulletParamFree;


    /// <summary>
    /// 発射時のパラメータを表す変数（入力用）
    /// </summary>
    private ShotParamOperationVariable m_LaunchParam;

    /// <summary>
    /// 弾の物理的な状態（取得用）
    /// </summary>
    private TransformOperation m_TransformOperation;

    /// <summary>
    /// 衝突判定があるかどうか
    /// </summary>
    public bool HasCollision { get; private set; }

    /// <summary>
    /// この弾に対応するスプライトレンダラー
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// ゲーム全体で共通の変数
    /// </summary>
    public static CommonOperationVariable CommonOperationVar { get; set; }


    public override void OnInitialize()
    {
        base.OnInitialize();

        m_CharaHit.OnEnter = OnEnterHitChara;
        m_CharaHit.OnStay = OnStayHitChara;
        m_CharaHit.OnExit = OnExitHitChara;
    }

    public override void OnUpdate()
    {
        // 時刻を更新する
        m_Time += Time.deltaTime;

        // この弾の物理的な状態
        TransformSimple transformSimple;

        // 軌道が等速直線運動なら
        if (m_TransformOperation == null)
        {
            // 発射パラメータのみによってこの弾の物理的な状態を決定する
            transformSimple = m_ShotParam.GetTransformInertially(m_Time);
        }
        // 軌道が等速直線運動以外なら
        else
        {
            // 時刻を外部に反映させる
            m_TimeOperation.Value = m_Time;

            // 時刻をstatic変数に反映させる
            BulletTime.m_Time = m_Time;

            // 発射パラメータを外部に反映させる
            m_LaunchParam.SetValue(m_ShotParam);


            // bool型のパラメータをstatic変数に反映させる
            BulletBool.BoolArray = m_BulletParamFree.m_Bool;

            // int型のパラメータをstatic変数に反映させる
            BulletInt.IntArray = m_BulletParamFree.m_Int;

            // float型のパラメータをstatic変数に反映させる
            BulletFloat.FloatArray = m_BulletParamFree.m_Float;

            // Vector2型のパラメータをstatic変数に反映させる
            BulletVector2.Vector2Array = m_BulletParamFree.m_Vector2;


            // この弾の物理的な状態を外部の演算により求める（それぞれのパラメータがもしnullなら、それについては慣性に従って求める）
            transformSimple = m_TransformOperation.GetResultTransform(m_ShotParam, m_Time);
        }

        // 実際にこの弾の物理的な状態を更新する

        transform.localPosition = new Vector3(transformSimple.Position.x, 0, transformSimple.Position.y);
        transform.localEulerAngles = new Vector3(0, 90 - transformSimple.Angle * Mathf.Rad2Deg, 0);
        transform.localScale = Vector3.one * transformSimple.Scale;

        spriteRenderer.color = new Color(1, 1, 1, transformSimple.Opacity);
        HasCollision = transformSimple.CanCollide;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        //if (BattleHackingBulletManager.Instance.IsOutOfBulletField(this))
        //{
        //    DestroyBullet();
        //}

        Vector3 position = transform.localPosition;

        if (
            position.x < CommonOperationVar.PositionXMin.GetResultFloat() || CommonOperationVar.PositionXMax.GetResultFloat() < position.x ||
            position.z < CommonOperationVar.PositionYMin.GetResultFloat() || CommonOperationVar.PositionYMax.GetResultFloat() < position.z
            )
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


//TransformSimple transformSimple = m_TrajectoryBase.GetTransform(m_Time);

//m_LaunchPosition.Value = m_ShotParam.ShotPosition;
//m_LaunchAngle.Value = m_ShotParam.Angle;
//m_LaunchScale.Value = m_ShotParam.Scale;
//m_LaunchSpeed.Value = m_ShotParam.Speed;

//m_LaunchParam.Position.Value = m_ShotParam.ShotPosition;
//m_LaunchParam.Angle.Value = m_ShotParam.Angle;
//m_LaunchParam.Scale.Value = m_ShotParam.Scale;
//m_LaunchParam.Speed.Value = m_ShotParam.Speed;


///// <summary>
///// 発射時の位置を表す変数（入力用）
///// </summary>
//public OperationVector2Variable m_LaunchPosition;

///// <summary>
///// 発射時の角度を表す変数（入力用）
///// </summary>
//public OperationFloatVariable m_LaunchAngle;

///// <summary>
///// 発射時の大きさを表す変数（入力用）
///// </summary>
//public OperationFloatVariable m_LaunchScale;

///// <summary>
///// 発射時の速さを表す変数（入力用）
///// </summary>
//public OperationFloatVariable m_LaunchSpeed;


//bullet.m_LaunchPosition = launchPosition;
//bullet.m_LaunchAngle = launchAngle;
//bullet.m_LaunchScale = launchScale;
//bullet.m_LaunchSpeed = launchSpeed;


//OperationVector2Variable launchPosition,
//OperationFloatVariable launchAngle,
//OperationFloatVariable launchScale,
//OperationFloatVariable launchSpeed,


//transformSimple = new TransformSimple(
//    m_TrajectoryBasis.m_Transform.m_Position + (m_TrajectoryBasis.m_Speed * m_Time) * Calc.RThetaToVec2(1, m_TrajectoryBasis.m_Transform.m_Angle),
//    m_TrajectoryBasis.m_Transform.m_Angle,
//    m_TrajectoryBasis.m_Transform.m_Scale
//    );


//SimpleTrajectory trajectoryBase,

//TrajectoryBasis trajectoryBasis,
//TrajectoryBase trajectory,
//bool isPlayers,


// 付け加えた部分

//bullet.m_BasePosition = position;
//bullet.m_Speed = bulletParam.OrbitalParam.Speed;

//// 本来は、どのクラスで初期化したかはこのクラス内からは見えない（？）
//bullet.m_TrajectoryBase = trajectoryBase;

//switch (bullet.trajectoryBase)
//{
//    case ConstAcceleLinearMotion constAcceleLinearMotion:
//        break;
//}

//bullet.m_TrajectoryBasis = trajectoryBasis;

//bullet.m_Trajectory = trajectory;

//bullet.m_IsPlayers = isPlayers;


///// <summary>
///// 弾の軌道
///// </summary>
//public SimpleTrajectory m_TrajectoryBase;

///// <summary>
///// 弾の軌道の基礎情報
///// </summary>
//public TrajectoryBasis m_TrajectoryBasis;

///// <summary>
///// 弾の軌道
///// </summary>
//public TrajectoryBase m_Trajectory;

///// <summary>
///// プレイヤーの弾かどうか
///// </summary>
//public bool m_IsPlayers;


//// プレイヤーの弾なら
//if (false)
//{
//    //transformSimple = new TransformSimple(
//    //    m_TrajectoryBasis.m_Transform.m_Position + (m_TrajectoryBasis.m_Speed * m_Time + 5 * m_Time * m_Time / 2) * Calc.RThetaToVec2(1, m_TrajectoryBasis.m_Transform.m_Angle),
//    //    m_TrajectoryBasis.m_Transform.m_Angle,
//    //    m_TrajectoryBasis.m_Transform.m_Scale
//    //    );
//}
//// 敵の弾なら
//else
//{


//transform.localEulerAngles = Calc.CalcEulerAngles(Vector3.zero, transformSimple.m_Angle);


//transformSimple = new TransformSimple(
//    m_ShotParam.Position + m_ShotParam.Velocity * m_Time,
//    m_ShotParam.Angle + m_ShotParam.AngleSpeed * m_Time,
//    m_ShotParam.Scale + m_ShotParam.ScaleSpeed * m_Time
//    );


//// 弾の不透明度を更新する
//float alpha;
//if (m_Time <= 0.2F)
//{
//    alpha = m_Time * 5;
//}
//else
//{
//    alpha = 1;
//}
//spriteRenderer.color = new Color(1, 1, 1, alpha);

//// 衝突判定があるかどうかを更新する
//HasCollision = 1 < m_Time;


///// <param name="shotParam">発射時のパラメータ</param>

//var bulletOwner = shotParam.BulletOwner;

//var bulletPrefab = bulletOwner.GetBulletPrefab(shotParam.BulletIndex);


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
