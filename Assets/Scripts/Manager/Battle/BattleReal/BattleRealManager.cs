using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バトルのリアルモードの処理を管理する。
/// </summary>
[Serializable]
public partial class BattleRealManager : ControllableObject, IStateCallback<E_BATTLE_REAL_STATE>
{
    #region Define

    public class StateCycle : StateCycleBase<BattleRealManager, E_BATTLE_REAL_STATE> { }

    public class InnerState : State<E_BATTLE_REAL_STATE, BattleRealManager>
    {
        public InnerState(E_BATTLE_REAL_STATE state, BattleRealManager target) : base(state, target) { }
        public InnerState(E_BATTLE_REAL_STATE state, BattleRealManager target, StateCycle cycle) : base(state, target, cycle) { }
    }

    #endregion

    #region Field

    private BattleRealParamSet m_ParamSet;
    private StateMachine<E_BATTLE_REAL_STATE, BattleRealManager> m_StateMachine;
    private BattleManager m_BattleManager;

    /// <summary>
    /// カットシーン呼び出し制御用インスタンス
    /// </summary>
    private CutsceneCaller m_CutsceneCaller;

    /// <summary>
    /// 会話呼び出し制御用インスタンス
    /// </summary>
    private TalkCaller m_TalkCaller;

    #endregion

    #region Open Callback

    public Action<E_BATTLE_REAL_STATE> ChangeStateAction { get; set; }

    #endregion

    public static BattleRealManager Builder(BattleManager battleManager, BattleRealParamSet param)
    {
        var manager = new BattleRealManager();
        manager.m_BattleManager = battleManager;
        manager.SetParam(param);
        manager.OnInitialize();
        return manager;
    }

    private void SetParam(BattleRealParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_StateMachine = new StateMachine<E_BATTLE_REAL_STATE, BattleRealManager>();
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.START, this, new StartState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME, this, new GameState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.DEAD, this, new DeadState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.TO_HACKING, this, new ToHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.STAY_HACKING, this, new StayHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.FROM_HACKING, this, new FromHackingState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.WAIT_CUTSCENE, this, new WaitCutSceneState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.WAIT_TALK, this, new WaitTalkState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.WAIT_DIALOG, this, new WaitDialogState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME_CLEAR, this, new GameClearState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.GAME_OVER, this, new GameOverState()));
        m_StateMachine.AddState(new InnerState(E_BATTLE_REAL_STATE.END, this, new EndState()));

        BattleRealInputManager.Builder();
        BattleRealTimerManager.Builder();
        BattleRealEventManager.Builder(this, m_ParamSet.EventTriggerParamSet);
        BattleRealPlayerManager.Builder(this, m_ParamSet.PlayerManagerParamSet);
        BattleRealEnemyGroupManager.Builder();
        BattleRealEnemyManager.Builder(this, m_ParamSet.EnemyManagerParamSet);
        BattleRealBulletGeneratorManager.Builder();
        BattleRealBulletManager.Builder(this, m_ParamSet.BulletManagerParamSet);
        BattleRealItemManager.Builder(this, m_ParamSet.ItemManagerParamSet);
        BattleRealEffectManager.Builder(this);
        BattleRealCollisionManager.Builder();
        BattleRealCameraManager.Instance.OnInitialize();
        BattleRealUiManager.Instance.OnInitialize();
        BattleRealUiManager.Instance.SetCallback(this);

        m_CutsceneCaller = null;
        m_TalkCaller = null;
    }

    public override void OnFinalize()
    {
        m_TalkCaller?.StopTalk();
        m_TalkCaller = null;

        m_CutsceneCaller?.StopCutscene();
        m_CutsceneCaller = null;

        ChangeStateAction = null;

        BattleRealUiManager.Instance.OnFinalize();
        BattleRealCameraManager.Instance.OnFinalize();
        BattleRealCollisionManager.Instance.OnFinalize();
        BattleRealEffectManager.Instance.OnFinalize();
        BattleRealItemManager.Instance.OnFinalize();
        BattleRealBulletManager.Instance.OnFinalize();
        BattleRealBulletGeneratorManager.Instance.OnFinalize();
        BattleRealEnemyManager.Instance.OnFinalize();
        BattleRealEnemyGroupManager.Instance.OnFinalize();
        BattleRealPlayerManager.Instance.OnFinalize();
        BattleRealEventManager.Instance.OnFinalize();
        BattleRealTimerManager.Instance.OnFinalize();
        BattleRealInputManager.Instance.OnFinalize();

        m_StateMachine.OnFinalize();
        m_StateMachine = null;
        m_ParamSet = null;
        m_BattleManager = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_StateMachine.OnUpdate();

        if (BattleRealInputManager.Instance.Menu == E_INPUT_STATE.DOWN)
        {
            m_BattleManager.ExitGame();
        }
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

    /// <summary>
    /// BattleRealManagerのステートを変更する。
    /// </summary>
    public void RequestChangeState(E_BATTLE_REAL_STATE state)
    {
        m_StateMachine?.Goto(state);
    }

    /// <summary>
    /// カットシーンを表示する。
    /// </summary>
    public void ShowCutscene(ShowCutsceneParam showCutsceneParam)
    {
        // 使い切りのはずのインスタンスが存在しているということはカットシーンの重複呼び出しであると解釈する
        if (m_CutsceneCaller != null)
        {
            Debug.LogWarning("カットシーンが存在している時にカットシーンを呼び出すことは禁止しています。");
            return;
        }

        m_CutsceneCaller = new CutsceneCaller(showCutsceneParam.CutsceneName,
            controller =>
            {
                RequestChangeState(E_BATTLE_REAL_STATE.WAIT_CUTSCENE);
            },
            () =>
            {
                m_CutsceneCaller = null;
                BattleRealEventManager.Instance.AddEventParam(showCutsceneParam.OnCompletedEvents);

                if (showCutsceneParam.AutoChangeToGameState)
                {
                    RequestChangeState(E_BATTLE_REAL_STATE.GAME);
                }
            },
            () =>
            {
                m_CutsceneCaller = null;
                Debug.LogWarningFormat("カットシーンの呼び出しに失敗しました。 name : {0}", showCutsceneParam.CutsceneName);
            });
    }

    /// <summary>
    /// 会話を表示する。
    /// </summary>
    public void ShowTalk(ShowTalkParam showTalkParam)
    {
        // 使い切りのはずのインスタンスが存在しているということは会話の重複呼び出しであると解釈する
        if (m_TalkCaller != null)
        {
            Debug.LogWarning("会話中に会話を呼び出すことは禁止しています。");
            return;
        }

        m_TalkCaller = new TalkCaller(showTalkParam.ScenarioLabel,
            () =>
            {
                RequestChangeState(E_BATTLE_REAL_STATE.WAIT_TALK);
            },
            () =>
            {
                m_TalkCaller = null;
                BattleRealEventManager.Instance.AddEventParam(showTalkParam.OnCompletedEvents);

                if (showTalkParam.AutoChangeToGameState)
                {
                    RequestChangeState(E_BATTLE_REAL_STATE.GAME);
                }
            },
            () =>
            {
                m_TalkCaller = null;
                Debug.LogWarningFormat("会話の呼び出しに失敗しました。 name : {0}", showTalkParam.ScenarioLabel);
            });
    }

    /// <summary>
    /// ダイアログを表示する。
    /// </summary>
    public void ShowDialog(ShowDialogParam showDialogParam)
    {
        // DialogManager.ShowDialog();
        // ダイアログ終了時の処理を追加したりもする
    }

    public void GameClearWithoutHackingComplete()
    {
        DataManager.Instance.BattleData.SetHackingComplete(false);
        RequestChangeState(E_BATTLE_REAL_STATE.GAME_CLEAR);
    }

    public void GameClearWithHackingComplete()
    {
        DataManager.Instance.BattleData.SetHackingComplete(true);
        RequestChangeState(E_BATTLE_REAL_STATE.GAME_CLEAR);
    }

    public void GameOver()
    {
        RequestChangeState(E_BATTLE_REAL_STATE.GAME_OVER);
    }

    /// <summary>
    /// ハッキング開始をリクエストする。
    /// </summary>
    public void RequestStartHacking()
    {
        m_BattleManager.RequestChangeState(E_BATTLE_STATE.TO_HACKING);
    }

    public void End()
    {
        m_BattleManager.RequestChangeState(E_BATTLE_STATE.END);
    }
}
