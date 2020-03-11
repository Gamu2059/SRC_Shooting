#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 会話の表示パラメータ
/// </summary>
[Serializable]
public struct ShowTalkParam
{
    [Tooltip("会話のシナリオラベル")]
    public string ScenarioLabel;

    [Tooltip("会話終了時にGAMEステートに自動的に遷移させるか")]
    public bool AutoChangeToGameState;

    [Tooltip("会話終了時に実行するイベント")]
    public BattleRealEventTriggerParam[] OnCompletedEvents;
}
