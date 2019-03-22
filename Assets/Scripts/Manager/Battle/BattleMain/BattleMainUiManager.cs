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

	public void ShowGameOver()
	{

	}

	public void ShowGameClear()
	{

	}
}
