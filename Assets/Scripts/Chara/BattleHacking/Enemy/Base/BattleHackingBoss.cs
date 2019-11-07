using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ハッキングモードの複数の動作を持つボスの動きを定義したコントローラ
/// </summary>
public class BattleHackingBoss : BattleHackingEnemyController
{
    public enum E_PHASE
    {
        START,
        ATTACK,
        CHANGE_ATTACK,
        DEAD,
        END,
    }

    #region Field

    protected BattleHackingBossParamSet m_BossParamSet;

    protected StateMachine<E_PHASE> m_StateMachine;
    protected BattleHackingBossBehaviorParamSet[] m_BehaviorParamSets;

    protected List<BattleHackingBossBehavior> m_Behaviors;
    protected BattleHackingBossBehavior m_CurrentBehavior;
    protected int m_AttackPhase;

    #endregion

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is BattleHackingBossParamSet paramSet)
        {
            m_BossParamSet = paramSet;
            m_BehaviorParamSets = paramSet.BehaviorParamSets;
        }
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IsLookMoveDir = false;
        m_WillDestroyOnOutOfEnemyField = false;

        m_Behaviors = new List<BattleHackingBossBehavior>();

        m_StateMachine = new StateMachine<E_PHASE>();

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.START)
        {
            m_OnStart = StartOnStart,
            m_OnUpdate = UpdateOnStart,
            m_OnLateUpdate = LateUpdateOnStart,
            m_OnFixedUpdate = FixedUpdateOnStart,
            m_OnEnd = EndOnStart,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.ATTACK)
        {
            m_OnStart = StartOnAttack,
            m_OnUpdate = UpdateOnAttack,
            m_OnLateUpdate = LateUpdateOnAttack,
            m_OnFixedUpdate = FixedUpdateOnAttack,
            m_OnEnd = EndOnAttack,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.CHANGE_ATTACK)
        {
            m_OnStart = StartOnChangeAttack,
            m_OnUpdate = UpdateOnChangeAttack,
            m_OnLateUpdate = LateUpdateOnChangeAttack,
            m_OnFixedUpdate = FixedUpdateOnChangeAttack,
            m_OnEnd = EndOnChangeAttack,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.DEAD)
        {
            m_OnStart = StartOnDead,
            m_OnUpdate = UpdateOnDead,
            m_OnLateUpdate = LateUpdateOnDead,
            m_OnFixedUpdate = FixedUpdateOnDead,
            m_OnEnd = EndOnDead,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.END)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        InitializeBehaviors();

        RequestChangeState(E_PHASE.START);
    }

    public override void OnFinalize()
    {
        if (m_Behaviors != null)
        {
            foreach (var b in m_Behaviors)
            {
                b.OnFinalize();
            }
            m_Behaviors.Clear();
            m_Behaviors = null;
        }

        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_StateMachine.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_StateMachine.OnFixedUpdate();
    }

    #endregion

    private BattleHackingBossBehavior CreateBehavior(BattleHackingBossBehaviorParamSet bossBehaviorParamSet)
    {
        if (bossBehaviorParamSet == null)
        {
            return null;
        }

        Type type = null;
        try
        {
            type = Type.GetType(bossBehaviorParamSet.BehaviorClass);
        }
        catch (Exception)
        {
            return null;
        }

        if (type == null || !type.IsSubclassOf(typeof(BattleHackingBossBehavior)))
        {
            return null;
        }

        var cstr = type.GetConstructor(new[] { typeof(BattleHackingEnemyController), typeof(BattleHackingBossBehaviorParamSet) });
        if (cstr == null)
        {
            return null;
        }

        return cstr.Invoke(new object[] { this, bossBehaviorParamSet }) as BattleHackingBossBehavior;
    }

    private void InitializeBehaviors()
    {
        for (int i = 0; i < m_BehaviorParamSets.Length; i++)
        {
            var param = m_BehaviorParamSets[i];
            var behavior = CreateBehavior(param);
            if (behavior == null)
            {
                Debug.LogError("ボスの振る舞いを生成できませんでした。Type:" + param.BehaviorClass);
            }

            behavior.OnInitialize();
            m_Behaviors.Add(behavior);
        }
    }

    public void RequestChangeState(E_PHASE state)
    {
        m_StateMachine.Goto(state);
    }

    #region Start State

    private void StartOnStart()
    {
        m_AttackPhase = 0;
        m_CurrentBehavior = m_Behaviors[m_AttackPhase];

        transform.position = new Vector3(0, 0, 1);

        RequestChangeState(E_PHASE.ATTACK);
    }

    private void UpdateOnStart()
    {

    }

    private void LateUpdateOnStart()
    {

    }

    private void FixedUpdateOnStart()
    {

    }

    private void EndOnStart()
    {

    }

    #endregion

    #region Attack State

    private void StartOnAttack()
    {
        m_CurrentBehavior?.OnStart();
    }

    private void UpdateOnAttack()
    {
        m_CurrentBehavior?.OnUpdate();
    }

    private void LateUpdateOnAttack()
    {
        m_CurrentBehavior?.OnLateUpdate();
    }

    private void FixedUpdateOnAttack()
    {
        m_CurrentBehavior?.OnFixedUpdate();
    }

    private void EndOnAttack()
    {
        m_CurrentBehavior?.OnEnd();
    }

    #endregion

    #region Change Attack State

    private void StartOnChangeAttack()
    {
        m_AttackPhase = Mathf.Min(m_AttackPhase + 1, m_Behaviors.Count - 1);
        m_CurrentBehavior = m_Behaviors[m_AttackPhase];
        RequestChangeState(E_PHASE.ATTACK);
    }

    private void UpdateOnChangeAttack()
    {

    }

    private void LateUpdateOnChangeAttack()
    {

    }

    private void FixedUpdateOnChangeAttack()
    {

    }

    private void EndOnChangeAttack()
    {

    }

    #endregion

    #region Dead State

    private void StartOnDead()
    {

    }

    private void UpdateOnDead()
    {

    }

    private void LateUpdateOnDead()
    {

    }

    private void FixedUpdateOnDead()
    {

    }

    private void EndOnDead()
    {

    }

    #endregion

    #region End State

    private void StartOnEnd()
    {

    }

    private void UpdateOnEnd()
    {

    }

    private void LateUpdateOnEnd()
    {

    }

    private void FixedUpdateOnEnd()
    {

    }

    private void EndOnEnd()
    {

    }

    #endregion
}
