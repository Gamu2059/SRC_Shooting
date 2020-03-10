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
    [Serializable]
    public struct FadeData
    {
        public bool IsFadeOut;
        public float Duration;
        public Color FadeOutColor;
    }

    [SerializeField]
    private FadeData m_FadeData;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new FadePlayableBehaviour();
        behaviour.FadeData = m_FadeData;
        return ScriptPlayable<FadePlayableBehaviour>.Create(graph, behaviour);
    }
}
