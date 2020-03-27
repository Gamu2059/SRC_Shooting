#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 制御フローの複数処理機構。
/// </summary>
[Serializable, CreateAssetMenu(menuName = "Param/Sequence/Group/Default", fileName = "default.sequence_group.asset", order = 0)]
public class SequenceGroup : SequenceElement
{
    [SerializeField]
    private List<SequenceElement> m_Elements;

    [Header("Option Parameter")]

    [SerializeField]
    private SequenceOptionFunc[] m_OnStartOptions;

    [SerializeField]
    private SequenceOptionFunc[] m_OnEndOptions;

    [SerializeField]
    private SequenceOptionFunc[] m_OnLoopedOptions;

    public int CurrentIndex { get; private set; }
    protected SequenceController Controller { get; private set; }

    /// <summary>
    /// これに入ってきた時に呼び出される。
    /// 初期化処理等を行う。
    /// </summary>
    public void OnStartGroup(SequenceController controller)
    {
        CurrentIndex = 0;
        Controller = controller;

        if (m_OnStartOptions != null)
        {
            foreach (var option in m_OnStartOptions)
            {
                option?.Call();
            }
        }

        OnStart();
    }

    /// <summary>
    /// これのループ部分に差し掛かった時に呼び出される。
    /// フラグの初期化処理等を行う。
    /// </summary>
    public void OnLoopedGroup()
    {
        CurrentIndex = 0;

        if (m_OnLoopedOptions != null)
        {
            foreach (var option in m_OnLoopedOptions)
            {
                option?.Call();
            }
        }

        OnLooped();
    }

    /// <summary>
    /// これから出ていく時に呼び出される。
    /// 終了処理等を行う。
    /// </summary>
    public void OnEndGroup()
    {
        OnEnd();

        if (m_OnEndOptions != null)
        {
            foreach (var option in m_OnEndOptions)
            {
                option?.Call();
            }
        }

        Controller = null;
    }

    /// <summary>
    /// 指し示すインデックスを一つ進める。
    /// </summary>
    public void Forward()
    {
        CurrentIndex++;
    }

    /// <summary>
    /// 現在指し示している要素を取得する。
    /// </summary>
    public SequenceElement GetCurrentReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex);
    }

    /// <summary>
    /// 現在指し示している要素の次の要素を取得する。
    /// </summary>
    public SequenceElement GetNextReferenceElement()
    {
        return GetReferenceElementAt(CurrentIndex + 1);
    }

    /// <summary>
    /// 要素を直接指定して取得する。
    /// </summary>
    public SequenceElement GetReferenceElementAt(int index)
    {
        if (m_Elements == null || index < 0 || IsLastOver(index))
        {
            return null;
        }

        return m_Elements[index];
    }

    public bool IsLastOver(int index)
    {
        return m_Elements == null || index >= m_Elements.Count;
    }

    #region Have to Override Method

    public virtual void OnStart() { }

    public virtual void OnLooped() { }

    public virtual void OnEnd() { }

    /// <summary>
    /// このグループが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public virtual bool IsEndGroup()
    {
        return true;
    }

    #endregion
}
