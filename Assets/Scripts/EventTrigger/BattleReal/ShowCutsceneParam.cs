#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// カットシーン呼び出しパラメータ
/// </summary>
[Serializable]
public struct ShowCutsceneParam
{
    [Tooltip("カットシーンの名前")]
    public string CutsceneName;

    [Tooltip("カットシーン終了時にGAMEステートに自動的に遷移させるか")]
    public bool AutoChangeToGameState;

    [Tooltip("カットシーン終了時に実行するイベント")]
    public BattleRealEventTriggerParam OnCompletedEvent;
}
