using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// AnimationTrackの代わりにAnimationCurveでTransformを制御する。
/// </summary>
public class BattleAnimationPlayableBehaviour : PlayableBehaviour
{
    public Transform m_CharaObject;
    private Vector3 m_InitPos;

    /// <summary>
    /// Timelineが始まった時に呼び出される。
    /// このトラックが始まる時とは異なる。
    /// </summary>
    public override void OnGraphStart(Playable playable)
    {
        
    }

    /// <summary>
    /// Timelineが終わった時に呼び出される。
    /// このトラックが終わる時とは異なる。
    /// </summary>
    public override void OnGraphStop(Playable playable)
    {
        
    }

    /// <summary>
    /// このトラックが始まる時に呼び出される。
    /// </summary>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        m_InitPos = m_CharaObject.localPosition;
    }

    /// <summary>
    /// このトラックが終わる時に呼び出される。
    /// </summary>
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    /// <summary>
    /// フレームの前段階で呼び出される。
    /// </summary>
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }

    /// <summary>
    /// 毎フレーム呼び出される。
    /// </summary>
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (m_CharaObject == null) return;
        var currentTime = (float)playable.GetTime() / (float)playable.GetDuration();
        var currentPos = Vector3.Lerp(Vector3.up, new Vector3(0,1,30), currentTime);
        m_CharaObject.localPosition = currentPos + m_InitPos;
    }
}
