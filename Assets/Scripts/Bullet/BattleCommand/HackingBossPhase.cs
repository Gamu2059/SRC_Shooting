#pragma warning disable 0649

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

    [SerializeField, Tooltip("難易度変動演算初期化オブジェクト")]
    private DifficultyInitializerBase m_DifficultyInitializer;

    [SerializeField, Tooltip("攻撃が始まる前の初期化処理")]
    private ForSetupBase m_ForSetup;

    [SerializeField, Tooltip("多重forループ")]
    private ForBase m_MultiForLoop;

    [SerializeField, Tooltip("敵本体の物理的な状態")]
    private TransformOperation m_BossTransform;

    [SerializeField, Tooltip("弾幕")]
    private BulletShotParamBase m_DanmakuArray;

    [SerializeField, Tooltip("BGM再生用のパラメータ")]
    private PlaySoundParam m_PlaySoundParam;
    public PlaySoundParam PlaySoundParam
    {
        get { return m_PlaySoundParam; }
    }

    /// <summary>
    /// この攻撃の開始からの時刻
    /// </summary>
    private float m_Time;


    public void OnStarts()
    {
        m_Time = 0;
        BulletTime.Time = 0;

        m_DifficultyInitializer.Setup();

        if (m_ForSetup != null)
        {
            m_ForSetup.Setup();
        }

        m_DanmakuArray.OnStarts();

        if (m_MultiForLoop != null)
        {
            m_MultiForLoop.Setup();
        }
    }


    public TransformSimple OnUpdates(BattleHackingBossBehavior boss)
    {
        m_Time += Time.deltaTime;
        BulletTime.Time = m_Time;

        TransformSimple transform = null;

        if (m_MultiForLoop == null)
        {
            transform = m_BossTransform.GetResultTransform();
        }
        else
        {
            for (m_MultiForLoop.Init(); m_MultiForLoop.IsTrue(); m_MultiForLoop.Process())
            {
                transform = m_BossTransform.GetResultTransform();
            }
        }

        m_DanmakuArray.OnUpdates(boss.GetEnemy());

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


//if (m_MultiForLoop == null ? true : m_MultiForLoop.Init())
//{
//    do
//    {
//        transform = m_BossTransform.GetResultTransform();
//    }
//    while (m_MultiForLoop == null ? false : m_MultiForLoop.Process());
//}


//if (m_DifficultyInitializer != null)
//{
//    m_DifficultyInitializer.Setup();
//}

//BattleHackingFreeTrajectoryBulletController.CommonOperationVar = m_CommonOperationVariable;


//[SerializeField, Tooltip("ゲーム全体で共通の変数へのリンク")]
//private CommonOperationVariable m_CommonOperationVariable;


//foreach (DifficultyInitializerBase difficultyInitializer in m_DifficultyInitializer)
//{
//    difficultyInitializer.Setup();
//}
