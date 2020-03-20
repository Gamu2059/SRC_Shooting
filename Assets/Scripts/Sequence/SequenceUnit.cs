#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 制御フローの単一処理機構。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Unit/Default", fileName = "default.sequence_unit.asset", order = 0)]
public class SequenceUnit : SequenceElement
{
    [SerializeField, Tooltip("シーケンスさせる座標系")]
    protected Space m_SpaceType;

    [Header("End Parameter")]

    [SerializeField, Tooltip("終了条件に先立って、割り込み終了関数を適用するかどうか")]
    private bool m_ApplyInterruptEnd;

    [SerializeField, Tooltip("割り込み終了関数")]
    private SequenceInterruptEndFunc m_InterruptEndFunc;

    [SerializeField, Tooltip("割り込み終了関数によって終了しなかった場合の、デフォルトの終了値")]
    private bool m_DefaultEndValue;

    [Header("Sequence Option")]

    [SerializeField]
    private SequenceOptionFunc[] m_OnStartOptions;

    [SerializeField]
    private SequenceOptionFunc[] m_OnEndOptions;

    protected Transform Target { get; private set; }
    protected SequenceController Controller { get; private set; }
    protected float CurrentTime { get; private set; }

    /// <summary>
    /// これに入ってきた時に呼び出される。
    /// 初期化処理等を行う。
    /// </summary>
    public void OnStartUnit(Transform target, SequenceController controller)
    {
        Target = target;
        Controller = controller;
        CurrentTime = 0;

        if (m_OnStartOptions == null)
        {
            return;
        }

        foreach (var option in m_OnStartOptions)
        {
            option?.Call(Target);
        }

        OnStart();
    }

    /// <summary>
    /// これに入った後、毎フレーム呼び出される。
    /// </summary>
    public void OnUpdateUnit(float deltaTime)
    {
        CurrentTime += deltaTime;
        OnUpdate(deltaTime);
    }

    /// <summary>
    /// これから出ていく時に呼び出される。
    /// 終了処理等を行う。
    /// </summary>
    public void OnEndUnit()
    {
        OnEnd();

        if (m_OnEndOptions == null)
        {
            return;
        }

        foreach (var option in m_OnEndOptions)
        {
            option?.Call(Target);
        }

        Controller = null;
        Target = null;
    }

    /// <summary>
    /// これが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public bool IsEndUnit()
    {
        if (m_ApplyInterruptEnd && m_InterruptEndFunc != null)
        {
            if (m_InterruptEndFunc.IsInterruptEnd(Target, Controller))
            {
                return true;
            }
        }

        return IsEnd();
    }

    #region Have to Override Method

    protected virtual void OnStart() { }

    protected virtual void OnUpdate(float deltaTime) { }

    protected virtual void OnEnd() { }

    protected virtual bool IsEnd() { return m_DefaultEndValue; }

    /// <summary>
    /// これに入ってきた時のトランスフォームの座標と回転を予測する。
    /// </summary>
    public virtual void GetStartTransform(Transform target, out Space spaceType, out Vector3 position, out Vector3 rotate)
    {
        spaceType = m_SpaceType;
        position = target.position;
        rotate = target.eulerAngles;
    }

    #endregion
}
