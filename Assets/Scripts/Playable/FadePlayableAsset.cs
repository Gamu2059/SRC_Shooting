#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

/// <summary>
/// FadeManagerにパラメータを渡すためのアセット。
/// </summary>
[Serializable]
public class FadePlayableAsset : PlayableAsset
{
    [SerializeField]
    private FadeParam m_FadeParam;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new FadePlayableBehaviour();
        behaviour.FadeParam = m_FadeParam;
        return ScriptPlayable<FadePlayableBehaviour>.Create(graph, behaviour);
    }
}
