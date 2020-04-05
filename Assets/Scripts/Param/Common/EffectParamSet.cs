#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// エフェクトの発生パラメータ。
/// リアルモードとハッキングモードで共通。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Effect/Effect", fileName = "param.effect.asset")]
public class EffectParamSet : ScriptableObject
{
    [SerializeField, Tooltip("エフェクト本体")]
    private BattleCommonEffectController m_Effect;
    public BattleCommonEffectController Effect => m_Effect;

    [SerializeField]
    private E_RELATIVE m_FirePositionRelative;
    public E_RELATIVE FirePositionRelative => m_FirePositionRelative;

    [SerializeField, Tooltip("エフェクトの発生位置 RelativeがABSOLUTEの場合は画面中央が原点に、RelativeがRELATIVEの場合は発生オブジェクトが原点になる")]
    private Vector3ValueParam m_FirePosition;

    [SerializeField]
    private E_RELATIVE m_FireRotationRelative;
    public E_RELATIVE FireRotationRelative => m_FireRotationRelative;

    [SerializeField, Tooltip("エフェクトの発生角度 RelativeがABSOLUTEの場合はゲーム空間が基準に、RelativeがRELATIVEの場合は発生オブジェクトが基準になる")]
    private Vector3ValueParam m_FireRotation;

    [SerializeField]
    private E_RELATIVE m_FireScaleRelative;
    public E_RELATIVE FireScaleRelative => m_FireScaleRelative;

    [SerializeField, Tooltip("エフェクトの発生スケール RelativeがABSOLUTEの場合はゲーム空間が基準に、RelativeがRELATIVEの場合は発生オブジェクトのスケールに対しての倍率になる")]
    private Vector3ValueParam m_FireScale;

    [SerializeField, Tooltip("発生オブジェクトの位置を追従するかどうか")]
    private bool m_IsAllowOwnerPosition;
    public bool IsAllowOwnerPosition => m_IsAllowOwnerPosition;

    [SerializeField, Tooltip("発生オブジェクトの角度を追従するかどうか")]
    private bool m_IsAllowOwnerRotation;
    public bool IsAllowOwnerRotation => m_IsAllowOwnerRotation;

    [SerializeField, Tooltip("再生時間を超過したら自動的に破棄するかどうか")]
    private bool m_IsAutoDestroyDuration;
    public bool IsAutoDestroyDuration => m_IsAutoDestroyDuration;

    [SerializeField, Tooltip("再生時間")]
    private float m_Duration;
    public float Duration => m_Duration;

    [SerializeField, Tooltip("再生と同時に鳴らしたい音")]
    private PlaySoundParam[] m_PlaySoundParams;
    public PlaySoundParam[] PlaySoundParams => m_PlaySoundParams;

    public Vector3 GetFirePosition()
    {
        if (m_FirePosition == null)
        {
            return Vector3.zero;
        }

        return m_FirePosition.GetValue();
    }

    public Vector3 GetFireRotation()
    {
        if (m_FireRotation == null)
        {
            return Vector3.zero;
        }

        return m_FireRotation.GetValue();
    }

    public Vector3 GetFireScale()
    {
        if (m_FireScale == null)
        {
            // スケールだけは1にしないと見えなくなってしまう
            return Vector3.one;
        }

        return m_FireScale.GetValue();
    }
}
