using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイマーを管理する。
/// </summary>
public class TimerManager : SingletonMonoBehavior<TimerManager>
{
	/// <summary>
	/// TimerManagerのタイマーサイクル。
	/// </summary>
	private Timer.E_TIMER_CYCLE m_TimerManagerCycle;

	/// <summary>
	/// このTimerManagerが保持する全てのTimerインスタンス。
	/// </summary>
	private LinkedList<Timer> m_TimerList;

	private LinkedList<Timer> m_GotoStopTimerList;

	/// <summary>
	/// TimerManagerのタイマーサイクルを取得する。
	/// </summary>
	public Timer.E_TIMER_CYCLE GetTimerManagerCycle()
	{
		return m_TimerManagerCycle;
	}



	public override void OnInitialize()
	{
		m_TimerList = new LinkedList<Timer>();
		m_GotoStopTimerList = new LinkedList<Timer>();
		m_TimerManagerCycle = Timer.E_TIMER_CYCLE.UPDATE;
	}

	/// <summary>
	/// 1秒間に FixedTimeStep * TimeScale 回呼び出される。
	/// </summary>
	public override void OnFixedUpdate()
	{
		if( m_TimerManagerCycle != Timer.E_TIMER_CYCLE.UPDATE )
		{
			return;
		}

		foreach( var timer in m_TimerList )
		{
			if( timer != null )
			{
				timer.OnFixedUpdate();
			}
		}

		RemoveStopTimers();
	}



	/// <summary>
	/// 停止したタイマーを削除する。
	/// </summary>
	private void RemoveStopTimers()
	{
		int count = m_GotoStopTimerList.Count;

		for( int i = 0; i < count; i++ )
		{
			var timer = m_GotoStopTimerList.First.Value;
			m_TimerList.Remove( timer );
			m_GotoStopTimerList.RemoveFirst();
		}
	}



	/// <summary>
	/// タイマーを登録する。
	/// </summary>
	public void RegistTimer( Timer timer )
	{
		if( timer == null )
		{
			return;
		}

		m_TimerList.AddLast( timer );
		timer.SetTimerCycle( Timer.E_TIMER_CYCLE.UPDATE );
		timer.SetTimerManager( this );
	}

	/// <summary>
	/// タイマーを削除する。
	/// </summary>
	public void RemoveTimer( Timer timer )
	{
		if( timer == null )
		{
			return;
		}

		if( timer.GetTimerCycle() != Timer.E_TIMER_CYCLE.UPDATE && timer.GetTimerCycle() != Timer.E_TIMER_CYCLE.PAUSE )
		{
			timer.SetTimerCycle( Timer.E_TIMER_CYCLE.STOP );
			EventUtility.SafeInvokeAction( timer.GetStopCallBack() );
		}

		m_GotoStopTimerList.AddLast( timer );
	}

	/// <summary>
	/// TimerManager全体で一時停止する。
	/// </summary>
	public void PauseTimerManager()
	{
		if( m_TimerManagerCycle != Timer.E_TIMER_CYCLE.UPDATE )
		{
			return;
		}

		m_TimerManagerCycle = Timer.E_TIMER_CYCLE.PAUSE;
	}

	/// <summary>
	/// TimerManager全体で一時停止を解除する。
	/// </summary>
	public void ResumeTimerManager()
	{
		if( m_TimerManagerCycle != Timer.E_TIMER_CYCLE.PAUSE )
		{
			return;
		}

		m_TimerManagerCycle = Timer.E_TIMER_CYCLE.UPDATE;
	}
}
