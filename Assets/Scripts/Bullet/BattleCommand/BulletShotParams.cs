#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の発射と軌道をまとめて表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/bulletShotParams", fileName = "BulletShotParams", order = 0)]
[System.Serializable]
public class BulletShotParams : BulletShotParamBase
{

    [SerializeField, Tooltip("攻撃が始まる前の初期化処理")]
    private ForSetupBase m_ForSetup;

    [SerializeField, Tooltip("弾を撃つ時の多重forループ")]
    private MultiForLoop m_MultiForLoop;

    [SerializeField, Tooltip("弾を撃つ時の効果音")]
    private E_COMMON_SOUND m_ShotSE;

    [SerializeField, Tooltip("弾の変更可能なパラメータの初期値（演算）")]
    private BulletParamFreeOperation m_BulletParamFreeOperationChangeableInit;

    [SerializeField, Tooltip("弾の変更可能なパラメータの更新した値（演算）")]
    private BulletParamFreeOperation m_BulletParamFreeOperationChangeableUpdate;

    [SerializeField, Tooltip("弾の物理的な状態を決める時の多重forループ")]
    private ForOnceBase m_MultiForLoopForTransform;

    [SerializeField, Tooltip("弾の物理的な状態")]
    private TransformOperation m_BulletTransform;

    [SerializeField, Tooltip("弾が弾を発射するためのオブジェクト")]
    private BulletShotParams m_BulletShotParams;


    public override void OnStarts()
    {
        if (m_ForSetup != null)
        {
            m_ForSetup.Setup();
        }

        m_MultiForLoop.Setup();
    }


    public override void OnUpdates(CommandCharaController owner)
    {

        //// 一度でも弾を発射したかどうか
        //bool isShoot = false;

        for (m_MultiForLoop.Init(); m_MultiForLoop.IsTrue(); m_MultiForLoop.Process())
        {
            // 弾を撃つ
            BattleHackingFreeTrajectoryBulletController.ShotBullet(
                owner,
                m_MultiForLoopForTransform,
                m_BulletParamFreeOperationChangeableInit,
                m_BulletParamFreeOperationChangeableUpdate,
                m_BulletTransform,
                m_BulletShotParams,
                m_ShotSE
                );

            //isShoot = true;
        }

        //if (isShoot)
        //{
        //    AudioManager.Instance.Play(shotSE);
        //}
    }
}





//if (m_MultiForLoop.Init())
//{
//    do
//    {

//    }
//    while (m_MultiForLoop.Process());

//    // このフレーム内で1つでも弾が発射されたら、このフレーム内で1回だけ発射音を鳴らす。
//    //AudioManager.Instance.Play(BattleHackingEnemyManager.Instance.ParamSet.MediumShot02Se);
//    AudioManager.Instance.Play(E_COMMON_SOUND.ENEMY_SHOT_MEDIUM_02);
//}


//CommonOperationVariable commonOperationVariable,


//commonOperationVariable.DTime.GetResultFloat(),
//BulletDTime.DTime,
//null,
//m_BulletParamFreeOperation,

//commonOperationVariable.BulletTimeProperty,
//commonOperationVariable.LaunchParam,


//[SerializeField, Tooltip("弾が持つパラメータ（演算）")]
//private BulletParamFreeOperation m_BulletParamFreeOperation;
