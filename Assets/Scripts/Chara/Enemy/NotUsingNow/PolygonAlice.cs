#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonAlice : DanmakuAbstract
{

    // 発射間隔
    [SerializeField]
    private float shotInterval;

    // way数
    [SerializeField]
    private int way;

    // 発射地点の円の半径
    [SerializeField]
    private float circleRadius;

    // 一辺を通過するのにかかる時間
    [SerializeField]
    private float vertexPeriod;

    // 頂点の数
    [SerializeField]
    private int vertexNum;

    // 弾の速さ
    [SerializeField]
    private float bulletSpeed = 10;


    // 本体の位置とオイラー角を更新する
    protected override void UpdateSelf()
    {

    }


    // 現在のあるべき発射回数を計算する(小数)
    protected override float CalcNowShotNum()
    {
        return Time.time / shotInterval;
    }


    // 発射時刻を計算する
    protected override float CalcLaunchTime()
    {
        return shotInterval * realShotNum;
    }


    // 弾の位置とオイラー角を計算して発射する[発射時刻、発射からの経過時間]
    protected override void ShotBullets(float launchTime, float dTime)
    {


        // 現在はどこの辺か
        int nowSide = Mathf.FloorToInt(launchTime / vertexPeriod) % vertexNum;

        // 現在は辺の中のどの位置か
        float nowSidePhase = launchTime % vertexPeriod;

        // 敵本体の位置から見た発射位置
        Vector3 relaiveLaunchPos = Vector3.Lerp(LaunchPos(nowSide), LaunchPos(nowSide + 1), nowSidePhase);

        // 発射後の移動距離
        float distance = bulletSpeed * dTime;

        // 弾源が図形を一周する時間
        float cyclePeriod = vertexPeriod * vertexNum;

        for (int i = 0; i < way; i++)
        {
            // 進行方向の角度
            //float movingRad = Mathf.PI * 2 * nowSide / vertexNum + Mathf.PI * 2 / vertexNum / 2 + Mathf.PI / 2;
            float movingRad = launchTime % cyclePeriod / cyclePeriod * Mathf.PI * 2;

            // way数による角度のズレ
            float wayRad = movingRad + Mathf.PI * 2 * i / way;

            // 発射された弾の位置
            Vector3 pos = transform.position;
            pos += relaiveLaunchPos;
            pos += new Vector3(distance * Mathf.Cos(wayRad), 0, distance * Mathf.Sin(wayRad));

            Vector3 eulerAngles;

            eulerAngles = CalcEulerAngles(wayRad);

            // 弾を撃つ
            BulletShotParam bulletShotParam = new BulletShotParam(this, 0, 0, 0, pos, eulerAngles, transform.localScale);
            BulletController.ShotBullet(bulletShotParam);
        }
    }


    // 頂点の番号からその頂点の位置を求める(中心は原点)
    private Vector3 LaunchPos(int vertexIndex)
    {
        // 中心からの角度
        float rad = Mathf.PI * 2 * vertexIndex / vertexNum;

        return circleRadius * new Vector3(Mathf.Cos(rad),0, Mathf.Sin(rad));
    }


    // Vector3でのLerp関数
    private Vector3 LerpVector3(Vector3 vec1, Vector3 vec2,float t)
    {
        return new Vector3(Mathf.Lerp(vec1.x, vec2.x,t),Mathf.Lerp(vec1.y, vec2.y, t),Mathf.Lerp(vec1.z, vec2.z, t));
    }
}