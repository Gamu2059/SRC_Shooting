using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードのボスを制御するクラス。
/// </summary>
public partial class BattleRealBossController : BattleRealEnemyBase
{
    #region Define

    private enum E_STATE
    {
        /// <summary>
        /// 生成直後に遷移するステート
        /// </summary>
        START,

        /// <summary>
        /// 移動したり攻撃したりするためのステート
        /// </summary>
        BEHAVIOR,

        /// <summary>
        /// ダウンした時の移動ステート
        /// </summary>
        DOWN_BEHAVIOR,

        /// <summary>
        /// 攻撃等のフェーズを変化させる時に遷移するステート
        /// </summary>
        CHANGE_BEHAVIOR,

        /// <summary>
        /// ハッキングに成功した時に遷移するステート
        /// </summary>
        HACKING_SUCCESS,

        /// <summary>
        /// ハッキングに失敗した時に遷移するステート
        /// </summary>
        HACKING_FAILURE,

        /// <summary>
        /// シーケンスによる自動制御を受けるステート
        /// </summary>
        SEQUENCE,

        /// <summary>
        /// 撃破された時に遷移するステート
        /// </summary>
        DEAD,

        /// <summary>
        /// 救出された時に遷移するステート
        /// </summary>
        RESCUE,
    }

    private class StateCycle : StateCycleBase<BattleRealBossController, E_STATE> { }

    private class InnerState : State<E_STATE, BattleRealBossController>
    {
        public InnerState(E_STATE state, BattleRealBossController target) : base(state, target) { }
        public InnerState(E_STATE state, BattleRealBossController target, StateCycle cycle) : base(state, target, cycle) { }
    }

    /// <summary>
    /// ボスのコントローラで保持するためのクラス。
    /// </summary>
    private class BehaviorSet
    {
        public E_ENEMY_BEHAVIOR_TYPE BehaviorType;
        public BattleRealEnemyBehaviorUnit Behavior;
        public BattleRealEnemyBehaviorGroup BehaviorGroup;
        public E_ENEMY_BEHAVIOR_TYPE DownBehaviorType;
        public BattleRealEnemyBehaviorUnit DownBehavior;
        public BattleRealEnemyBehaviorGroup DownBehaviorGroup;
        public BattleHackingLevelParamSet HackingLevelParamSet;
        public int DownHp;
        public float DownHealTime;
        public float HpRateThresholdNextBehavior;
        public ItemCreateParam HackingSuccessItemParam;
        public SequenceGroup StartSequenceGroup;
    }

    #endregion

    #region Field

    private StateMachine<E_STATE, BattleRealBossController> m_StateMachine;
    private BattleRealBossParam m_BossParam;
    private List<BehaviorSet> m_BehaviorSets;

    private SequenceGroup m_ReservedSequenceGroup;

    private int m_CurrentBehaviorSetIndex;
    private BehaviorSet m_CurrentBehaviorSet;

    private BattleRealEnemyBehaviorController m_BehaviorController;
    private BattleRealEnemyBehaviorController m_DownBehaviorController;

    public float NowDownHp { get; private set; }
    public float MaxDownHp { get; private set; }
    public int HackingCompleteNum { get; private set; }
    public int HackingSuccessCount { get; private set; }

    private Transform m_EnemyBodyCollider;
    private int m_CarryOverHackingBossDamage;

    #endregion

    #region Game Cycle

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();
        m_BossParam = Param as BattleRealBossParam;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_STATE, BattleRealBossController>();
        m_StateMachine.AddState(new InnerState(E_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_STATE.BEHAVIOR, this, new BehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DOWN_BEHAVIOR, this, new DownBehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.CHANGE_BEHAVIOR, this, new ChangeBehaviorState()));
        m_StateMachine.AddState(new InnerState(E_STATE.HACKING_SUCCESS, this, new HackingSuccessState()));
        m_StateMachine.AddState(new InnerState(E_STATE.HACKING_FAILURE, this, new HackingFailureState()));
        m_StateMachine.AddState(new InnerState(E_STATE.SEQUENCE, this, new SequenceState()));
        m_StateMachine.AddState(new InnerState(E_STATE.DEAD, this, new DeadState()));
        m_StateMachine.AddState(new InnerState(E_STATE.RESCUE, this, new RescueState()));

        m_EnemyBodyCollider = GetCollider().GetColliderTransform(E_COLLIDER_TYPE.ENEMY_MAIN_BODY)?.Transform;
        if (m_EnemyBodyCollider == null)
        {
            Debug.LogError("[{0}] : ENEMY_BODYタイプのコライダーを見つけることができませんでした");
        }

        m_CarryOverHackingBossDamage = 0;
        m_ReservedSequenceGroup = null;

        m_BehaviorController = new BattleRealEnemyBehaviorController(this);
        m_DownBehaviorController = new BattleRealEnemyBehaviorController(this);
        m_BehaviorController.OnInitialize();
        m_DownBehaviorController.OnInitialize();

        InitializeBehaviorSet();

        WillDestroyOnOutOfEnemyField = false;
        IsBoss = true;

        RequestChangeState(E_STATE.START);
    }

    private void InitializeBehaviorSet()
    {
        m_BehaviorSets = new List<BehaviorSet>();
        for (var i = 0; i < m_BossParam.BehaviorSets.Length; i++)
        {
            var originBehaviorSet = m_BossParam.BehaviorSets[i];
            var behaviorSet = new BehaviorSet();

            behaviorSet.BehaviorType = originBehaviorSet.BehaviorType;
            if (behaviorSet.BehaviorType == E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT)
            {
                behaviorSet.Behavior = Instantiate(originBehaviorSet.Behavior);
                behaviorSet.Behavior.OnStartUnit(this, null);
            }
            else
            {
                // BehaviorControllerでInstantiateするので参照代入
                behaviorSet.BehaviorGroup = originBehaviorSet.BehaviorGroup;
            }

            behaviorSet.DownBehaviorType = originBehaviorSet.DownBehaviorType;
            if (behaviorSet.DownBehaviorType == E_ENEMY_BEHAVIOR_TYPE.BEHAVIOR_UNIT)
            {
                behaviorSet.DownBehavior = Instantiate(originBehaviorSet.DownBehavior);
                behaviorSet.DownBehavior.OnStartUnit(this, null);
            }
            else
            {
                // DownBehaviorControllerでInstantiateするので参照代入
                behaviorSet.DownBehaviorGroup = originBehaviorSet.DownBehaviorGroup;
            }

            if (originBehaviorSet.HackingLevelParamSet == null)
            {
                Debug.LogWarningFormat("[{0}] : HackingLevelParamSetが登録されていません。 index : {1}", GetType().Name, i);
            }
            else
            {
                behaviorSet.HackingLevelParamSet = Instantiate(originBehaviorSet.HackingLevelParamSet);

            }
            behaviorSet.DownHp = originBehaviorSet.DownHp;
            behaviorSet.DownHealTime = originBehaviorSet.DownHealTime;
            behaviorSet.HpRateThresholdNextBehavior = originBehaviorSet.HpRateThresholdNextBehavior;
            behaviorSet.HackingSuccessItemParam = originBehaviorSet.HackingSuccessItemParam;
            behaviorSet.StartSequenceGroup = originBehaviorSet.StartSequenceGroup;

            m_BehaviorSets.Add(behaviorSet);
        }
    }

    public override void OnFinalize()
    {
        m_DownBehaviorController.OnFinalize();
        m_BehaviorController.OnFinalize();
        m_DownBehaviorController = null;
        m_BehaviorController = null;

        // 自身が作成したエフェクトを全て破棄する
        BattleRealEffectManager.Instance.DestroyEffectByOwner(transform, true);

        m_CurrentBehaviorSetIndex = -1;
        m_CurrentBehaviorSet = null;
        if (m_BehaviorSets != null)
        {
            m_BehaviorSets.Clear();
            m_BehaviorSets = null;
        }

        m_StateMachine.OnFinalize();
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();
        Debug.Log(m_StateMachine.CurrentState.Key);
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

    #region Suffer

    protected override void OnEnterSufferBullet(HitSufferData<BulletController> sufferData)
    {
        base.OnEnterSufferBullet(sufferData);

        var colliderType = sufferData.SufferCollider.Transform.ColliderType;
        if (colliderType == E_COLLIDER_TYPE.CRITICAL)
        {
            // 通常状態の時にエナジー弾を食らうとダウンダメージになる
            var currentState = m_StateMachine.CurrentState.Key;
            if (currentState == E_STATE.BEHAVIOR)
            {
                switch (sufferData.OpponentObject)
                {
                    case BattleRealPlayerMainBullet hackerBullet:
                        DamageDownHp(hackerBullet.GetNowDownDamage());
                        break;
                }
            }
        }
    }

    protected override void OnEnterSufferChara(HitSufferData<BattleRealCharaController> sufferData)
    {
        base.OnEnterSufferChara(sufferData);
        var sufferType = sufferData.SufferCollider.Transform.ColliderType;
        switch (sufferType)
        {
            case E_COLLIDER_TYPE.CRITICAL:
                break;
            case E_COLLIDER_TYPE.ENEMY_HACKING:
                var currentState = m_StateMachine.CurrentState.Key;
                if (currentState == E_STATE.DOWN_BEHAVIOR)
                {
                    var colliderType = sufferData.HitCollider.Transform.ColliderType;
                    if (colliderType == E_COLLIDER_TYPE.PLAYER_HACKING)
                    {
                        HackingDataHolder.HackingLevelParamSet = m_CurrentBehaviorSet.HackingLevelParamSet;
                        HackingDataHolder.CarryOverHackingBossDamage = m_CarryOverHackingBossDamage;
                        BattleRealEnemyManager.Instance.RequestToHacking();
                    }
                }
                break;
        }
    }

    #endregion

    /// <summary>
    /// ボスのステートを変更する。
    /// </summary>
    private void RequestChangeState(E_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// このボスのダウンHPを回復する。回復量は0より大きくなければ処理されない。
    /// </summary>
    public void RecoverDownHp(float recover)
    {
        if (recover <= 0)
        {
            return;
        }

        NowDownHp = Mathf.Clamp(NowDownHp + recover, 0, MaxDownHp);
        OnRecoverDownHp();
    }

    protected virtual void OnRecoverDownHp()
    {

    }

    /// <summary>
    /// このボスのダウンHPを削る。ダメージ量は0より大きくなければ処理されない。
    /// </summary>
    public void DamageDownHp(float damage)
    {
        if (damage <= 0)
        {
            return;
        }

        NowDownHp = Mathf.Clamp(NowDownHp - damage, 0, MaxDownHp);
        OnDamageDownHp();

        if (NowDownHp <= 0)
        {
            RequestChangeState(E_STATE.DOWN_BEHAVIOR);
        }
    }

    protected virtual void OnDamageDownHp()
    {

    }

    /// <summary>
    /// 指定したインデックスの振る舞いに切り替える。
    /// </summary>
    private void ChangeBehaviorSet(int index)
    {
        if (m_BehaviorSets == null)
        {
            return;
        }

        if (index < 0 || index >= m_BehaviorSets.Count)
        {
            return;
        }

        var behaviorSet = m_BehaviorSets[index];
        if (behaviorSet == null)
        {
            Debug.LogWarningFormat("[{0}] : BehaviorSet is null. index : {1}", GetType().Name, index);
            return;
        }

        m_CurrentBehaviorSetIndex = index;
        m_CurrentBehaviorSet = behaviorSet;

        if (MaxDownHp > 0)
        {
            // MaxDownHpが0より大きい時は、割合を保持しながら変更する
            var rate = NowDownHp / MaxDownHp;
            MaxDownHp = m_CurrentBehaviorSet.DownHp;
            NowDownHp = m_CurrentBehaviorSet.DownHp * rate;
        }
        else
        {
            // MaxDownHpが0の時は直代入する
            MaxDownHp = m_CurrentBehaviorSet.DownHp;
            NowDownHp = 0;
        }
    }

    /// <summary>
    /// シーケンスによる制御を開始する。
    /// </summary>
    private void StartSequence(SequenceGroup sequenceGroup)
    {
        if (sequenceGroup == null)
        {
            return;
        }

        m_ReservedSequenceGroup = sequenceGroup;
        RequestChangeState(E_STATE.SEQUENCE);
    }

    /// <summary>
    /// HPが0になって死亡した時の処理。
    /// </summary>
    protected sealed override void OnDead()
    {
        base.OnDead();
        RequestChangeState(E_STATE.DEAD);
    }

    protected void OnRescueDestroy()
    {
        BattleRealItemManager.Instance.CreateItem(transform.position, m_BossParam.RescueItemParam);
        DataManager.Instance.BattleData.AddScore(m_BossParam.RescueScore);

        Destroy();
    }

    protected void ExecuteResuceEvent()
    {
        BattleRealEventManager.Instance.AddEvent(m_BossParam.RescueEvents);
    }
}
