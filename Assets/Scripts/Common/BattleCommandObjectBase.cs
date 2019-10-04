using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleCommandにおけるオブジェクトの基底クラス。
/// </summary>
public class BattleCommandObjectBase : BattleObjectBase
{
    /// <summary>
	/// タイマーを保持するリスト。
	/// </summary>
	private Dictionary<string, Timer> m_TimerDict;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_TimerDict = new Dictionary<string, Timer>();
    }

    public override void OnFinalize()
    {
        base.OnFinalize();
        DestroyAllTimer();
    }

    /// <summary>
    /// 指定したタイマーのキーを保持しているかどうか。
    /// </summary>
    public bool ContainsTimerKey(string key)
    {
        if (m_TimerDict == null)
        {
            return false;
        }

        return m_TimerDict.ContainsKey(key);
    }

    /// <summary>
    /// タイマーを登録する。
    /// </summary>
    public void RegistTimer(string key, Timer timer)
    {
        if (m_TimerDict == null || ContainsTimerKey(key))
        {
            return;
        }

        m_TimerDict.Add(key, timer);
        //BattleHackingTimerManager.Instance.RegistTimer(timer);
    }

    /// <summary>
    /// タイマーを取得する。
    /// </summary>
    public Timer GetTimer(string key)
    {
        if (m_TimerDict == null || !ContainsTimerKey(key))
        {
            return null;
        }

        return m_TimerDict[key];
    }

    /// <summary>
    /// タイマーを完全破棄する。
    /// </summary>
    public void DestroyTimer(string key)
    {
        if (m_TimerDict == null || !ContainsTimerKey(key))
        {
            return;
        }

        var timer = m_TimerDict[key];
        m_TimerDict.Remove(key);

        if (timer != null)
        {
            timer.DestroyTimer();
        }
    }

    /// <summary>
    /// 全ての登録されているタイマーを完全破棄する。
    /// </summary>
    public void DestroyAllTimer()
    {
        if (m_TimerDict == null)
        {
            return;
        }

        foreach (var timer in m_TimerDict.Values)
        {
            timer.DestroyTimer();
        }

        m_TimerDict.Clear();
    }
}
