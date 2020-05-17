#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射タイミングを表すfor文を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/for/shottimer", fileName = "ForShottimer", order = 0)]
[System.Serializable]
public class ForShottimer : ForBase
{

    [SerializeField, Tooltip("実際の発射回数を表す変数")]
    private OperationIntVariable m_RealShotNumVariable;

    [SerializeField, Tooltip("発射時刻を表す演算")]
    private OperationFloatBase m_LaunchTimeOperation;

    protected float m_NowTime;


    public override void Setup()
    {
        // 発射回数を初期化する（フィールド宣言と同時に行えば、ここに書く必要はなさそう？いやでも、それだと同じハッキングに再挑戦した時に困るか。）
        m_RealShotNumVariable.Value = -1;
    }


    public override void Init()
    {
        m_NowTime = BulletTime.Time;
    }


    public override void Process()
    {

    }


    public override bool IsTrue()
    {
        // 試しに発射回数を1増やす
        m_RealShotNumVariable.Value++;

        // この発射回数での発射時刻
        float launchTime = m_LaunchTimeOperation.GetResultFloat();

        // 現在、既に次の発射がされているかどうか
        if (launchTime <= m_NowTime)
        {
            // 発射時刻
            BulletTime.Time = launchTime;

            // 発射からの経過時間
            BulletDTime.DTime = m_NowTime - launchTime;

            return true;
        }
        else
        {
            // 次の発射はまだなので、発射回数を現在のものに戻す
            m_RealShotNumVariable.Value--;

            return false;
        }
    }
}




//m_CommonOperationVariable.m_ArgumentTime.Value = m_CommonOperationVariable.m_PreviousTime.GetResultFloat();
//m_RealShotNumVariable.Value = m_ProperShotNumOperation.GetResultInt();


//[SerializeField, Tooltip("前フレームの時刻を表す変数")]
//private OperationFloatBase m_PreviousTimeOperation;

//[SerializeField, Tooltip("時刻を表す変数")]
//private OperationFloatBase m_TimeOperation;

//[SerializeField, Tooltip("発射時刻など引数を表す変数")]
//private OperationFloatVariable m_LaunchTimeVariable;

//[SerializeField, Tooltip("発射されてからの経過時間を表す変数")]
//private OperationFloatVariable m_DTimeVariable;


//if (m_RealShotNumVariable.Value < m_ProperShotNum)

//// 発射する弾の番号にする
//m_RealShotNumVariable.Value++;


//// 前フレームのあるべき発射回数
//m_RealShotNumVariable.Value = m_ProperShotNum;

//// 現在のあるべき発射回数
//m_CommonOperationVariable.m_ArgumentTime.Value = m_CommonOperationVariable.m_Time.GetResultFloat();
//m_ProperShotNum = m_ProperShotNumOperation.GetResultInt();


////[SerializeField, Tooltip("現在のあるべき発射回数")]
//private int m_ProperShotNum;

//[SerializeField, Tooltip("理想的な発射回数を表す演算")]
//private OperationIntBase m_ProperShotNumOperation;


//[SerializeField, Tooltip("このゲームで共通の変数群")]
//private CommonOperationVariable m_CommonOperationVariable;


//m_CommonOperationVariable.LaunchTime.Value = m_LaunchTimeOperation.GetResultFloat();

//m_CommonOperationVariable.BulletTimeProperty.Value = m_LaunchTimeOperation.GetResultFloat();


//m_CommonOperationVariable.DTime.Value = m_NowTime - m_CommonOperationVariable.LaunchTime.Value;
