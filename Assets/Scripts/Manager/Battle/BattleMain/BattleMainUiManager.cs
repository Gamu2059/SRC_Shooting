using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;

/// <summary>
/// BattleMainのUIを管理する。
/// </summary>
public class BattleMainUiManager : BattleSingletonMonoBehavior<BattleMainUiManager>
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

        SubscribeUiEvent();
	}

    private void SubscribeUiEvent()
    {
        BattleManager.Instance.GetScorePoperty().Subscribe((x)=>UpdateScore(x));
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

    public void UpdateScore(int score)
    {
        if (m_ScoreText == null)
        {
            return;
        }

        m_ScoreText.text = string.Format("Score : {0}", score);
    }
}
