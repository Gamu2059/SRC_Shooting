﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイマーを管理する。
/// </summary>
public abstract class TimerManagerBase<T> : Singleton<T> where T : TimerManagerBase<T>, new()
{
	protected TimerController m_TimerController;

	public override void OnInitialize()
	{
        base.OnInitialize();
		m_TimerController = new TimerController();
        m_TimerController.OnInitialize();
	}

    public override void OnFinalize()
    {
        m_TimerController.OnFinalize();
        m_TimerController = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
		m_TimerController.OnUpdate();
    }

	/// <summary>
	/// タイマーを登録する。
	/// </summary>
	public void RegistTimer( Timer timer )
	{
		m_TimerController.RegistTimer( timer );
	}

	/// <summary>
	/// タイマーを削除する。
	/// </summary>
	public void RemoveTimer( Timer timer )
	{
		m_TimerController.RemoveTimer( timer );
	}

	/// <summary>
	/// TimerManager全体で一時停止する。
	/// </summary>
	public void PauseTimerManager()
	{
		m_TimerController.PauseTimerManager();
	}

	/// <summary>
	/// TimerManager全体で一時停止を解除する。
	/// </summary>
	public void ResumeTimerManager()
	{
		m_TimerController.ResumeTimerManager();
	}
}
