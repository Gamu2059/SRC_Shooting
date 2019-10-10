﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リアルモードにおけるオブジェクトの基底クラス。
/// </summary>
public class BattleRealObjectBase : BattleObjectBase
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
        DestroyAllTimer();
        base.OnFinalize();
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
        BattleRealTimerManager.Instance.RegistTimer(timer);
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

    public override void OnRenderCollider()
    {
        if (BattleManager.Instance == null || BattleRealStageManager.Instance == null)
        {
            return;
        }

        DrawCollider();
    }

    protected override Material GetCollisionMaterial()
    {
        //if (BattleManager.Instance == null || BattleRealCollisionManager.Instance == null)
        //{
        //    return null;
        //}

        //return BattleRealCollisionManager.Instance.CollisionMaterial;
        return null;
    }

    protected override Vector2 CalcViewportPos(Vector2 worldPos)
    {
        if (BattleManager.Instance == null || BattleRealStageManager.Instance == null)
        {
            return Vector2.one / 2f;
        }

        return BattleRealStageManager.Instance.CalcViewportPosFromWorldPosition(worldPos.x, worldPos.y);
    }
}
