#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koimazeInheri : DanmakusAbstract
{

    // PlayerCharaManager.Instance.GetCurrentController().transform.position;

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // 角速度
    [SerializeField]
    private float angleSpeed;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected float ShotNum()
    {
        return Time.time / shotInterval;
    }


    // 発射時刻を計算する
    protected float LaunchTime(float realShotNum)
    {
        return shotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected void ShotBullets(float launchTime, float dTime)
    {

        float pastRad = angleSpeed * launchTime;
        pastRad = Modulo2PI(pastRad);

        float distance = bulletSpeed * dTime;
    }


    protected void Awake()
    {
        CalcRealShotNum[] calcRealShotNum = { ShotNum };
        CalcLaunchTime[] calcLaunchTime = { LaunchTime };
        ShotBullets[] shotBullets = { ShotBullets };

        base.Awake(calcRealShotNum, calcLaunchTime, shotBullets);
    }
}