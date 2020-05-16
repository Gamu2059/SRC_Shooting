using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using Rewired;

public class PreTitleScene : BaseScene
{
	[SerializeField]
	private VideoPlayer m_VideoPlayer;

	private Player m_Player;
	private bool m_IsEnableUpdate;

	protected override void OnAwake()
	{
		base.OnAwake();
		m_IsEnableUpdate = false;
	}

	public override void OnBeforeShow(Action onComplete)
	{
		OnInitializeManagers();
		m_IsEnableUpdate = true;
		m_Player = ReInput.players.GetPlayer(0);
		base.OnBeforeShow(onComplete);
	}

	public override void OnAfterHide(Action onComplete)
	{
		OnFinalizeManagers();
		base.OnAfterHide(onComplete);
	}

	private void Update()
	{
		if (!m_IsEnableUpdate)
		{
			return;
		}

		if (m_Player != null && m_Player.GetAnyButtonDown())
		{
			m_IsEnableUpdate = false;
			BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
			return;
		}

		if (m_VideoPlayer.frame >= 0 && (ulong)m_VideoPlayer.frame >= m_VideoPlayer.frameCount - 1)
		{
			m_IsEnableUpdate = false;
			BaseSceneManager.Instance.LoadScene(BaseSceneManager.E_SCENE.TITLE);
			return;
		}
	}
}
