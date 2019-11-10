using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761のハッキングモードの一つ目の行動パラメータのセット。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/INF-C-761/Hacker1_Phase1", fileName = "param.inf_c_761_hacker1_phase_1.asset")]
public class InfC761Hacker1Phase1ParamSet : BattleHackingBossBehaviorParamSet
{
    [SerializeField]
    private Vector3 m_BasePos;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_Amplitude;
    public float Amplitude => m_Amplitude;

    [SerializeField]
    private AnimationCurve m_NormalizedRate;
    public AnimationCurve NormalizedRate => m_NormalizedRate;

    [SerializeField]
    private float m_NextMoveWaitTime;
    public float NextMoveWaitTime => m_NextMoveWaitTime;

    [SerializeField]
    private float m_StartDuration;
    public float StartDuration => m_StartDuration;

    [SerializeField]
    private float m_MoveDuration;
    public float MoveDuration => m_MoveDuration;

    [Header("Shot Param")]

    [SerializeField, Tooltip("中央のShot Param")]
    private EnemyShotParam m_CenterShotParam;
    public EnemyShotParam CenterShotParam => m_CenterShotParam;

    [SerializeField, Tooltip("両サイドのShot Param")]
    private EnemyShotParam m_SideShotParam;
    public EnemyShotParam SideShotParam => m_SideShotParam;

    [SerializeField, Tooltip("本来の中央値が取れなかった場合の中央座標")]
    private Vector3 m_AlternativeCenterPos;
    public Vector3 AlternativeCenterPos => m_AlternativeCenterPos;

    [SerializeField]
    private Vector3 m_LeftShotOffset;
    public Vector3 LeftShotOffset => m_LeftShotOffset;

    [SerializeField]
    private Vector3 m_RightShotOffset;
    public Vector3 RigthShotOffset => m_RightShotOffset;


    [SerializeField, Tooltip("この攻撃の開始からの経過時間")]
    private float m_Time;

    [SerializeField, Tooltip("アセット用単位弾幕パラメータの配列")]
    private AllUDFieldArray m_AllUDFieldArray;

    [SerializeField, Tooltip("弾幕の抽象クラスの配列")]
    private DanmakuCountAbstract[] m_DanmakuCountAbstractArray;
}
