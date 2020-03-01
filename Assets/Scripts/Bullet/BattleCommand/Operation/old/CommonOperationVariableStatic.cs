//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// ゲーム全体を通して共通で使う変数へのリンクをまとめた静的クラス。
///// </summary>
//[System.Serializable]
//public static class CommonOperationVariableStatic : object
//{

//    [SerializeField, Tooltip("発射から現在までの時刻を表す変数")]
//    private static OperationFloatBase m_DTimeOperation;

//    [SerializeField, Tooltip("発射からの時刻を表す変数")]
//    public static OperationFloatVariable m_TimeOperation;

//    [SerializeField, Tooltip("発射時のパラメータを表す変数")]
//    public static ShotParamOperationVariable m_LaunchParam;

//    [SerializeField, Tooltip("前のフレームでの時刻")]
//    public static OperationFloatVariable m_PreviousTime;

//    [SerializeField, Tooltip("ハッキングの開始からの時刻を表す変数（どの演算からも参照されないなら、float型で良さそう（？））")]
//    public static OperationFloatVariable m_Time;

//    [SerializeField, Tooltip("発射時刻（引数の代わり）（必要なのは移行期間だけだと思う）")]
//    public static OperationFloatVariable m_ArgumentTime;

//    [SerializeField, Tooltip("自機の位置")]
//    public static OperationVector2Variable m_PlayerPosition;


//    //public static void OnStarts()
//    //{
//    //    m_PreviousTime.Value = 0;
//    //    m_Time.Value = 0;
//    //}


//    //public static void OnUpdates()
//    //{
//    //    m_PreviousTime.Value = m_Time.Value;
//    //    m_Time.Value += Time.deltaTime;

//    //    Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
//    //    Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);
//    //    m_PlayerPosition.Value = playerPositionVec2;

//    //    m_ArgumentTime.Value = m_Time.Value;
//    //}
//}
