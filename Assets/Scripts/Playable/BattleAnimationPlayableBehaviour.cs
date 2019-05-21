using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// AnimationTrackの代わりにAnimationCurveでTransformを制御する。
/// </summary>
public class BattleAnimationPlayableBehaviour : PlayableBehaviour
{
    /// <summary>
    /// アニメーションのターゲット
    /// </summary>
    [SerializeField]
    private Transform m_AnimationTarget;

    /// <summary>
    /// アニメーション配列
    /// </summary>
    [SerializeField]
    private List<BattleAnimationParam> m_Animations;

    private float m_PreTime;

    private Vector3 m_InitPosition;
    private Vector3 m_InitRotation;
    private Vector3 m_InitLocalPosition;
    private Vector3 m_InitLocalRotation;
    private Vector3 m_InitLocalScale;

    public void SetArguments(Transform target, List<BattleAnimationParam> animations)
    {
        m_AnimationTarget = target;
        m_Animations = animations;
    }

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
        if (m_AnimationTarget == null || m_Animations == null)
        {
            return;
        }

        m_InitPosition = m_AnimationTarget.position;
        m_InitRotation = m_AnimationTarget.eulerAngles;
        m_InitLocalPosition = m_AnimationTarget.localPosition;
        m_InitLocalRotation = m_AnimationTarget.localEulerAngles;
        m_InitLocalScale = m_AnimationTarget.localScale;

        m_PreTime = 0;
    }

    /// <summary>
    /// このトラックが終わる時に呼び出される。
    /// このトラックがTimelineのスタートよりも後に配置されていると、最初にも呼び出さる。
    /// </summary>
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (m_AnimationTarget == null || m_Animations == null || playable.GetPlayState() != PlayState.Playing)
        {
            return;
        }

        for (int i = 0; i < m_Animations.Count; i++)
        {
            var anim = m_Animations[i];
            if (!anim.UseEndValue)
            {
                continue;
            }

            var value = anim.EndValue;

            if (anim.RelativeType == E_RELATIVE.RELATIVE)
            {
                value += GetInitTargetValue(ref anim);
            }

            Animation(ref anim, value, true);
        }
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
        if (m_AnimationTarget == null || m_Animations == null)
        {
            return;
        }

        var currentTime = (float)playable.GetTime();

        for (int i = 0; i < m_Animations.Count; i++)
        {
            var anim = m_Animations[i];
            float value = anim.Animation.Evaluate(currentTime);

            if (anim.RelativeType == E_RELATIVE.RELATIVE)
            {
                value -= anim.Animation.Evaluate(m_PreTime);
            }

            Animation(ref anim, value, false);
        }

        m_PreTime = currentTime;
    }

    private void Animation(ref BattleAnimationParam anim, float value, bool isForceSet)
    {
        var animType = anim.AnimationType;

        if (IsPositionAnimation(animType) || IsLocalPositionAnimation(animType))
        {
            AnimationPosition(ref anim, value, isForceSet);
        }
        else if (IsRotationAnimation(animType) || IsLocalRotationAnimation(animType))
        {
            AnimationRotation(ref anim, value, isForceSet);
        }
        else
        {
            AnimationScale(ref anim, value, isForceSet);
        }
    }

    private void AnimationPosition(ref BattleAnimationParam anim, float value, bool isForceSet)
    {
        Vector3 origin;

        if (IsPositionAnimation(anim.AnimationType))
            origin = m_AnimationTarget.position;
        else if (IsLocalPositionAnimation(anim.AnimationType))
            origin = m_AnimationTarget.localPosition;
        else
            return;

        var target = GetAnimVector(ref anim, value, origin, isForceSet);

        if (IsPositionAnimation(anim.AnimationType))
            m_AnimationTarget.position = target;
        else if (IsLocalPositionAnimation(anim.AnimationType))
            m_AnimationTarget.localPosition = target;
    }

    private void AnimationRotation(ref BattleAnimationParam anim, float value, bool isForceSet)
    {
        Vector3 origin;

        if (IsRotationAnimation(anim.AnimationType))
            origin = m_AnimationTarget.eulerAngles;
        else if (IsLocalRotationAnimation(anim.AnimationType))
            origin = m_AnimationTarget.localEulerAngles;
        else
            return;

        var target = GetAnimVector(ref anim, value, origin, isForceSet);

        if (IsRotationAnimation(anim.AnimationType))
            m_AnimationTarget.eulerAngles = target;
        else if (IsLocalRotationAnimation(anim.AnimationType))
            m_AnimationTarget.localEulerAngles = target;
    }

    private void AnimationScale(ref BattleAnimationParam anim, float value, bool isForceSet)
    {
        Vector3 origin;

        if (IsLocalScaleAnimation(anim.AnimationType))
            origin = m_AnimationTarget.localScale;
        else
            return;

        var target = GetAnimVector(ref anim, value, origin, isForceSet);

        m_AnimationTarget.localScale = target;
    }

    private Vector3 GetAnimVector(ref BattleAnimationParam anim, float value, Vector3 origin, bool isForceSet)
    {
        var target = origin;

        switch ((int)anim.AnimationType % 3)
        {
            case 0:
                target.x = value;
                if (anim.RelativeType == E_RELATIVE.RELATIVE && !isForceSet)
                    target.x += origin.x;
                break;
            case 1:
                target.y = value;
                if (anim.RelativeType == E_RELATIVE.RELATIVE && !isForceSet)
                    target.y += origin.y;
                break;
            case 2:
                target.z = value;
                if (anim.RelativeType == E_RELATIVE.RELATIVE && !isForceSet)
                    target.z += origin.z;
                break;
        }

        return target;
    }

    private bool IsPositionAnimation(BattleAnimationParam.E_ANIMATION_TYPE type)
    {
        switch (type)
        {
            case BattleAnimationParam.E_ANIMATION_TYPE.POSITION_X:
            case BattleAnimationParam.E_ANIMATION_TYPE.POSITION_Y:
            case BattleAnimationParam.E_ANIMATION_TYPE.POSITION_Z:
                return true;
            default:
                return false;
        }
    }

    private bool IsRotationAnimation(BattleAnimationParam.E_ANIMATION_TYPE type)
    {
        switch (type)
        {
            case BattleAnimationParam.E_ANIMATION_TYPE.ROTATION_X:
            case BattleAnimationParam.E_ANIMATION_TYPE.ROTATION_Y:
            case BattleAnimationParam.E_ANIMATION_TYPE.ROTATION_Z:
                return true;
            default:
                return false;
        }
    }

    private bool IsLocalPositionAnimation(BattleAnimationParam.E_ANIMATION_TYPE type)
    {
        switch (type)
        {
            case BattleAnimationParam.E_ANIMATION_TYPE.L_POSITION_X:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_POSITION_Y:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_POSITION_Z:
                return true;
            default:
                return false;
        }
    }

    private bool IsLocalRotationAnimation(BattleAnimationParam.E_ANIMATION_TYPE type)
    {
        switch (type)
        {
            case BattleAnimationParam.E_ANIMATION_TYPE.L_ROTATION_X:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_ROTATION_Y:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_ROTATION_Z:
                return true;
            default:
                return false;
        }
    }

    private bool IsLocalScaleAnimation(BattleAnimationParam.E_ANIMATION_TYPE type)
    {
        switch (type)
        {
            case BattleAnimationParam.E_ANIMATION_TYPE.L_SCALE_X:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_SCALE_Y:
            case BattleAnimationParam.E_ANIMATION_TYPE.L_SCALE_Z:
                return true;
            default:
                return false;
        }
    }

    private float GetInitTargetValue(ref BattleAnimationParam anim)
    {
        var type = anim.AnimationType;
        Vector3 origin = Vector3.zero;

        switch (((int)type / 3) * 3)
        {
            case (int)BattleAnimationParam.E_ANIMATION_TYPE.POSITION_X:
                origin = m_InitPosition;
                break;
            case (int)BattleAnimationParam.E_ANIMATION_TYPE.ROTATION_X:
                origin = m_InitRotation;
                break;
            case (int)BattleAnimationParam.E_ANIMATION_TYPE.L_POSITION_X:
                origin = m_InitLocalPosition;
                break;
            case (int)BattleAnimationParam.E_ANIMATION_TYPE.L_ROTATION_X:
                origin = m_InitLocalRotation;
                break;
            case (int)BattleAnimationParam.E_ANIMATION_TYPE.L_SCALE_X:
                origin = m_InitLocalScale;
                break;
        }

        switch ((int)type % 3)
        {
            case 0:
                return origin.x;
            case 1:
                return origin.y;
            case 2:
                return origin.z;
        }

        return 0;
    }
}
