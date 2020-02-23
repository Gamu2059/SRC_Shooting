using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 制御フローの複数処理機構。
/// </summary>
[Serializable]
public class SequenceGroup : SequenceElement
{
    [SerializeField]
    private List<SequenceElement> m_Elements;

    private int m_CurrentIndex;
    public int CurretIndex => m_CurrentIndex;

    /// <summary>
    /// これに入ってきた時に呼び出される。
    /// 初期化処理等を行う。
    /// </summary>
    public virtual void OnStart()
    {
        m_CurrentIndex = 0;
    }

    /// <summary>
    /// これのループ部分に差し掛かった時に呼び出される。
    /// フラグの初期化処理等を行う。
    /// </summary>
    public virtual void OnLooped()
    {
        m_CurrentIndex = 0;
    }

    /// <summary>
    /// これから出ていく時に呼び出される。
    /// 終了処理等を行う。
    /// </summary>
    public virtual void OnEnd()
    {

    }

    /// <summary>
    /// このグループが終了するかどうかを判定する。
    /// 終了する場合はtrueを返す。
    /// </summary>
    public virtual bool IsEnd()
    {
        return true;
    }

    /// <summary>
    /// 指し示すインデックスを一つ進める。
    /// </summary>
    public void Forward()
    {
        m_CurrentIndex++;
    }

    /// <summary>
    /// 現在指し示している要素を取得する。
    /// </summary>
    public SequenceElement GetCurrentReferenceElement()
    {
        return GetReferenceElementAt(m_CurrentIndex);
    }

    /// <summary>
    /// 現在指し示している要素の次の要素を取得する。
    /// </summary>
    public SequenceElement GetNextReferenceElement()
    {
        return GetReferenceElementAt(m_CurrentIndex + 1);
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
}
