using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるハッキングのボスのある形態の状態を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/HackingBossPhaseState", fileName = "HackingBossPhaseState", order = 0)]
[System.Serializable]
public class HackingBossPhaseState1 : ScriptableObject
{
    //[SerializeField, Tooltip("開始からの経過時間。")]
    private float m_Time;

    //[SerializeField, Tooltip("現在の形態開始からの経過時間。")]
    private float m_PhaseTime;

    [SerializeField, Tooltip("初期状態")]
    private TransformSimple m_InitialTransform;

    [SerializeField, Tooltip("動き方を表すオブジェクトの配列")]
    private BezierMoving[] m_BezierMoving;

    //[SerializeField, Tooltip("現在の形態を表す整数")]
    private int m_Phase;


    public void OnStarts()
    {
        m_Time = 0;
        m_PhaseTime = 0;

        // 動き方の配列が空配列でなければ、最初の要素の最初の処理をする。
        if (m_BezierMoving.Length != 0)
        {
            m_BezierMoving[0].Init(m_InitialTransform);
        }

        m_Phase = 0;
    }


    public TransformSimple OnUpdates()
    {
        m_Time += Time.deltaTime;
        m_PhaseTime += Time.deltaTime;

        // もし形態が最後までいっていたら、その最後の状態でいることにする
        if (m_Phase >= m_BezierMoving.Length)
        {
            return m_BezierMoving[m_BezierMoving.Length - 1].GetEndTransform();
        }

        // もし既にこの形態が終わっているなら
        if (m_BezierMoving[m_Phase].GetDuration() < m_PhaseTime)
        {
            // 経過時間を正しくする
            m_PhaseTime -= m_BezierMoving[m_Phase].GetDuration();

            // 形態を次のものにする
            m_Phase++;

            // もし形態が最後までいっていたら、その最後の状態でいることにする
            if (m_Phase >= m_BezierMoving.Length)
            {
                return m_BezierMoving[m_BezierMoving.Length - 1].GetEndTransform();
            }
        }

        return m_BezierMoving[m_Phase].GetTransform(m_PhaseTime);
    }


    /// <summary>
    /// 状態を返す。
    /// </summary>
    /// <returns></returns>
    public TransformSimple GetTransform(float time)
    {
        return m_InitialTransform;
    }
}
