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
    public bool IsStart { get; private set; }

    /// <summary>
    /// カウント開始からの秒数
    /// </summary>
    private float m_Period;

    public EventTriggerTimePeriod()
    {
        IsStart = false;
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
        IsStart = true;
    }

    public void OnFixedUpdate()
    {
        if (!IsStart)
        {
            return;
        }

        m_Period += Time.fixedDeltaTime;
    }
}
