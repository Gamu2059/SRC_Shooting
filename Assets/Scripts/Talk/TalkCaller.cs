using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utage;

/// <summary>
/// 会話の呼び出しを制御する。
/// 1シナリオにつき、1インスタンスでの使用を想定。
/// </summary>
public class TalkCaller
{
    private Action m_OnStart;
    private Action m_OnEnd;

    public TalkCaller(string scenarioLabel, Action onStart = null, Action onEnd = null, Action onStartError = null)
    {
        if (AdvEngineManager.Instance == null || AdvEngineManager.Instance.AdvEngine == null)
        {
            onStartError?.Invoke();
            return;
        }

        m_OnStart = onStart;
        m_OnEnd = onEnd;

        var engine = AdvEngineManager.Instance.AdvEngine;
        var player = engine.ScenarioPlayer;
        player.OnBeginScenario.AddListener(OnBeginScenario);
        player.OnEndScenario.AddListener(OnEndScenario);

        engine.JumpScenario(scenarioLabel);
    }

    public void StopTalk()
    {
        if (AdvEngineManager.Instance == null || AdvEngineManager.Instance.AdvEngine == null)
        {
            return;
        }

        var player = AdvEngineManager.Instance.AdvEngine.ScenarioPlayer;
        player.OnBeginScenario.RemoveListener(OnBeginScenario);
        player.OnEndScenario.RemoveListener(OnEndScenario);
        player.EndScenario();
    }

    private void OnBeginScenario(AdvScenarioPlayer player)
    {
        player.OnBeginScenario.RemoveListener(OnBeginScenario);
        m_OnStart?.Invoke();
    }

    private void OnEndScenario(AdvScenarioPlayer player)
    {
        player.OnEndScenario.RemoveListener(OnEndScenario);
        m_OnEnd?.Invoke();
    }
}
