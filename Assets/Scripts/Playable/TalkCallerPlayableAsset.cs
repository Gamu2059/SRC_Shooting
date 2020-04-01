#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

/// <summary>
/// AdvEngineManagerにパラメータを渡すためのアセット。
/// </summary>
[Serializable]
public class TalkCallerPlayableAsset : PlayableAsset
{
    [SerializeField]
    private string m_ScenarioLabel;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behavior = new TalkCallerPlayableBehavior();
        behavior.ScenarioLabel = m_ScenarioLabel;
        behavior.PlayableDirectorObject = go;
        
        return ScriptPlayable<TalkCallerPlayableBehavior>.Create(graph, behavior);
    }
}
