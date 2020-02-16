using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるハッキングのボスのある形態の状態を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/HackingBossPhaseState", fileName = "HackingBossPhaseState", order = 0)]
[System.Serializable]
public class HackingBossPhaseState : ScriptableObject
{

    [SerializeField, Tooltip("前のフレームでの時刻")]
    private OperationFloatVariable m_PreviousTime;

    [SerializeField, Tooltip("ハッキングの開始からの時刻を表す変数（どの演算からも参照されないなら、float型で良さそう（？））")]
    private OperationFloatVariable m_Time;

    [SerializeField, Tooltip("物理的な状態")]
    private TransformOperation m_Transform;

    [SerializeField, Tooltip("発射時刻（引数の代わり）（必要なのは移行期間だけだと思う）")]
    public OperationFloatVariable m_ArgumentTime;

    [SerializeField, Tooltip("自機の位置")]
    private OperationVector2Variable m_PlayerPosition;



    public void OnStarts()
    {
        m_PreviousTime.Value = 0;
        m_Time.Value = 0;
    }


    public TransformSimple OnUpdates()
    {
        m_PreviousTime.Value = m_Time.Value;
        m_Time.Value += Time.deltaTime;

        Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
        Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);
        m_PlayerPosition.SetValueVector2(playerPositionVec2);

        m_ArgumentTime.Value = m_Time.Value;

        return m_Transform.GetResultValues();
    }
}






//    // もし形態が最後までいっていたら、その最後の状態でいることにする
//    if (m_Phase >= m_BezierMovingFlow.Length)
//    {
//        return m_BezierMovingFlow[m_BezierMovingFlow.Length - 1].GetEndTransform();
//    }

//    // もし既にこの形態が終わっているなら
//    if (m_BezierMovingFlow[m_Phase].GetDuration() < m_PhaseTime)
//    {
//        // 経過時間を正しくする
//        m_PhaseTime -= m_BezierMovingFlow[m_Phase].GetDuration();

//        // 形態を次のものにする
//        m_Phase++;

//        // もし形態が最後までいっていたら、その最後の状態でいることにする
//        if (m_Phase >= m_BezierMovingFlow.Length)
//        {
//            return m_BezierMovingFlow[m_BezierMovingFlow.Length - 1].GetEndTransform();
//        }

//        // 次の形態の最初にやる処理をやる。
//        m_BezierMovingFlow[m_Phase].Init(m_BezierMovingFlow[m_Phase - 1].GetEndTransform());
//    }

//    return m_BezierMovingFlow[m_Phase].GetTransform(m_PhaseTime);



//for (int i = 0; i < m_DurationArray.Length; i++)
//{
//    if (m_Time.Value <= m_DurationArray[i].GetResultFloat())
//    {
//        for (int j = 0; j < i; j++)
//        {
//            //m_ArgumentTime.SetValueFloat(m_ArgumentTime.GetResultFloat() - m_DurationArray[j].GetResultFloat());
//            m_ArgumentTime.Value -= m_DurationArray[j].GetResultFloat();
//        }

//        return new TransformSimple(m_PositionArray[i].GetResultVector2(), 0, 1);
//    }
//}

//for (int j = 0; j < m_DurationArray.Length; j++)
//{
//    //m_ArgumentTime.SetValueFloat(m_ArgumentTime.GetResultFloat() - m_DurationArray[j].GetResultFloat());
//    m_ArgumentTime.Value -= m_DurationArray[j].GetResultFloat();
//}

//return new TransformSimple(m_PositionArray[m_DurationArray.Length].GetResultVector2(), 0, 1);


//[SerializeField, Tooltip("初期状態")]
//private TransformSimple m_InitialTransform;

//[SerializeField, Tooltip("動き方を表すオブジェクトの配列（最初の部分）")]
//private MovingPhaseBase[] m_BezierMovingFlow;

//[SerializeField, Tooltip("動き方を表すオブジェクトの配列（ループする部分）")]
//private MovingPhaseBase[] m_BezierMovingLoop;

////[SerializeField, Tooltip("現在の形態を表す整数")]
//private int m_Phase;

////[SerializeField, Tooltip("現在の形態開始からの経過時間")]
//private float m_PhaseTime;

////[SerializeField, Tooltip("現在、ループしている部分にいるかどうか")]
//private bool m_IsLoopPhase;


//// 動き方の配列が空配列でなければ、最初の要素の最初の処理をする。
//if (m_BezierMovingFlow.Length != 0)
//{
//    m_BezierMovingFlow[0].Init(m_InitialTransform);
//}

//m_Phase = 0;
//m_PhaseTime = 0;
//m_IsLoopPhase = false;


//m_PhaseTime += Time.deltaTime;

//// フロー部分にいるなら
//if (!m_IsLoopPhase)
//{
//    // もし既にこの形態が終わっているなら
//    if (m_BezierMovingFlow[m_Phase].GetDuration() < m_PhaseTime)
//    {
//        //Debug.Log("フローの形態変化");

//        // 経過時間を正しくする
//        m_PhaseTime -= m_BezierMovingFlow[m_Phase].GetDuration();

//        // 形態を次のものにする
//        m_Phase++;

//        // もし形態が最後までいっていたら、ループ部分の最初の形態に進む
//        if (m_BezierMovingFlow.Length <= m_Phase)
//        {
//            //Debug.Log("フローからループへ");

//            m_IsLoopPhase = true;
//            m_Phase = 0;

//            // 次の形態の最初にやる処理をやる。
//            m_BezierMovingLoop[0].Init(m_BezierMovingFlow[m_BezierMovingFlow.Length - 1].GetEndTransform());

//            return m_BezierMovingLoop[m_Phase].GetTransform(m_PhaseTime);
//        }

//        // 次の形態の最初にやる処理をやる。
//        m_BezierMovingFlow[m_Phase].Init(m_BezierMovingFlow[m_Phase - 1].GetEndTransform());
//    }

//    return m_BezierMovingFlow[m_Phase].GetTransform(m_PhaseTime);
//}
//// ループ部分にいるなら
//else
//{
//    // もし既にこの形態が終わっているなら
//    if (m_BezierMovingLoop[m_Phase].GetDuration() < m_PhaseTime)
//    {
//        //Debug.Log("ループの形態変化");

//        // 経過時間を正しくする
//        m_PhaseTime -= m_BezierMovingLoop[m_Phase].GetDuration();

//        // 形態を次のものにする
//        m_Phase++;

//        // もし形態が最後までいっていたら、ループ部分の最初の形態に戻る
//        if (m_BezierMovingLoop.Length <= m_Phase)
//        {
//            //Debug.Log("ループする");

//            m_Phase = 0;

//            // 次の形態の最初にやる処理をやる。
//            m_BezierMovingLoop[0].Init(m_BezierMovingLoop[m_BezierMovingLoop.Length - 1].GetEndTransform());
//        }
//        else
//        {
//            // 次の形態の最初にやる処理をやる。
//            m_BezierMovingLoop[m_Phase].Init(m_BezierMovingLoop[m_Phase - 1].GetEndTransform());
//        }
//    }

//    return m_BezierMovingLoop[m_Phase].GetTransform(m_PhaseTime);
//}


///// <summary>
///// 状態を返す。（今は、発射時の値しか計算できない。LaunchTimeの値に基づいて計算しているから。）
///// </summary>
//public TransformSimple GetTransform(float time)
//{
//    m_ArgumentTime.SetValueFloat(time);

//    return new TransformSimple(m_Position.GetResultVector2(), 0, 1);


//    //int phase = 0;
//    //float beginTime = 0;
//    //bool isLoopPhase = false;

//    //while (phase < m_BezierMovingFlow.Length && beginTime + m_BezierMovingFlow[phase].GetDuration() <= time)
//    //{
//    //    beginTime += m_BezierMovingFlow[phase].GetDuration();
//    //    phase++;

//    //    //Debug.Log("FlowWhile");
//    //}

//    //// フロー部分が最後までいっているなら
//    //if (phase == m_BezierMovingFlow.Length)
//    //{
//    //    isLoopPhase = true;
//    //    phase = 0;

//    //    //Debug.Log("LoopIf");

//    //    while (beginTime + m_BezierMovingLoop[phase].GetDuration() <= time)
//    //    {
//    //        beginTime += m_BezierMovingLoop[phase].GetDuration();
//    //        phase++;
//    //        phase %= m_BezierMovingLoop.Length;

//    //        //Debug.Log("LoopWhile");
//    //    }
//    //}

//    //// フロー部分なら
//    //if (!isLoopPhase)
//    //{
//    //    //Debug.Log("Flow: " + m_BezierMovingFlow[phase].GetTransform(time - beginTime).ToString());

//    //    return m_BezierMovingFlow[phase].GetTransform(time - beginTime);
//    //}
//    //// ループ部分なら
//    //else
//    //{
//    //    //Debug.Log("Loop: " + m_BezierMovingLoop[phase].GetTransform(time - beginTime).ToString());

//    //    return m_BezierMovingLoop[phase].GetTransform(time - beginTime);
//    //}
//}


///// <summary>
///// 状態を返す。
///// </summary>
//public TransformSimple GetDdtTransform(float time)
//{
//    int phase = 0;
//    float beginTime = 0;
//    bool isLoopPhase = false;

//    while (phase < m_BezierMovingFlow.Length && beginTime + m_BezierMovingFlow[phase].GetDuration() <= time)
//    {
//        beginTime += m_BezierMovingFlow[phase].GetDuration();
//        phase++;
//    }

//    // フロー部分が最後までいっているなら
//    if (phase == m_BezierMovingFlow.Length)
//    {
//        isLoopPhase = true;
//        phase = 0;

//        while (beginTime + m_BezierMovingLoop[phase].GetDuration() <= time)
//        {
//            beginTime += m_BezierMovingLoop[phase].GetDuration();
//            phase++;
//            phase %= m_BezierMovingLoop.Length;
//        }
//    }

//    // フロー部分なら
//    if (!isLoopPhase)
//    {
//        return m_BezierMovingFlow[phase].GetDdtTransform(time - beginTime);
//    }
//    // ループ部分なら
//    else
//    {
//        return m_BezierMovingLoop[phase].GetDdtTransform(time - beginTime);
//    }
//}


//[SerializeField, Tooltip("所要時間の配列")]
//private OperationFloatBase[] m_DurationArray;

//[SerializeField, Tooltip("位置の配列")]
//private OperationVector2Base[] m_PositionArray;


//[SerializeField, Tooltip("位置を表す変数")]
//private OperationVector2Base m_Position;

//[SerializeField, Tooltip("角度を表す変数")]
//private OperationFloatBase m_Angle;

//[SerializeField, Tooltip("大きさを表す変数")]
//private OperationFloatBase m_Scale;
