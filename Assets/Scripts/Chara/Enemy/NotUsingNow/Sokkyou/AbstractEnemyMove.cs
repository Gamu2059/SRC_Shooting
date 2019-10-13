using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class AbstractEnemyMove : BattleRealEnemyController
{
    // その形態になってからの経過時間// 起動してからの経過時間(時刻)
    protected float m_Time;

    // 現在の形態
    protected int m_NowPhase;

    // 形態の総数
    private int m_NumPhase;

    // それぞれの形態の時間の長さ// それぞれの形態の終了時刻の配列(次の形態の開始時刻)(形態変化時刻)
    [SerializeField, Tooltip("それぞれの形態の時間の長さ")]
    protected float[] m_PhaseTime;

    // 敵本体の初期位置
    protected Vector3 m_InitialPosition;


    // 第何形態で弾幕を出すか
    [SerializeField, Tooltip("何形態目で弾幕を出すか")]
    protected int m_DanmakuPhase;

    // 現在弾幕を出しているか
    private bool m_IsDanmakuPhase;

    // 実際の今までの発射回数
    protected int[] m_RealShotNum;

    // デリゲートを定義する
    protected delegate float ProperShotNumDelegate();
    protected delegate float LaunchTimeDelegate(float realShotNum);
    protected delegate void ShotBulletsDelegate(float launchTime, float dTime);

    // デリゲートの配列
    protected ProperShotNumDelegate[] m_ProperShotNumDelegate;
    protected LaunchTimeDelegate[] m_LaunchTimeDelegate;
    protected ShotBulletsDelegate[] m_ShotBulletsDelegate;


    // 理想の発射回数を返すデリゲートを返す関数
    protected abstract ProperShotNumDelegate[] GetProperShotNumDelegate();

    // 発射時刻を返すデリゲートを返す関数
    protected abstract LaunchTimeDelegate[] GetLaunchTimeDelegate();

    // 弾を発射するデリゲートを返す関数
    protected abstract ShotBulletsDelegate[] GetShotBulletsDelegate();


    protected abstract void BezierPositionMoving();


    protected override void OnAwake()
    {
        base.OnAwake();

        // 時刻をゼロにする
        m_Time = 0;

        // 現在の状態をゼロにする
        m_NowPhase = 0;

        // 弾幕を出しているか
        m_IsDanmakuPhase = m_NowPhase == m_DanmakuPhase;

        // 形態の数
        m_NumPhase = m_PhaseTime.Length;

        // 初期位置を代入する
        m_InitialPosition = transform.position;

        // 理想の発射回数を返すデリゲートに代入する
        m_ProperShotNumDelegate = GetProperShotNumDelegate();

        // 発射時刻を返すデリゲートに代入する
        m_LaunchTimeDelegate = GetLaunchTimeDelegate();

        // 弾を発射するデリゲートに代入する
        m_ShotBulletsDelegate = GetShotBulletsDelegate();

        // 実際の発射回数を全て0にする
        m_RealShotNum = new int[m_ShotBulletsDelegate.Length];
        for(int i = 0;i < m_ShotBulletsDelegate.Length; i++)
        {
            m_RealShotNum[i] = 0;
        }
    }


    public override void OnUpdate()
    {
        // 経過時間を進める
        m_Time += Time.deltaTime;

        // 次の形態に移行しているかどうか
        if(m_PhaseTime[m_NowPhase] < m_Time)
        {

            // 経過時間を正しくする
            m_Time -= m_PhaseTime[m_NowPhase];

            // 形態を次のものにする
            m_NowPhase++;

            // 全て終了しているか
            if (m_NowPhase == m_NumPhase)
            {
                Destroy();
            }

            // 弾幕を出しているか
            m_IsDanmakuPhase = m_NowPhase == m_DanmakuPhase;
        }

        // 敵の状態を更新する
        BezierPositionMoving();

        // 弾幕を出す
        if (m_IsDanmakuPhase)
        {
            DanmakuUpdate();
        }
    }


    // 弾幕の毎フレームの処理
    private void DanmakuUpdate()
    {
        // それぞれの弾幕について処理をする
        for (int i = 0; i < m_ShotBulletsDelegate.Length; i++)
        {
            // 現在のあるべき発射回数
            int properShotNum = Mathf.FloorToInt(m_ProperShotNumDelegate[i]());

            // 発射されるべき回数分、弾を発射する
            while (m_RealShotNum[i] < properShotNum)
            {
                // 発射する弾の番号にする
                m_RealShotNum[i]++;

                // 発射時刻
                float launchTime = m_LaunchTimeDelegate[i](m_RealShotNum[i]);

                // 発射からの経過時間
                float dTime = m_Time - launchTime;

                // 弾を撃つ
                m_ShotBulletsDelegate[i](launchTime, dTime);
            }
        }
    }



    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
    }



    // 角度からオイラー角を計算する
    protected Vector3 CalcEulerAngles(float rad)
    {
        Vector3 angle = transform.eulerAngles;
        angle.y = -(rad * Mathf.Rad2Deg) + 90;
        return angle;
    }


    // 2πで割った余りにする
    protected float Modulo2PI(float rad)
    {
        rad %= Mathf.PI * 2;
        return rad;
    }


    // オイラー角から角度を計算する
    protected float CalcRad()
    {
        Vector3 angle = transform.eulerAngles;
        return (90 - angle.y) * Mathf.Deg2Rad;
    }




    // 敵の状態を決めるデリゲートを定義する
    //[SerializeField]
    //protected delegate void MoveEnemyDelegate();

    // 敵の状態を決めるデリゲートの配列
    //[SerializeField]
    //protected MoveEnemyDelegate[] m_MoveEnemyDelegate;


    // 敵の状態を決めるデリゲートを返す関数
    //[SerializeField]
    //protected abstract MoveEnemyDelegate[] GetMoveEnemyDelegate();

    // それぞれの形態の時間の長さを返す関数
    //[SerializeField]
    //protected abstract float[] GetPhaseTime();


    // デリゲートに関数を代入する
    //m_MoveEnemyDelegate = GetMoveEnemyDelegate();

    // 形態時間配列を代入する
    //m_PhaseTime = GetPhaseTime();

    // 形態の数
    //m_NumPhase = m_MoveEnemyDelegate.Length;


    //m_MoveEnemyDelegate[m_NowPhase]();
}