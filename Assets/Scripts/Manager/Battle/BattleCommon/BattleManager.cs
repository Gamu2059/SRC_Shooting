using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
    public enum E_BATTLE_STATUS
    {
        /// <summary>
        /// 現在、BattleMainの状態であることを示す。
        /// </summary>
		MAIN,

        /// <summary>
        /// 現在、BattleCommandの状態であることを示す。
        /// </summary>
		COMMAND,

        /// <summary>
        /// BattleCommandへの遷移中であることを示す。
        /// </summary>
        TRANSITION_COMMAND,

        /// <summary>
        /// BattleMainへの遷移中であることを示す。
        /// </summary>
        TRANSITION_MAIN,
    }

    #region Field Inspector

    /// <summary>
    /// メインのバトル画面のマネージャーリスト。
    /// </summary>
    [SerializeField]
    private List<BattleControllableMonoBehavior> m_BattleMainManagers;

    /// <summary>
    /// コマンドイベント画面のマネージャーリスト。
    /// </summary>
    [SerializeField]
    private List<BattleControllableMonoBehavior> m_BattleCommandManagers;

    [SerializeField]
    private E_BATTLE_STATUS m_InitMode;

    [Header("Game Progress")]

    [SerializeField]
    private E_BATTLE_STATUS m_BattleStatus;

    [SerializeField]
    public bool m_PlayerNotDead;

    #endregion

    #region Field

    /// <summary>
    /// 状態遷移のリクエストを受けたかどうか。
    /// </summary>
    private bool m_IsRequestedTransition;

    /// <summary>
    /// リクエストされた遷移先状態。
    /// </summary>
    private E_BATTLE_STATUS m_RequestedBattleStatus;

    /// <summary>
    /// 全てのマネージャが初期化された後のコールバック
    /// </summary>
    public Action m_OnInitManagers;

    /// <summary>
    /// 全てのマネージャがスタートした後のコールバック
    /// </summary>
    public Action m_OnStartManagers;

    #endregion

    #region Get & Set

    /// <summary>
    /// メインのバトル画面のマネージャーリストを取得する。
    /// </summary>
    public List<BattleControllableMonoBehavior> GetBattleMainManegers()
    {
        return m_BattleMainManagers;
    }

    /// <summary>
    /// コマンドイベント画面のマネージャーリストを取得する。
    /// </summary>
    public List<BattleControllableMonoBehavior> GetBattleCommandManegers()
    {
        return m_BattleCommandManagers;
    }

    #endregion

    /// <summary>
    /// シーン読み込み時に呼び出される。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();

        EchoBulletIndexGenerater.OnInitialize();
        m_BattleMainManagers.ForEach(m => m.OnInitialize());
        m_BattleCommandManagers.ForEach(m => m.OnInitialize());

        EventUtility.SafeInvokeAction(m_OnInitManagers);
        m_OnInitManagers = null;
    }

    /// <summary>
    /// シーン破棄時に呼び出される。
    /// </summary>
	public override void OnFinalize()
    {
        base.OnFinalize();

        m_BattleMainManagers.ForEach(m => m.OnFinalize());
        m_BattleCommandManagers.ForEach(m => m.OnFinalize());
        EchoBulletIndexGenerater.OnFinalize();
    }

    /// <summary>
    /// シーン読み込み時の一度だけ呼び出される。
    /// </summary>
	public override void OnStart()
    {
        base.OnStart();

        m_BattleMainManagers.ForEach(m => m.OnStart());
        m_BattleCommandManagers.ForEach(m => m.OnStart());

        EventUtility.SafeInvokeAction(m_OnStartManagers);
        m_OnStartManagers = null;

        if (m_InitMode == E_BATTLE_STATUS.MAIN || m_InitMode == E_BATTLE_STATUS.TRANSITION_MAIN)
        {
            TransitionForceBattleMain();
        }
        else
        {
            TransitionForceBattleCommand();
        }
    }

    /// <summary>
    /// シーン読み込み後、毎フレーム呼び出される。
    /// </summary>
	public override void OnUpdate()
    {
        base.OnUpdate();

        switch (m_BattleStatus)
        {
            case E_BATTLE_STATUS.MAIN:
                m_BattleMainManagers.ForEach(m => m.OnUpdate());
                break;
            case E_BATTLE_STATUS.COMMAND:
                m_BattleCommandManagers.ForEach(m => m.OnUpdate());
                break;
        }
    }

    /// <summary>
    /// シーン読み込み後、OnUpdateの後に毎フレーム呼び出される。
    /// </summary>
	public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        switch (m_BattleStatus)
        {
            case E_BATTLE_STATUS.MAIN:
                m_BattleMainManagers.ForEach(m => m.OnLateUpdate());
                break;
            case E_BATTLE_STATUS.COMMAND:
                m_BattleCommandManagers.ForEach(m => m.OnLateUpdate());
                break;
        }

        if (m_IsRequestedTransition)
        {
            if (m_RequestedBattleStatus == E_BATTLE_STATUS.MAIN)
            {
                ProcessTransitionBattleMain();
            }
            else if (m_RequestedBattleStatus == E_BATTLE_STATUS.COMMAND)
            {
                ProcessTransitionBattleCommand();
            }

            m_IsRequestedTransition = false;
        }
    }

    /// <summary>
    /// シーン読み込み後、固定間隔で呼び出される。
    /// </summary>
	public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        switch (m_BattleStatus)
        {
            case E_BATTLE_STATUS.MAIN:
                m_BattleMainManagers.ForEach(m => m.OnFixedUpdate());
                break;
            case E_BATTLE_STATUS.COMMAND:
                m_BattleCommandManagers.ForEach(m => m.OnFixedUpdate());
                break;
        }
    }

    /// <summary>
    /// ゲームを開始する。
    /// </summary>
    public void GameStart()
    {

    }

    /// <summary>
    /// ゲームオーバーにする。
    /// </summary>
	public void GameOver()
    {
        DetachBattleMainInputAction();
        BattleMainUiManager.Instance.ShowGameOver();
        BattleMainAudioManager.Instance.StopAllBGM();
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        {
            BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        });
        TimerManager.Instance.RegistTimer(timer);
    }

    /// <summary>
    /// ゲームクリアにする。
    /// </summary>
	public void GameClear()
    {
        DetachBattleMainInputAction();

        BattleMainUiManager.Instance.ShowGameClear();
        var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, 1, () =>
        {
            BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
        });
        TimerManager.Instance.RegistTimer(timer);
    }

    /// <summary>
    /// BattleCommandからBattleMainへと遷移する。
    /// </summary>
    public void TransitionBattleMain()
    {
        if (m_BattleStatus != E_BATTLE_STATUS.COMMAND || m_IsRequestedTransition)
        {
            return;
        }

        TransitionForceBattleMain();
    }

    /// <summary>
    /// BattleMainからBattleCommandへと遷移する。
    /// </summary>
    public void TransitionBattleCommand()
    {
        if (m_BattleStatus != E_BATTLE_STATUS.MAIN || m_IsRequestedTransition)
        {
            return;
        }

        TransitionForceBattleCommand();
    }

    /// <summary>
    /// 強制的にBattleMainへと遷移する。
    /// </summary>
    private void TransitionForceBattleMain()
    {
        m_IsRequestedTransition = true;
        m_RequestedBattleStatus = E_BATTLE_STATUS.MAIN;
    }

    /// <summary>
    /// 強制的にBattleCommandへと遷移する。
    /// </summary>
    private void TransitionForceBattleCommand()
    {
        m_IsRequestedTransition = true;
        m_RequestedBattleStatus = E_BATTLE_STATUS.COMMAND;
    }

    /// <summary>
    /// 実際にBattleMainに状態遷移する処理を行う。
    /// </summary>
    private void ProcessTransitionBattleMain()
    {
        m_BattleStatus = E_BATTLE_STATUS.TRANSITION_MAIN;

        DetachBattleCommandInputAction();
        m_BattleCommandManagers.ForEach(m => m.OnDisableObject());
        m_BattleMainManagers.ForEach(m => m.OnEnableObject());
        AttachBattleMainInputAction();

        m_BattleStatus = E_BATTLE_STATUS.MAIN;
    }

    /// <summary>
    /// 実際にBattleCommandに状態遷移する処理を行う。
    /// </summary>
    private void ProcessTransitionBattleCommand()
    {
        m_BattleStatus = E_BATTLE_STATUS.TRANSITION_COMMAND;

        DetachBattleMainInputAction();
        m_BattleMainManagers.ForEach(m => m.OnDisableObject());
        m_BattleCommandManagers.ForEach(m => m.OnEnableObject());
        AttachBattleCommandInputAction();

        m_BattleStatus = E_BATTLE_STATUS.COMMAND;
    }

    private void TransitionMode(InputManager.E_INPUT_STATE state)
    {
        if (state != InputManager.E_INPUT_STATE.DOWN)
        {
            return;
        }

        DetachBattleMainInputAction();
        DetachBattleCommandInputAction();
        BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.STAGE1);
    }

    /// <summary>
    /// Main画面で必要な入力アクションを付与する。
    /// </summary>
    private void AttachBattleMainInputAction()
    {
        try
        {
            InputManager.Instance.HorizontalAction += PlayerCharaManager.Instance.OnInputHorizontal;
            InputManager.Instance.VerticalAction += PlayerCharaManager.Instance.OnInputVertical;
            InputManager.Instance.ChangeCharaAction += PlayerCharaManager.Instance.OnInputChangeChara;
            InputManager.Instance.ShotAction += PlayerCharaManager.Instance.OnInputShot;
            InputManager.Instance.BombAction += PlayerCharaManager.Instance.OnInputBomb;
            InputManager.Instance.MenuAction += TransitionMode;
            //InputManager.Instance.MenuAction += ItemManager.Instance.OnAttractAction;
            //InputManager.Instance.MenuAction += PlayerCharaManager.Instance.OnInputMenu;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Main画面で必要な入力アクションを外す。
    /// </summary>
    private void DetachBattleMainInputAction()
    {
        // 何も無い時にイベントを減算すると例外が発生するので適当に例外処理を挟む
        try
        {
            InputManager.Instance.HorizontalAction -= PlayerCharaManager.Instance.OnInputHorizontal;
            InputManager.Instance.VerticalAction -= PlayerCharaManager.Instance.OnInputVertical;
            InputManager.Instance.ChangeCharaAction -= PlayerCharaManager.Instance.OnInputChangeChara;
            InputManager.Instance.ShotAction -= PlayerCharaManager.Instance.OnInputShot;
            InputManager.Instance.BombAction -= PlayerCharaManager.Instance.OnInputBomb;
            InputManager.Instance.MenuAction -= TransitionMode;
            //InputManager.Instance.MenuAction -= ItemManager.Instance.OnAttractAction;
            //InputManager.Instance.MenuAction -= PlayerCharaManager.Instance.OnInputMenu;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Command画面で必要な入力アクションを付与する。
    /// </summary>
    private void AttachBattleCommandInputAction()
    {
        try
        {
            InputManager.Instance.HorizontalAction += CommandPlayerCharaManager.Instance.OnInputHorizontal;
            InputManager.Instance.VerticalAction += CommandPlayerCharaManager.Instance.OnInputVertical;
            InputManager.Instance.ShotAction += CommandPlayerCharaManager.Instance.OnInputShot;
            InputManager.Instance.MenuAction += TransitionMode;
            //InputManager.Instance.MenuAction += PlayerCharaManager.Instance.OnInputMenu;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    /// <summary>
    /// Command画面で必要な入力アクションを外す。
    /// </summary>
    private void DetachBattleCommandInputAction()
    {
        // 何も無い時にイベントを減算すると例外が発生するので適当に例外処理を挟む
        try
        {
            InputManager.Instance.HorizontalAction -= CommandPlayerCharaManager.Instance.OnInputHorizontal;
            InputManager.Instance.VerticalAction -= CommandPlayerCharaManager.Instance.OnInputVertical;
            InputManager.Instance.ShotAction -= CommandPlayerCharaManager.Instance.OnInputShot;
            InputManager.Instance.MenuAction -= TransitionMode;
            //InputManager.Instance.MenuAction -= PlayerCharaManager.Instance.OnInputMenu;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
