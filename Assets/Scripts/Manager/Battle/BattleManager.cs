using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトル画面のマネージャーを管理する上位マネージャ。
/// メイン画面とコマンド画面の切り替えを主に管理する。
/// </summary>
public class BattleManager : SingletonMonoBehavior<BattleManager>
{
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
}
