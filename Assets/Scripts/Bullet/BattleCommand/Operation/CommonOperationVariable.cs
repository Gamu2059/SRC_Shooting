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
    private OperationFloatVariable m_DTimeOperation;
    public OperationFloatVariable DTimeOperation
    {
        set { m_DTimeOperation = value; }
        get { return m_DTimeOperation; }
    }

    [SerializeField, Tooltip("発射からの時刻を表す変数")]
    private OperationFloatVariable m_TimeOperation;
    public OperationFloatVariable TimeOperation
    {
        set { m_TimeOperation = value; }
        get { return m_TimeOperation; }
    }

    [SerializeField, Tooltip("発射時のパラメータを表す変数")]
    private ShotParamOperationVariable m_LaunchParam;
    public ShotParamOperationVariable LaunchParam
    {
        set { m_LaunchParam = value; }
        get { return m_LaunchParam; }
    }

    [SerializeField, Tooltip("前のフレームでの時刻")]
    private OperationFloatVariable m_PreviousTime;
    public OperationFloatVariable PreviousTime
    {
        set { m_PreviousTime = value; }
        get { return m_PreviousTime; }
    }

    [SerializeField, Tooltip("ハッキングの開始からの時刻を表す変数（どの演算からも参照されないなら、float型で良さそう（？））")]
    private OperationFloatVariable m_Time;
    public OperationFloatVariable Time
    {
        set { m_Time = value; }
        get { return m_Time; }
    }

    [SerializeField, Tooltip("発射時刻（引数の代わり）（必要なのは移行期間だけだと思う）")]
    private OperationFloatVariable m_ArgumentTime;
    public OperationFloatVariable ArgumentTime
    {
        set { m_ArgumentTime = value; }
        get { return m_ArgumentTime; }
    }

    [SerializeField, Tooltip("自機の位置")]
    private OperationVector2Variable m_PlayerPosition;
    public OperationVector2Variable PlayerPosition
    {
        set { m_PlayerPosition = value; }
        get { return m_PlayerPosition; }
    }

    [SerializeField, Tooltip("弾の見える位置のx座標の最小値")]
    public OperationFloatBase PositionXMin;

    [SerializeField, Tooltip("弾の見える位置のx座標の最大値")]
    public OperationFloatBase PositionXMax;

    [SerializeField, Tooltip("弾の見える位置のy座標の最小値")]
    public OperationFloatBase PositionYMin;

    [SerializeField, Tooltip("弾の見える位置のy座標の最大値")]
    public OperationFloatBase PositionYMax;


    public void OnStarts()
    {
        PreviousTime.Value = 0;
        Time.Value = 0;
    }


    public void OnUpdates()
    {
        PreviousTime.Value = Time.Value;
        Time.Value += UnityEngine.Time.deltaTime;

        Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
        Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);
        PlayerPosition.Value = playerPositionVec2;

        ArgumentTime.Value = Time.Value;
    }
}
