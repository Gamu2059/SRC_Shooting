using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideEternal : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float m_ShotInterval;

    // 弾源の円の半径の位相速度
    [SerializeField]
    private float circleRadiusPhaseSpeed;

    // 弾源の円の半径の最大値
    [SerializeField]
    private float circleRadiusMax;

    // 弾の速さ
    [SerializeField]
    private float m_BulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / m_ShotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return m_ShotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {

        // 発射後の移動距離
        float distance = m_BulletSpeed * dTime;

        // 弾の発射角度
        float launchRad = Random.Range(0, Mathf.PI * 2);

        // 弾源円半径の位相
        float circleRadiusPhase = circleRadiusPhaseSpeed * launchTime;

        // 今回の弾源円半径
        float circleRadius = circleRadiusMax * (1 - Mathf.Cos(circleRadiusPhase)) / 2;

        // 位置のランダムなブレの成分
        Vector2 launchPos = Random.insideUnitCircle * circleRadius;

        // 弾の位置を発射時の位置にする
        Vector3 pos = transform.position;

        // 弾の位置を発射地点にする
        pos += new Vector3(launchPos.x, 0, launchPos.y);

        // 弾を経過した時間だけ進ませる
        pos += new Vector3(distance * Mathf.Cos(launchRad), 0, distance * Mathf.Sin(launchRad));

        // 弾の角度
        Vector3 eulerAngles = CalcEulerAngles(launchRad);

        // 弾を撃つ
        BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
        BulletController.ShotBullet(bulletShotParam);
    }
}