#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// INF-C-761の第1段階の挙動。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/BattleReal/INF-C-761/Phase1", fileName = "phase1.inf_c_761.asset", order = 1)]
public class InfC761Phase1Behavior : BattleRealEnemyBehaviorUnit
{
    #region Define

    private enum E_STATE
    {
        START,
        WAIT,
        MOVE_TO_RIGHT,
        MOVE_TO_LEFT,
    }

    private enum E_SHOT_STATE
    {
        NONE,
        PHASE1,
    }

    private enum E_RAPID_SHOT_EDGE
    {
        RIGHT,
        LEFT,
    }

    private class StateCycle : StateCycleBase<InfC761Phase1Behavior, E_STATE> { }

    private class InnerState : State<E_STATE, InfC761Phase1Behavior>
    {
        public InnerState(E_STATE state, InfC761Phase1Behavior target) : base(state, target) { }
        public InnerState(E_STATE state, InfC761Phase1Behavior target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field Inspector Readonly!

    [SerializeField]
    private Vector3 m_BasePos;
    public Vector3 BasePos => m_BasePos;

    [SerializeField]
    private float m_StartDuration;
    public float StartDuration => m_StartDuration;

    [Header("Move Param")]
    [SerializeField]
    private BossMoveParam[] m_MoveParams;
    public BossMoveParam[] MoveParams => m_MoveParams;

    [SerializeField]
    private float[] m_Amplitudes;
    public float[] Amplitudes => m_Amplitudes;

    [SerializeField]
    private float[] m_NextMoveWaitTimes;
    public float[] NextMoveWaitTimes => m_NextMoveWaitTimes;

    [SerializeField]
    private float[] m_MoveDurations;
    public float[] MoveDurations => m_MoveDurations;

    [SerializeField]
    private AnimationCurve[] m_NormalizedRates;
    public AnimationCurve[] NormalizedRates => m_NormalizedRates;

    [SerializeField]
    private float[] m_GenericDurations;
    public float[] GenericDurations => m_GenericDurations;

    [Header("Shot Param")]
    [SerializeField]
    private float[] m_ShotCoolTimes;
    public float[] ShotCoolTimes => m_ShotCoolTimes;

    [SerializeField]
    private EnemyShotParam[] m_ShotParams;
    public EnemyShotParam[] ShotParams => m_ShotParams;

    [SerializeField]
    private Vector3[] m_ShotOffSets;
    public Vector3[] ShotOffSets => m_ShotOffSets;

    [SerializeField]
    private int m_NumberOfChangeBullet;
    public int NumberOfChangeBullet => m_NumberOfChangeBullet;

    [SerializeField]
    private int m_NumberOfRapidShot;
    public int NumberOfRapidShot => m_NumberOfRapidShot;

    #endregion

    #region Field

    private StateMachine<E_STATE, InfC761Phase1Behavior> m_StateMachine;

    private Vector3 m_MoveStartPos;
    private Vector3 m_MoveEndPos;

    private float m_Duration;
    private float m_TimeCount;

    private float m_LargeBulletShotTimeCount;
    private int m_NumberOfShotLargeBullet;
    private float m_SmallBulletShotTimeCount;

    private E_RAPID_SHOT_EDGE m_ShotEdge;
    private float m_RectShotBulletTimeCount;
    private bool m_IsShotRectBullet;
    private float m_RectRapidShotBulletTimeCount;
    private int m_NumberOfRapidShotRectBullet;

    private E_SHOT_STATE m_ShotPhase;

    #endregion

    #region Game Cycle

    protected override void OnStart()
    {
        base.OnStart();

        m_StateMachine = new StateMachine<E_STATE, InfC761Phase1Behavior>();
        m_StateMachine.AddState(new InnerState(E_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_STATE.WAIT, this, new WaitState()));
        m_StateMachine.AddState(new InnerState(E_STATE.MOVE_TO_LEFT, this, new MoveToLeftState()));
        m_StateMachine.AddState(new InnerState(E_STATE.MOVE_TO_RIGHT, this, new MoveToRightState()));

        m_MoveStartPos = Enemy.transform.position;
        m_MoveEndPos = BasePos;
        m_TimeCount = 0;
        m_Duration = StartDuration;
        m_ShotPhase = E_SHOT_STATE.NONE;
        m_LargeBulletShotTimeCount = 0f;
        m_NumberOfShotLargeBullet = 0;
        m_SmallBulletShotTimeCount = 0f;
        m_ShotEdge = E_RAPID_SHOT_EDGE.RIGHT;
        m_RectShotBulletTimeCount = 0;
        m_IsShotRectBullet = false;
        m_RectRapidShotBulletTimeCount = 0f;
        m_NumberOfRapidShotRectBullet = 0;

        RequestChangeState(E_STATE.START);
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        m_StateMachine.OnUpdate();
    }

    protected override void OnEnd()
    {
        m_StateMachine.OnFinalize();
        m_StateMachine = null;
        base.OnEnd();
    }

    #endregion

    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    private Vector3 GetMovePosition(int normarizedRateIndex)
    {
        var rate = NormalizedRates[normarizedRateIndex];
        var duration = rate.keys[rate.keys.Length - 1].time;
        var t = rate.Evaluate(m_TimeCount * duration / m_Duration);
        return Vector3.Lerp(m_MoveStartPos, m_MoveEndPos, t);
    }

    private class StartState : StateCycle
    {
        public override void OnUpdate()
        {
            base.OnUpdate();
            Target.m_TimeCount += Time.deltaTime;
            Target.Enemy.transform.position = Target.GetMovePosition(0);
            if (Target.m_TimeCount >= Target.m_Duration)
            {

            }
        }
    }

    private class WaitState : StateCycle
    {

    }

    private class MoveToLeftState : StateCycle
    {

    }

    private class MoveToRightState : StateCycle
    {

    }
}
