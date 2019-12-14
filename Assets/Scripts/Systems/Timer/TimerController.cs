using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerController
{
    private LinkedList<Timer> m_StandbyTimers;

	private LinkedList<Timer> m_Timers;

	private LinkedList<Timer> m_GotoStopTimers;

	/// <summary>
	/// TimerManagerのタイマーサイクル。
	/// </summary>
	private E_TIMER_CYCLE m_TimerCycle;

    public void OnInitialize()
    {
        m_StandbyTimers = new LinkedList<Timer>();
		m_Timers = new LinkedList<Timer>();
		m_GotoStopTimers = new LinkedList<Timer>();
		m_TimerCycle = E_TIMER_CYCLE.UPDATE;
    }

    public void OnFinalize()
    {
        foreach (var timer in m_Timers)
        {
            RemoveTimer(timer);
        }

        RemoveStopTimers();

        m_GotoStopTimers.Clear();
        m_Timers.Clear();
        m_StandbyTimers.Clear();
        m_GotoStopTimers = null;
        m_Timers = null;
        m_StandbyTimers = null;
    }

	public void OnUpdate()
	{
		if( m_TimerCycle != E_TIMER_CYCLE.UPDATE )
		{
			return;
		}

		foreach( var timer in m_Timers )
		{
			if( timer != null )
			{
				timer.OnUpdate();
			}
		}

		RemoveStopTimers();
        RegisterStandbyTimers();
	}

	private void RemoveStopTimers()
	{
        foreach (var timer in m_GotoStopTimers)
        {
			m_Timers.Remove( timer );
        }

        m_GotoStopTimers.Clear();
	}

    private void RegisterStandbyTimers()
    {
        foreach (var timer in m_StandbyTimers)
        {
            m_Timers.AddLast(timer);
        }

        m_StandbyTimers.Clear();
    }

    /// <summary>
    /// タイマーを登録する。
    /// </summary>
    public void RegistTimer( Timer timer )
	{
		if( timer == null || m_StandbyTimers.Contains(timer))
		{
			return;
		}

        m_StandbyTimers.AddLast(timer);
		timer.SetTimerCycle( E_TIMER_CYCLE.UPDATE );
		timer.SetTimerController( this );
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

		if( timer.GetTimerCycle() != E_TIMER_CYCLE.UPDATE && timer.GetTimerCycle() != E_TIMER_CYCLE.PAUSE )
		{
			timer.SetTimerCycle( E_TIMER_CYCLE.STOP );
            timer.GetStopCallBack()?.Invoke();
		}

		m_GotoStopTimers.AddLast( timer );
	}

	/// <summary>
	/// TimerManager全体で一時停止する。
	/// </summary>
	public void PauseTimerManager()
	{
		if( m_TimerCycle != E_TIMER_CYCLE.UPDATE )
		{
			return;
		}

		m_TimerCycle = E_TIMER_CYCLE.PAUSE;
	}

	/// <summary>
	/// TimerManager全体で一時停止を解除する。
	/// </summary>
	public void ResumeTimerManager()
	{
		if( m_TimerCycle != E_TIMER_CYCLE.PAUSE )
		{
			return;
		}

		m_TimerCycle = E_TIMER_CYCLE.UPDATE;
	}
}
