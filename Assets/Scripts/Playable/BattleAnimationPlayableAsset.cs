using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class BattleAnimationPlayableAsset : PlayableAsset
{

    [SerializeField]
    private ExposedReference<Transform> m_Target;

    [SerializeField]
    private List<BattleAnimationParam> m_Animations;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new BattleAnimationPlayableBehaviour();
        behaviour.SetArguments(m_Target.Resolve(graph.GetResolver()), m_Animations);
        return ScriptPlayable<BattleAnimationPlayableBehaviour>.Create(graph, behaviour);
    }
}
