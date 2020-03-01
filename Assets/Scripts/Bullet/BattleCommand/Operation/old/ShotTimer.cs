//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 発射タイミングを表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/ShotTimer", fileName = "ShotTimer", order = 0)]
//[System.Serializable]
//public class ShotTimer : ScriptableObject
//{

//    //[SerializeField, Tooltip("現在のあるべき発射回数")]
//    private int m_ProperShotNum;

//    [SerializeField, Tooltip("前フレームの時刻を表す変数")]
//    private OperationFloatBase m_PreviousTimeOperation;

//    [SerializeField, Tooltip("時刻を表す変数")]
//    private OperationFloatBase m_TimeOperation;

//    [SerializeField, Tooltip("発射時刻など引数を表す変数")]
//    private OperationFloatVariable m_LaunchTimeVariable;

//    [SerializeField, Tooltip("発射されてからの経過時間を表す変数")]
//    private OperationFloatVariable m_DTimeVariable;

//    [SerializeField, Tooltip("実際の発射回数を表す変数")]
//    private OperationIntVariable m_RealShotNumVariable;

//    [SerializeField, Tooltip("発射時刻を表す演算")]
//    private OperationFloatBase m_LaunchTimeOperation;

//    [SerializeField, Tooltip("理想的な発射回数を表す演算")]
//    private OperationIntBase m_ProperShotNumOperation;


//    public void OnStarts()
//    {

//    }


//    public void OnUpdates()
//    {
//        // 前フレームのあるべき発射回数
//        m_LaunchTimeVariable.Value = m_PreviousTimeOperation.GetResultFloat();
//        m_RealShotNumVariable.Value = m_ProperShotNumOperation.GetResultInt();

//        // 現在のあるべき発射回数
//        m_LaunchTimeVariable.Value = m_TimeOperation.GetResultFloat();
//        m_ProperShotNum = m_ProperShotNumOperation.GetResultInt();
//    }


//    public bool HasNextAndNext()
//    {
//        if(m_RealShotNumVariable.Value < m_ProperShotNum)
//        {
//            // 発射する弾の番号にする
//            m_RealShotNumVariable.Value++;

//            // 発射時刻
//            m_LaunchTimeVariable.Value = m_LaunchTimeOperation.GetResultFloat();

//            // 発射からの経過時間
//            m_DTimeVariable.Value = m_TimeOperation.GetResultFloat() - m_LaunchTimeVariable.Value;

//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//}




////List<(float realShotNum, float launchTime, float dTime)> launchDTimeList = new List<(float realShotNum, float launchTime, float dTime)>();


////// 発射されるべき回数分、弾を発射する
////while (m_RealShotNum < properShotNum)
////{
////    // 発射する弾の番号にする
////    m_RealShotNum++;

////    // 発射時刻
////    float launchTime = m_RealShotNum * m_ShotInterval;

////    // 発射からの経過時間
////    float dTime = m_Time - launchTime;

////    launchDTimeList.Add((m_RealShotNum, launchTime, dTime));
////}

////return launchDTimeList;


////public bool HasNext()
////{
////    return m_IsMainPhase && m_RealShotNum < m_ProperShotNum;
////}


////public void Next()
////{
////    // 発射する弾の番号にする
////    m_RealShotNum++;

////    // 発射時刻
////    m_LaunchTime = m_RealShotNum * m_ShotInterval;

////    // 発射からの経過時間
////    m_DTime = m_Time - m_LaunchTime;
////}


////[SerializeField, Tooltip("ローカル時刻を表す変数")]
////private OperationFloatVariable m_PhaseTimeVariable;


////m_IsMainPhase = false;


////// まだ何もしない時間なら
////if (!m_IsMainPhase)
////{
////    // この最初の形態を抜けていたら
////    if (m_InitialTime <= m_Time)
////    {
////        m_IsMainPhase = true;

////        m_Time -= m_InitialTime;
////    }
////    // 今もまだ最初の形態なら
////    else
////    {
////        return;
////    }
////}


////if(m_IsMainPhase && m_RealShotNum < m_ProperShotNum)

////if(m_RealShotNum < m_ProperShotNum)


////return m_InitialTime + m_LaunchTime;


//////[SerializeField, Tooltip("メインの形態であるかどうか")]
////private bool m_IsMainPhase;

////[SerializeField, Tooltip("最初のなにもしない時間の長さ")]
////private float m_InitialTime;

//////[SerializeField, Tooltip("現在の形態の開始からの時刻")]
////private float m_Time;

//////[SerializeField, Tooltip("実際に発射された回数")]
////private int m_RealShotNum;

//////[SerializeField, Tooltip("発射時刻")]
////private float m_LaunchTime;

//////[SerializeField, Tooltip("発射されてからの経過時間")]
////private float m_DTime;


////m_Time = 0;


//////時間を進める
////m_Time += Time.deltaTime;


////if(m_RealShotNumVariable.Value < m_NowProperShotNum)

////m_RealShotNumVariable.Value++;

////m_LaunchTimeVariable.Value = m_RealShotNumVariable.Value * m_ShotInterval;


////[SerializeField, Tooltip("実際に発射された回数を表す変数")]
////private OperationIntVariable m_RealShotNumVariable;


////m_RealShotNumVariable.Value = -1;


////m_PreviousProperShotNum = Mathf.FloorToInt(m_PreviousTimeVariable.GetResultFloat() / m_ShotInterval);

////m_NowProperShotNum = Mathf.FloorToInt(m_TimeVariable.GetResultFloat() / m_ShotInterval);


////m_LaunchTimeVariable.Value = m_RealShotNum * m_ShotInterval;


////[SerializeField, Tooltip("前フレームもしくは現在の時刻を表す、引数代わりの、演算")]
////private OperationFloatVariable m_ArgumentTime;


////m_RealShotNum = GetProperShotNum(m_PreviousTimeOperation.GetResultFloat());

////m_ProperShotNum = GetProperShotNum(m_TimeOperation.GetResultFloat());


////m_LaunchTimeVariable.Value = GetLaunchTime(m_RealShotNum);


////[SerializeField, Tooltip("発射間隔")]
////private float m_ShotInterval;


/////// <summary>
/////// 時刻から理想の発射回数を返す。（数列でいえば、この値になるのは何項目の直後か、というのを求めている）
/////// </summary>
////public int GetProperShotNum(float time)
////{
////    return Mathf.FloorToInt(time / m_ShotInterval);
////}


/////// <summary>
/////// 発射回数から発射時刻を返す。（数列でいえば一般項を求めるメソッド）
/////// </summary>
////public float GetLaunchTime(int shotNum)
////{
////    return shotNum * m_ShotInterval;
////}


//////[SerializeField, Tooltip("前フレームのあるべき発射回数（つまり実際の発射回数）")]
////private int m_RealShotNum;


////m_RealShotNum = m_ProperShotNumOperation.GetResultInt();


////// 発射する弾の番号にする
////m_RealShotNum++;

////// 変数にも代入する
////m_RealShotNumVariable.Value = m_RealShotNum;


/////// <summary>
/////// 発射時刻を返す。後々多態性を使う。
/////// </summary>
/////// <returns></returns>
////public float GetLaunchTime()
////{
////    return m_LaunchTimeVariable.Value;
////}


/////// <summary>
/////// 発射されてからの経過時間を返す。
/////// </summary>
/////// <returns></returns>
////public float GetDTime()
////{
////    return m_DTimeVariable.Value;
////}


/////// <summary>
/////// 実際に発射された回数を返す。
/////// </summary>
/////// <returns></returns>
////public int GetRealShotNum()
////{
////    return m_RealShotNumVariable.Value;
////}
