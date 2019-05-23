using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントのタイムピリオド。
/// </summary>
public class EventTriggerTimePeriod
{
    /// <summary>
    /// タイムピリオドのカウントが開始しているかどうか
    /// </summary>
    private bool m_IsStart;

    /// <summary>
    /// カウント開始からの秒数
    /// </summary>
    private float m_Period;

    public EventTriggerTimePeriod()
    {
        m_IsStart = false;
    }

    /// <summary>
    /// カウントを開始しているかを取得する。
    /// </summary>
    public bool IsStart()
    {
        return m_IsStart;
    }

    /// <summary>
    /// カウント開始からの秒数を取得する。
    /// </summary>
    public float GetPeriod()
    {
        return m_Period;
    }

    /// <summary>
    /// カウントを開始する。
    /// </summary>
    public void CountStart()
    {
        m_IsStart = true;
    }

    public void OnFixedUpdate()
    {
        if (!m_IsStart)
        {
            return;
        }

        m_Period += Time.fixedDeltaTime;
    }
}
