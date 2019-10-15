using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class BattleAnimationPlayableAsset : PlayableAsset
{
    public const string PLAYABLE_OBJECT = "PlayableObject";

    [SerializeField]
    private string m_ReferenceName;

    [SerializeField]
    private BattleAnimationParam m_AnimParam;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new BattleAnimationPlayableBehaviour();

        bool result;
        var target = graph.GetResolver().GetReferenceValue(m_ReferenceName, out result) as Transform;
        var playable = graph.GetResolver().GetReferenceValue(PLAYABLE_OBJECT, out result) as BattleRealPlayableBase;

        behaviour.SetArguments(target, m_AnimParam, playable);
        return ScriptPlayable<BattleAnimationPlayableBehaviour>.Create(graph, behaviour);
    }
}
