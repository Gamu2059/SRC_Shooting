using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ある単一のオブジェクトをシーケンスを用いて制御する
/// </summary>
public class SequenceController : ControllableMonoBehavior
{
    private Stack<SequenceGroup> m_GroupStack;
    private SequenceGroup m_CurrentGroup;
    private SequenceUnit m_CurrentUnit;

    /// <summary>
    /// 指定したシーケンスを終えた時に呼ばれるコールバック
    /// </summary>
    public Action OnEndSequence;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GroupStack = new Stack<SequenceGroup>();
        m_CurrentGroup = null;
        m_CurrentUnit = null;
    }

    public override void OnFinalize()
    {
        OnEndSequence = null;
        m_GroupStack.Clear();
        m_GroupStack = null;
        base.OnFinalize();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_CurrentUnit == null)
        {
            return;
        }

        m_CurrentUnit.OnUpdateUnit(Time.deltaTime);
        if (m_CurrentUnit.IsEndUnit())
        {
            GoNextUnit(true, false);
        }
    }

    /// <summary>
    /// 処理階層をルートから始める
    /// </summary>
    public void BuildSequence(SequenceGroup rootGroup)
    {
        if (rootGroup == null)
        {
            return;
        }

        m_CurrentGroup = Instantiate(rootGroup);
        m_CurrentGroup.OnStartGroup(this);
        GoNextUnit(false, false);
    }

    /// <summary>
    /// 処理階層を次のUnitへと遷移させる
    /// </summary>
    /// <param name="isForward">グループが指し示すUnitを一つ進めるかどうか</param>
    /// <param name="isSelfLoop">グループ自身の中でループが発生しているかどうか</param>
    private void GoNextUnit(bool isForward, bool isSelfLoop)
    {
        if (m_CurrentGroup == null)
        {
            return;
        }

        if (isForward)
        {
            m_CurrentGroup.Forward();
        }

        var nextReferenceElement = m_CurrentGroup.GetCurrentReferenceElement();

        if (nextReferenceElement == null)
        {
            if (m_CurrentGroup.IsLastOver(m_CurrentGroup.CurrentIndex))
            {
                // 次が無いのでグループの終了判定を見る
                if (m_CurrentGroup.IsEndGroup())
                {
                    m_CurrentGroup.OnEndGroup();

                    if (m_GroupStack.Count > 0)
                    {
                        // 1つ上に戻って次を探す
                        m_CurrentGroup = m_GroupStack.Pop();
                        GoNextUnit(true, false);
                    }
                    else
                    {
                        // もう何もないので処理を止める
                        m_CurrentGroup = null;
                        m_CurrentUnit = null;
                        OnEndSequence?.Invoke();
                    }
                }
                else
                {
                    if (!isSelfLoop)
                    {
                        // 自分自身の最初に戻って次を探す
                        m_CurrentGroup.OnLoopedGroup();
                        GoNextUnit(false, true);
                    }
                    else
                    {
                        // 無限ループするので処理を止める
                        Debug.LogErrorFormat("{0} : 無限ループが発生したため終了します", GetType().Name);
                        m_CurrentGroup = null;
                        m_CurrentUnit = null;
                        OnEndSequence?.Invoke();
                    }
                }
            }
            else
            {
                // 単純にnullなだけなので、次に飛ばす
                GoNextUnit(true, false);
            }

            return;
        }

        if (nextReferenceElement is SequenceGroup nextReferenceGroup)
        {
            // 次のUnitがGroupだった場合は、スタックに入れてさらに下の階層をたどる
            m_GroupStack.Push(m_CurrentGroup);
            m_CurrentGroup = Instantiate(nextReferenceGroup);
            m_CurrentGroup.OnStartGroup(this);
            GoNextUnit(false, false);
            return;
        }

        m_CurrentUnit?.OnEndUnit();

        if (nextReferenceElement is SequenceUnit nextReferenceUnit)
        {
            m_CurrentUnit = Instantiate(nextReferenceUnit);
            m_CurrentUnit.OnStartUnit(transform, this);
        }
        else
        {
            Debug.LogErrorFormat("{0} : 適切でない型のSequenceUnitです {1}", GetType().Name, nextReferenceElement.GetType().Name);
        }
    }

    /// <summary>
    /// 次にあるUnitの参照体を取得する。
    /// </summary>
    public SequenceUnit GetNextReferenceUnit()
    {
        if (m_CurrentGroup == null)
        {
            return null;
        }

        return GetNextReferenceUnit(m_CurrentGroup.CurrentIndex + 1, false);
    }

    private SequenceUnit GetNextReferenceUnit(int index, bool isSelfLoop)
    {
        if (m_CurrentGroup == null)
        {
            return null;
        }

        var referenceElement = m_CurrentGroup.GetReferenceElementAt(index);
        if (referenceElement == null)
        {
            if (m_CurrentGroup.IsLastOver(index))
            {
                // グループの末端まで行った
                if (m_CurrentGroup.IsEndGroup())
                {
                    if (m_GroupStack.Count < 1)
                    {
                        return null;
                    }

                    var tempGroup = m_CurrentGroup;
                    m_CurrentGroup = m_GroupStack.Pop();
                    referenceElement = GetNextReferenceUnit(0, false);
                    m_GroupStack.Push(m_CurrentGroup);
                    m_CurrentGroup = tempGroup;
                }
                else
                {
                    if (isSelfLoop)
                    {
                        return null;
                    }

                    referenceElement = GetNextReferenceUnit(0, true);
                }
            }
            else
            {
                // 単純にnullだっただけ
                referenceElement = GetNextReferenceUnit(index + 1, false);
            }
        }

        if (referenceElement == null)
        {
            return null;
        }

        if (referenceElement is SequenceUnit sequenceUnit)
        {
            return sequenceUnit;
        }

        if (referenceElement is SequenceGroup sequenceGroup)
        {
            var tempGroup = m_CurrentGroup;
            m_CurrentGroup = sequenceGroup;
            var unit = GetNextReferenceUnit(0, false);
            m_CurrentGroup = tempGroup;
            return unit;
        }

        return null;
    }
}
