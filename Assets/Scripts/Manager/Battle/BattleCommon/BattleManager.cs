using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
	public enum E_BATTLE_MODE
	{
		MAIN,
		COMMAND,
	}

	/// <summary>
	/// メインのバトル画面のマネージャーリスト。
	/// </summary>
	[SerializeField]
	private List<ControllableMonoBehaviour> m_BattleMainManagers;

	/// <summary>
	/// コマンドイベント画面のマネージャーリスト。
	/// </summary>
	[SerializeField]
	private List<ControllableMonoBehaviour> m_BattleCommandManagers;

	[Header( "Game Progress" )]

	[SerializeField]
	private IntReactiveProperty m_Score;

	[SerializeField]
	private IntReactiveProperty m_BestScore;

	[SerializeField]
	private BoolReactiveProperty m_IsBestScore;

	[SerializeField]
	public bool m_PlayerNotDead;

	/// <summary>
	/// メインのバトル画面のマネージャーリストを取得する。
	/// </summary>
	public List<ControllableMonoBehaviour> GetBattleMainManegers()
	{
		return m_BattleMainManagers;
	}

	/// <summary>
	/// コマンドイベント画面のマネージャーリストを取得する。
	/// </summary>
	public List<ControllableMonoBehaviour> GetBattleCommandManegers()
	{
		return m_BattleCommandManagers;
	}


	public void SubscribeScore( System.IObserver<int> action )
	{
		if( m_Score == null )
		{
			return;
		}

		m_Score.Subscribe( action );
	}

	public void SubscribeBestScore( System.IObserver<int> action )
	{
		if( m_BestScore == null )
		{
			return;
		}

		m_BestScore.Subscribe( action );
	}

	public void SubscribeIsBestScore( System.IObserver<bool> action )
	{
		if( m_IsBestScore == null )
		{
			return;
		}

		m_IsBestScore.Subscribe( action );
	}


	public override void OnInitialize()
	{
		base.OnInitialize();

		m_BattleMainManagers.ForEach( m => m.OnInitialize() );
        AttachInputAction();
	}

	public override void OnFinalize()
	{
		base.OnFinalize();

        DetachInputAction();
		m_BattleMainManagers.ForEach( m => m.OnFinalize() );
	}

	public override void OnStart()
	{
		base.OnStart();

		m_BattleMainManagers.ForEach( m => m.OnStart() );
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		m_BattleMainManagers.ForEach( m => m.OnUpdate() );
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();

		m_BattleMainManagers.ForEach( m => m.OnLateUpdate() );
	}

	public override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		m_BattleMainManagers.ForEach( m => m.OnFixedUpdate() );

    }

    /// <summary>
    /// バトル画面で必要な入力アクションを付与する。
    /// </summary>
    private void AttachInputAction()
    {
        InputManager.Instance.HorizontalAction += PlayerCharaManager.Instance.OnInputHorizontal;
        InputManager.Instance.VerticalAction += PlayerCharaManager.Instance.OnInputVertical;
        InputManager.Instance.ChangeCharaAction += PlayerCharaManager.Instance.OnInputChangeChara;
        InputManager.Instance.ShotAction += PlayerCharaManager.Instance.OnInputShot;
        InputManager.Instance.BombAction += PlayerCharaManager.Instance.OnInputBomb;
        //InputManager.Instance.MenuAction += PlayerCharaManager.Instance.OnInputMenu;
    }

    /// <summary>
    /// バトル画面で必要な入力アクションを外す。
    /// </summary>
    private void DetachInputAction()
    {
        InputManager.Instance.HorizontalAction -= PlayerCharaManager.Instance.OnInputHorizontal;
        InputManager.Instance.VerticalAction -= PlayerCharaManager.Instance.OnInputVertical;
        InputManager.Instance.ChangeCharaAction -= PlayerCharaManager.Instance.OnInputChangeChara;
        InputManager.Instance.ShotAction -= PlayerCharaManager.Instance.OnInputShot;
        InputManager.Instance.BombAction -= PlayerCharaManager.Instance.OnInputBomb;
        //InputManager.Instance.MenuAction -= PlayerCharaManager.Instance.OnInputMenu;
    }

	public void GameOver()
	{
		BattleMainUiManager.Instance.ShowGameOver();
		BattleMainAudioManager.Instance.StopAllBGM();
		var timer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, 1, () =>
		{
			BaseSceneManager.Instance.LoadScene( BaseSceneManager.E_SCENE.TITLE );
		} );
		TimerManager.Instance.RegistTimer( timer );
	}

	public void GameClear()
	{
		if( m_IsBestScore.Value )
		{
			PlayerPrefs.SetInt( "BestScore", m_BestScore.Value );
			PlayerPrefs.Save();
		}

		BattleMainUiManager.Instance.ShowGameClear();
		var timer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, 1, () =>
		{
			BaseSceneManager.Instance.LoadScene( BaseSceneManager.E_SCENE.TITLE );
		} );
		TimerManager.Instance.RegistTimer( timer );
	}

	public void AddScore( int score )
	{
		m_Score.Value += score;

		if( m_Score.Value > m_BestScore.Value )
		{
			if( !m_IsBestScore.Value )
			{
				m_IsBestScore.Value = true;
			}

			m_BestScore.Value = m_Score.Value;
		}
	}


	private void InitReactiveProperty()
	{
		m_BestScore = new IntReactiveProperty( 0 );
		m_Score = new IntReactiveProperty( 0 );
		m_IsBestScore = new BoolReactiveProperty( false );
	}

	private void InitScore()
	{
		m_Score.Value = 0;
		m_BestScore.Value = PlayerPrefs.GetInt( "BestScore", 0 );
	}
}
