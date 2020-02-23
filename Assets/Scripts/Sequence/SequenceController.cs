using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある単一のオブジェクトをシーケンスを用いて制御する
/// </summary>
public class SequenceController : ControllableMonoBehavior
{
    [SerializeField]
    private SequenceGroup m_RootGroup;

    private Stack<SequenceGroup> m_GroupStack;
    private SequenceGroup m_CurrentGroup;
    private SequenceUnit m_CurrentUnit;

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GroupStack = new Stack<SequenceGroup>();
    }

    public override void OnFinalize()
    {
        m_GroupStack.Clear();
        m_GroupStack = null;
        base.OnFinalize();
    }

    private void Start()
    {
        OnInitialize();
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void OnStart()
    {
        base.OnStart();
        BuildSequence(m_RootGroup);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_CurrentUnit == null)
        {
            return;
        }

        m_CurrentUnit.OnUpdate(transform, Time.deltaTime);
        if (m_CurrentUnit.IsEnd())
        {
            GoNextUnit(true, false);
        }
    }

    /// <summary>
    /// 処理階層をルートから始める
    /// </summary>
    private void BuildSequence(SequenceGroup rootGroup)
    {
        if (rootGroup == null)
        {
            return;
        }

        m_CurrentGroup = Instantiate(rootGroup);
        m_CurrentGroup.OnStart();
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
            if (m_CurrentGroup.IsLastOver(m_CurrentGroup.CurretIndex))
            {
                // 次が無いのでグループの終了判定を見る
                if (m_CurrentGroup.IsEnd())
                {
                    m_CurrentGroup.OnEnd();

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
                    }
                }
                else
                {
                    if (!isSelfLoop)
                    {
                        // 自分自身の最初に戻って次を探す
                        m_CurrentGroup.OnLooped();
                        GoNextUnit(false, true);
                    }
                    else
                    {
                        // 無限ループするので処理を止める
                        Debug.LogErrorFormat("{0} : 無限ループが発生したため終了します", GetType().Name);
                        m_CurrentGroup = null;
                        m_CurrentUnit = null;
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
            m_CurrentGroup.OnStart();
            GoNextUnit(false, false);
            return;
        }

        m_CurrentUnit?.OnEnd(transform);

        if (nextReferenceElement is SequenceUnit nextReferenceUnit)
        {
            m_CurrentUnit = Instantiate(nextReferenceUnit);
            m_CurrentUnit.OnStart(transform);
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

        return GetNextReferenceUnit(m_CurrentGroup.CurretIndex + 1, false);
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
                if (m_CurrentGroup.IsEnd())
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
