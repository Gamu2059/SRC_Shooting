using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射タイミングを表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/ShotTimer", fileName = "ShotTimer", order = 0)]
[System.Serializable]
public class ShotTimer : ScriptableObject
{
    //[SerializeField, Tooltip("メインの形態であるかどうか")]
    private bool m_IsMainPhase;

    [SerializeField, Tooltip("最初のなにもしない時間の長さ")]
    private float m_InitialTime;

    [SerializeField, Tooltip("発射間隔")]
    private float m_ShotInterval;

    //[SerializeField, Tooltip("現在の形態の開始からの時刻")]
    private float m_Time;

    //[SerializeField, Tooltip("実際に発射された回数")]
    private int m_RealShotNum;

    //[SerializeField, Tooltip("現在のあるべき発射回数")]
    private int m_ProperShotNum;

    //[SerializeField, Tooltip("発射時刻")]
    private float m_LaunchTime;

    //[SerializeField, Tooltip("発射されてからの経過時間")]
    private float m_DTime;


    public void OnStarts()
    {
        m_IsMainPhase = false;
        m_Time = 0;
        m_RealShotNum = -1;
    }


    public void OnUpdates()
    {
        //時間を進める
        m_Time += Time.deltaTime;

        // まだ何もしない時間なら
        if (!m_IsMainPhase)
        {
            // この最初の形態を抜けていたら
            if (m_InitialTime <= m_Time)
            {
                m_IsMainPhase = true;

                m_Time -= m_InitialTime;
            }
            // 今もまだ最初の形態なら
            else
            {
                return;
            }
        }

        // 現在のあるべき発射回数
        m_ProperShotNum = Mathf.FloorToInt(m_Time / m_ShotInterval);
    }


    public bool HasNext()
    {
        return m_IsMainPhase && m_RealShotNum < m_ProperShotNum;
    }


    public void Next()
    {
        // 発射する弾の番号にする
        m_RealShotNum++;

        // 発射時刻
        m_LaunchTime = m_RealShotNum * m_ShotInterval;

        // 発射からの経過時間
        m_DTime = m_Time - m_LaunchTime;
    }


    /// <summary>
    /// 発射時刻を返す。後々多態性を使う。
    /// </summary>
    /// <returns></returns>
    public float GetLaunchTime()
    {
        return m_LaunchTime;
    }


    /// <summary>
    /// 発射されてからの経過時間を返す。
    /// </summary>
    /// <returns></returns>
    public float GetDTime()
    {
        return m_DTime;
    }


    /// <summary>
    /// 実際に発射された回数を返す。
    /// </summary>
    /// <returns></returns>
    public int GetRealShotNum()
    {
        return m_RealShotNum;
    }
}




//List<(float realShotNum, float launchTime, float dTime)> launchDTimeList = new List<(float realShotNum, float launchTime, float dTime)>();


//// 発射されるべき回数分、弾を発射する
//while (m_RealShotNum < properShotNum)
//{
//    // 発射する弾の番号にする
//    m_RealShotNum++;

//    // 発射時刻
//    float launchTime = m_RealShotNum * m_ShotInterval;

//    // 発射からの経過時間
//    float dTime = m_Time - launchTime;

//    launchDTimeList.Add((m_RealShotNum, launchTime, dTime));
//}

//return launchDTimeList;