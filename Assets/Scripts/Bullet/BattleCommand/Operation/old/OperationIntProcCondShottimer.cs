//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// int型の条件付き操作のある発射タイミングの演算を表すクラス。
///// </summary>
//[CreateAssetMenu(menuName = "Param/Danmaku/operation/int/process/conditional/shottimer", fileName = "OperationIntProcCondShottimer", order = 0)]
//[System.Serializable]
//public class OperationIntProcCondShottimer : OperationIntProcCondBase
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
//        if (m_RealShotNumVariable.Value < m_ProperShotNum)
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


//    public override float GetResultFloat()
//    {
//        return 0;
//    }


//    public override int GetResultInt()
//    {
//        return 0;
//    }


//    public override void Init()
//    {
//        // 前フレームのあるべき発射回数
//        m_LaunchTimeVariable.Value = m_PreviousTimeOperation.GetResultFloat();
//        m_RealShotNumVariable.Value = m_ProperShotNumOperation.GetResultInt();

//        // 現在のあるべき発射回数
//        m_LaunchTimeVariable.Value = m_TimeOperation.GetResultFloat();
//        m_ProperShotNum = m_ProperShotNumOperation.GetResultInt();
//    }


//    public override void Process()
//    {

//    }


//    public override bool IsTrue()
//    {
//        if (m_RealShotNumVariable.Value < m_ProperShotNum)
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





////// 増えていくなら
////if (m_Inc > 0)
////{
////    return m_Value < m_Init + m_Inc * m_TimeNum;
////}
////// 減っていくなら
////else
////{
////    return m_Value > m_Init + m_Inc * m_TimeNum;
////}