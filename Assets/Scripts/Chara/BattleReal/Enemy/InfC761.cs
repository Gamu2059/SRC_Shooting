using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// Inf-C-761のコントローラ
/// </summary>
public class InfC761 : BattleRealEnemyController
{
    public enum E_PHASE
    {
        START,
        ATTACK,
        DOWN,
        HACKING_SUCCESS,
        CHANGE_ATTACK,
        DEAD,
        RESCUE,
        END,
    }

    #region Field

    private StateMachine<E_PHASE> m_StateMachine;
    private BattleRealBossBehaviorParamSet[] m_AttackParamSets;
    private BattleRealBossBehaviorParamSet[] m_DownParamSets;

    private List<BattleRealBossBehavior> m_AttackBehaviors;
    private List<BattleRealBossBehavior> m_DownBehaviors;

    private BattleRealBossBehavior m_CurrentAttack;
    private BattleRealBossBehavior m_CurrentDown;

    private int m_AttackPhase;
    private int m_DownPhase;
    private float m_DownHp;

    #endregion

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is InfC761ParamSet paramSet)
        {
            m_AttackParamSets = paramSet.AttackParamSets;
            m_DownParamSets = paramSet.DownParamSets;
        }
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_IsLookMoveDir = false;
        m_WillDestroyOnOutOfEnemyField = false;

        m_AttackBehaviors = new List<BattleRealBossBehavior>();
        m_DownBehaviors = new List<BattleRealBossBehavior>();

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

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.DOWN)
        {
            m_OnStart = StartOnDown,
            m_OnUpdate = UpdateOnDown,
            m_OnLateUpdate = LateUpdateOnDown,
            m_OnFixedUpdate = FixedUpdateOnDown,
            m_OnEnd = EndOnDown,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.HACKING_SUCCESS)
        {
            m_OnStart = StartOnHackingSuccess,
            m_OnUpdate = UpdateOnHackingSuccess,
            m_OnLateUpdate = LateUpdateOnHackingSuccess,
            m_OnFixedUpdate = FixedUpdateOnHackingSuccess,
            m_OnEnd = EndOnHackingSuccess,
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

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.RESCUE)
        {
            m_OnStart = StartOnRescue,
            m_OnUpdate = UpdateOnRescue,
            m_OnLateUpdate = LateUpdateOnRescue,
            m_OnFixedUpdate = FixedUpdateOnRescue,
            m_OnEnd = EndOnRescue,
        });

        m_StateMachine.AddState(new State<E_PHASE>(E_PHASE.END)
        {
            m_OnStart = StartOnEnd,
            m_OnUpdate = UpdateOnEnd,
            m_OnLateUpdate = LateUpdateOnEnd,
            m_OnFixedUpdate = FixedUpdateOnEnd,
            m_OnEnd = EndOnEnd,
        });

        InitializeAttackBehaviors();
        InitializeDownBehaviors();

        RequestChangeState(E_PHASE.START);
    }

    public override void OnFinalize()
    {
        if (m_DownBehaviors != null)
        {
            foreach (var b in m_DownBehaviors)
            {
                b.OnFinalize();
            }
            m_DownBehaviors.Clear();
            m_DownBehaviors = null;
        }

        if (m_AttackBehaviors != null)
        {
            foreach (var b in m_AttackBehaviors)
            {
                b.OnFinalize();
            }
            m_AttackBehaviors.Clear();
            m_AttackBehaviors = null;
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

    private BattleRealBossBehavior CreateBehavior(BattleRealBossBehaviorParamSet bossBehaviorParamSet)
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

        if (type == null || !type.IsSubclassOf(typeof(BattleRealBossBehavior)))
        {
            return null;
        }

        var cstr = type.GetConstructor(new[] { typeof(BattleRealEnemyController), typeof(BattleRealBossBehaviorParamSet) });
        if (cstr == null)
        {
            return null;
        }

        return cstr.Invoke(new object[] { this, bossBehaviorParamSet }) as BattleRealBossBehavior;
    }

    private void InitializeAttackBehaviors()
    {
        for (int i = 0; i < m_AttackParamSets.Length; i++)
        {
            var param = m_AttackParamSets[i];
            var behavior = CreateBehavior(param);
            if (behavior == null)
            {
                Debug.LogError("ボスの振る舞いを生成できませんでした。Type:" + param.BehaviorClass);
            }

            behavior.OnInitialize();
            m_AttackBehaviors.Add(behavior);
        }
    }

    private void InitializeDownBehaviors()
    {
        for (int i = 0; i < m_DownParamSets.Length; i++)
        {
            var param = m_DownParamSets[i];
            var behavior = CreateBehavior(param);
            if (behavior == null)
            {
                Debug.LogError("ボスの振る舞いを生成できませんでした。Type:" + param.BehaviorClass);
            }

            behavior.OnInitialize();
            m_DownBehaviors.Add(behavior);
        }
    }

    public void RequestChangeState(E_PHASE state)
    {
        m_StateMachine.Goto(state);
    }

    #region Start State

    private void StartOnStart()
    {
        // 即席処理
        m_CurrentAttack = m_AttackBehaviors[0];
        m_CurrentDown = m_DownBehaviors[0];

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
        m_CurrentAttack?.OnStart();
    }

    private void UpdateOnAttack()
    {
        m_CurrentAttack?.OnUpdate();
    }

    private void LateUpdateOnAttack()
    {
        m_CurrentAttack?.OnLateUpdate();
    }

    private void FixedUpdateOnAttack()
    {
        m_CurrentAttack?.OnFixedUpdate();
    }

    private void EndOnAttack()
    {
        m_CurrentAttack?.OnEnd();
    }

    #endregion

    #region Down State

    private void StartOnDown()
    {
        m_CurrentDown?.OnStart();
    }

    private void UpdateOnDown()
    {
        m_CurrentDown?.OnUpdate();
    }

    private void LateUpdateOnDown()
    {
        m_CurrentDown?.OnLateUpdate();
    }

    private void FixedUpdateOnDown()
    {
        m_CurrentDown?.OnFixedUpdate();
    }

    private void EndOnDown()
    {
        m_CurrentDown?.OnEnd();
    }

    #endregion

    #region Hacking Success State

    private void StartOnHackingSuccess()
    {

    }

    private void UpdateOnHackingSuccess()
    {

    }

    private void LateUpdateOnHackingSuccess()
    {

    }

    private void FixedUpdateOnHackingSuccess()
    {

    }

    private void EndOnHackingSuccess()
    {

    }

    #endregion

    #region Change Attack State

    private void StartOnChangeAttack()
    {

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

    #region Rescue State

    private void StartOnRescue()
    {

    }

    private void UpdateOnRescue()
    {

    }

    private void LateUpdateOnRescue()
    {

    }

    private void FixedUpdateOnRescue()
    {

    }

    private void EndOnRescue()
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
