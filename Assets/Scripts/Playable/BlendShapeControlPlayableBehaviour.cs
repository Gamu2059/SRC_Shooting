using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// ブレンドシェイプを操作するためのコントローラ。
/// </summary>
public class BlendShapeControlPlayableBehaviour : PlayableBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public string ShapeName;
    public AnimationCurve ControlShapeCurve;

    private bool m_IsInvalid;
    private int m_ShapeIndex;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        m_IsInvalid = false;

        if (ControlShapeCurve == null || SkinnedMeshRenderer == null)
        {
            m_IsInvalid = true;
            return;
        }

        m_ShapeIndex = SkinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(ShapeName);
        if (m_ShapeIndex < 0)
        {
            m_IsInvalid = true;
            Debug.LogWarningFormat("{0} : shape name is invalid. skin name : {1}, shape name : {2}", SkinnedMeshRenderer.name, ShapeName);
            return;
        }
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (m_IsInvalid || ControlShapeCurve == null || SkinnedMeshRenderer == null)
        {
            return;
        }

        var value = ControlShapeCurve.Evaluate((float)playable.GetTime());
        SkinnedMeshRenderer.SetBlendShapeWeight(m_ShapeIndex, value);
    }
}
