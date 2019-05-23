using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class BattleAnimationPlayableAsset : PlayableAsset
{

    [SerializeField]
    private ExposedReference<Transform> m_Target;

    [SerializeField]
    private BattleAnimationParam m_AnimParam;

    //private Transform m_Target;

    //public void SetTarget(Transform target)
    //{
    //    m_Target = target;
    //}

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behaviour = new BattleAnimationPlayableBehaviour();
        behaviour.SetArguments(m_Target.Resolve(graph.GetResolver()), m_AnimParam);
        //behaviour.SetArguments(m_Target, m_Animations);
        return ScriptPlayable<BattleAnimationPlayableBehaviour>.Create(graph, behaviour);
    }
}
