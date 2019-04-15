using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTimerManagerBase<T> : BattleSingletonMonoBehavior<T> where T : BattleTimerManagerBase<T>
{

    protected TimerController m_TimerController;

    public override void OnInitialize()
    {
        m_TimerController = new TimerController();
    }

    /// <summary>
    /// 1秒間に FixedTimeStep * TimeScale 回呼び出される。
    /// </summary>
    public override void OnFixedUpdate()
    {
        m_TimerController.OnFixedUpdate();
    }

    /// <summary>
    /// タイマーを登録する。
    /// </summary>
    public void RegistTimer(Timer timer)
    {
        m_TimerController.RegistTimer(timer);
    }

    /// <summary>
    /// タイマーを削除する。
    /// </summary>
    public void RemoveTimer(Timer timer)
    {
        m_TimerController.RemoveTimer(timer);
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
