using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BlendShapeControlPlayableAsset : PlayableAsset
{
    [SerializeField]
    private ExposedReference<SkinnedMeshRenderer> m_SkinnedMeshRenderer;

    [SerializeField, Tooltip("操作対象のシェイプ名。完全一致である必要がある。")]
    private string m_ShapeName;

    [SerializeField, Tooltip("シェイプの制御値。時間軸の単位は秒。")]
    private AnimationCurve m_ControlShapeCurve;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var behavior = new BlendShapeControlPlayableBehaviour();
        behavior.SkinnedMeshRenderer = m_SkinnedMeshRenderer.Resolve(graph.GetResolver());
        behavior.ShapeName = m_ShapeName;
        behavior.ControlShapeCurve = m_ControlShapeCurve;
        return ScriptPlayable<BlendShapeControlPlayableBehaviour>.Create(graph, behavior);
    }
}
