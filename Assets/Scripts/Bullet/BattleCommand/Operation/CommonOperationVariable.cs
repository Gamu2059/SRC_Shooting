using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体を通して共通で使う変数へのリンクをまとめたクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/operation/common", fileName = "CommonOperationVariable", order = 0)]
[System.Serializable]
public class CommonOperationVariable : ScriptableObject
{

    [SerializeField, Tooltip("発射から現在までの時刻を表す変数")]
    public OperationFloatVariable m_DTimeOperation;

    [SerializeField, Tooltip("発射からの時刻を表す変数")]
    public OperationFloatVariable m_TimeOperation;

    [SerializeField, Tooltip("発射時のパラメータを表す変数")]
    public ShotParamOperationVariable m_LaunchParam;

    [SerializeField, Tooltip("前のフレームでの時刻")]
    public OperationFloatVariable m_PreviousTime;

    [SerializeField, Tooltip("ハッキングの開始からの時刻を表す変数（どの演算からも参照されないなら、float型で良さそう（？））")]
    public OperationFloatVariable m_Time;

    [SerializeField, Tooltip("発射時刻（引数の代わり）（必要なのは移行期間だけだと思う）")]
    public OperationFloatVariable m_ArgumentTime;

    [SerializeField, Tooltip("自機の位置")]
    public OperationVector2Variable m_PlayerPosition;


    public void OnStarts()
    {
        m_PreviousTime.Value = 0;
        m_Time.Value = 0;
    }


    public void OnUpdates()
    {
        m_PreviousTime.Value = m_Time.Value;
        m_Time.Value += Time.deltaTime;

        Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
        Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);
        m_PlayerPosition.Value = playerPositionVec2;

        m_ArgumentTime.Value = m_Time.Value;
    }
}
