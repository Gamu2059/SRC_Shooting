using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

	public override void OnInitialize()
	{
		base.OnInitialize();

		m_BattleMainManagers.ForEach( m => m.OnInitialize() );
	}

	public override void OnFinalize()
	{
		base.OnFinalize();

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
}
