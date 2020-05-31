#pragma warning disable 0649

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

    //[SerializeField, Tooltip("弾を撃つ間隔")]
    //private float m_ShotInterval = default;

    [SerializeField, Tooltip("弾を撃つ基準点")]
    private Transform[] m_ShotPositions = default;


    [SerializeField, Tooltip("発射位置を表す変数オブジェクト")]
    private OperationVector2Variable m_ShotPosition1;

    [SerializeField, Tooltip("発射位置を表す変数オブジェクト")]
    private OperationVector2Variable m_ShotPosition2;

    [SerializeField, Tooltip("ショットボタンが押されているかどうか")]
    private OperationBoolVariable m_IsShotButtonPressed;

    [SerializeField, Tooltip("弾発射パラメータ群")]
    private BulletShotParams m_BulletShotParams;

    /// <summary>
    /// ハッキング開始からの時刻
    /// </summary>
    private float m_Time;


    public override void OnStart()
    {
        base.OnStart();

        m_Time = 0;
        BulletTime.Time = 0;

        m_BulletShotParams.OnStarts();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        m_Time += Time.deltaTime;
        BulletTime.Time = m_Time;

        // 自機本体の位置を外部に知らせる
        m_ShotPosition1.Value = new Vector2(0, transform.localPosition.z) + new Vector2(m_ShotPositions[0].position.x, 0);
        m_ShotPosition2.Value = new Vector2(0, transform.localPosition.z) + new Vector2(m_ShotPositions[1].position.x, 0);

        m_BulletShotParams.OnUpdates(this);

        m_IsShotButtonPressed.Value = false;
    }

    /// <summary>
    /// 通常弾を発射する。
    /// </summary>
    public void ShotBullet()
    {
        m_IsShotButtonPressed.Value = true;
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
        //if (BattleManager.Instance.m_PlayerNotDead)
        //{
        //    return;
        //}

        base.Dead();

        var paramSet = BattleHackingPlayerManager.Instance.ParamSet;

        AudioManager.Instance.Stop(E_CUE_SHEET.PLAYER);
        BattleHackingEffectManager.Instance.CreateEffect(paramSet.DeadEffectParam, transform);
        BattleHackingPlayerManager.Instance.DeadPlayer();

        gameObject.SetActive(false);
    }
}




//new ConstAcceleLinearTrajectory1(
//    new SimpleTrajectory(
//        new TransformSimple(new Vector2(transform.localPosition.x, transform.localPosition.z), Calc.HALF_PI, 2),
//        0.5f),
//    5),


//new TrajectoryBasis(
//    new TransformSimple(new Vector2(transform.localPosition.x, transform.localPosition.z), Calc.HALF_PI, 2),
//    0.5F
//    ),
//null,
//false,
//new ShotParam(
//    m_ShotParamOperation.BulletIndex.GetResultInt(),
//    m_ShotParamOperation.Position.GetResultVector2() + localPositionVector2,
//    m_ShotParamOperation.Angle.GetResultFloat(),
//    m_ShotParamOperation.Scale.GetResultFloat(),
//    m_ShotParamOperation.Velocity.GetResultVector2(),
//    m_ShotParamOperation.AngleSpeed.GetResultFloat(),
//    m_ShotParamOperation.ScaleSpeed != null ? m_ShotParamOperation.ScaleSpeed.GetResultFloat() : 0,
//    m_ShotParamOperation.Opacity != null ? m_ShotParamOperation.Opacity.GetResultFloat() : 1,
//    m_ShotParamOperation.CanCollide != null ? m_ShotParamOperation.CanCollide.GetResultBool() : true
//    ),


//var shotParam = new CommandBulletShotParam(this);

//shotParam.Position = m_ShotPositions[i].position - transform.parent.position;
//Vector2 basePosition = m_ShotPositions[i].position;
//Debug.Log(i.ToString() + " : " + basePosition.x.ToString() + ", " + basePosition.y.ToString() +
//    "\nparent : " + transform.parent.position.x.ToString() + ", " + transform.parent.position.y.ToString() + ", " + transform.parent.position.z.ToString());
//Vector2 localPositionVector2 = new Vector2(transform.localPosition.x, transform.localPosition.z);
//Debug.Log(localPositionVector2.x.ToString() + ", " + localPositionVector2.y.ToString());


//[SerializeField, Tooltip("発射パラメータ（演算）")]
//private ShotParamOperation m_ShotParamOperation;

//[SerializeField, Tooltip("弾が持つパラメータ（演算）")]
//private BulletParamFreeOperation m_BulletParamFreeOperation;

//private float m_ShotTimeCount;


//m_ShotTimeCount = 0;


//m_ShotTimeCount -= Time.deltaTime;


//if (m_ShotTimeCount > 0)
//{
//    return;
//}

//if (m_ShotPositions == null)
//{
//    return;
//}

//AudioManager.Instance.Play(E_COMMON_SOUND.PLAYER_HACKING_SHOT);

//for(int j = 0;j < 1; j++)
//{
//    for (int i = 0; i < m_ShotPositions.Length; i++)
//    {
//        // 自機本体の位置を外部に知らせる
//        m_ShotPositionVariable.Value = new Vector2(0, transform.localPosition.z) + new Vector2(m_ShotPositions[i].position.x, 0);

//        // 弾を発射する
//        BattleHackingFreeTrajectoryBulletController.ShotBullet(
//            this,
//            null,
//            null,
//            0,
//            m_ShotParamOperation,
//            null,
//            null,
//            null,
//            null,
//            null,
//            null,
//            null
//            );
//    }
//}

//m_ShotTimeCount += m_ShotInterval;


//[SerializeField, Tooltip("ゲーム全体で共通の変数へのリンク")]
//private CommonOperationVariable m_CommonOperationVariable;
