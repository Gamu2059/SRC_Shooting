using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BattleMainのUIを管理する。
/// </summary>
public class BattleMainUiManager : SingletonMonoBehavior<BattleMainUiManager>
{
	[SerializeField]
	private GameObject m_GameOverGroup;

	[SerializeField]
	private GameObject m_GameClearGroup;

	[SerializeField]
	private Text m_ScoreText;

	public override void OnInitialize()
	{
		base.OnInitialize();

		HideGameClear();
		HideGameOver();
	}

	public void ShowGameOver()
	{
		m_GameOverGroup.SetActive( true );
	}

	public void ShowGameClear()
	{
		m_GameClearGroup.SetActive( true );
	}

	public void HideGameOver()
	{
		m_GameOverGroup.SetActive( false );
	}

	public void HideGameClear()
	{
		m_GameClearGroup.SetActive( false );
	}
}
