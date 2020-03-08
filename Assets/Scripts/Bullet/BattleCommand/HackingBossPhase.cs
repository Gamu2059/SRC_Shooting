using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// あるハッキングのボスのある形態を表すクラス。
/// </summary>
[CreateAssetMenu(menuName = "Param/Danmaku/HackingBossPhase", fileName = "HackingBossPhase", order = 0)]
[System.Serializable]
public class HackingBossPhase : ScriptableObject
{

    [SerializeField, Tooltip("敵本体の物理的な状態")]
    private TransformOperation m_BossTransform;

    [SerializeField, Tooltip("弾幕")]
    private Danmaku m_DanmakuArray;

    [SerializeField, Tooltip("ゲーム全体で共通の変数へのリンク")]
    private CommonOperationVariable m_CommonOperationVariable;


    public void OnStarts()
    {
        m_CommonOperationVariable.OnStarts();

        m_DanmakuArray.OnStarts();

        BattleHackingFreeTrajectoryBulletController.CommonOperationVar = m_CommonOperationVariable;
    }


    public TransformSimple OnUpdates(BattleHackingBossBehavior boss)
    {
        m_CommonOperationVariable.OnUpdates();

        TransformSimple transform = m_BossTransform.GetResultTransform();

        m_DanmakuArray.OnUpdates(
            boss,
            m_CommonOperationVariable
            );

        return transform;
    }
}





////[SerializeField, Tooltip("開始からの経過時間")]
//private float m_Time;


//[SerializeField, Tooltip("状態")]
//private HackingBossPhaseState m_HackingBossPhaseState;


//[SerializeField, Tooltip("発射から現在までの時刻を表す変数")]
//public OperationFloatBase m_DTimeOperation;

//[SerializeField, Tooltip("発射からの時刻を表す変数")]
//public OperationFloatVariable m_TimeOperation;

//[SerializeField, Tooltip("発射時のパラメータを表す変数")]
//public ShotParamOperationVariable m_LaunchParam;


//[SerializeField, Tooltip("前のフレームでの時刻")]
//private OperationFloatVariable m_PreviousTime;

//[SerializeField, Tooltip("ハッキングの開始からの時刻を表す変数（どの演算からも参照されないなら、float型で良さそう（？））")]
//private OperationFloatVariable m_Time;

//[SerializeField, Tooltip("発射時刻（引数の代わり）（必要なのは移行期間だけだと思う）")]
//public OperationFloatVariable m_ArgumentTime;

//[SerializeField, Tooltip("自機の位置")]
//private OperationVector2Variable m_PlayerPosition;


//m_CommonOperationVariable.m_PreviousTime.Value = m_CommonOperationVariable.m_Time.Value;
//m_CommonOperationVariable.m_Time.Value += Time.deltaTime;

//Vector3 playerPositionvec3 = BattleHackingPlayerManager.Instance.Player.transform.position;
//Vector2 playerPositionVec2 = new Vector2(playerPositionvec3.x, playerPositionvec3.z);
//m_CommonOperationVariable.m_PlayerPosition.Value = playerPositionVec2;

//m_CommonOperationVariable.m_ArgumentTime.Value = m_CommonOperationVariable.m_Time.Value;
