using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BattleAnimationPlayableAsset : PlayableAsset
{
    public ExposedReference<Transform> m_TargetObject;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new BattleAnimationPlayableBehaviour();
        behaviour.m_CharaObject = go.transform;
        return ScriptPlayable<BattleAnimationPlayableBehaviour>.Create(graph, behaviour);
    }
}
