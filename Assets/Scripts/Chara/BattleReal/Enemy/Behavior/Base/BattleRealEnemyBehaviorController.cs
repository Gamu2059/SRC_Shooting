using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// リアルモードの敵の振る舞いをシーケンスを用いて制御する
/// </summary>
public class BattleRealEnemyBehaviorController : ControllableObject
{
    private BattleRealEnemyBase m_Enemy;
    private Stack<BattleRealEnemyBehaviorGroup> m_GroupStack;
    private BattleRealEnemyBehaviorGroup m_CurrentGroup;
    private BattleRealEnemyBehaviorUnit m_CurrentUnit;

    /// <summary>
    /// 指定した振る舞いを終えた時に呼ばれるコールバック
    /// </summary>
    public Action OnEndBehavior;

    public BattleRealEnemyBehaviorController(BattleRealEnemyBase enemy)
    {
        m_Enemy = enemy;
    }

    /// <summary>
    /// コンストラクタとは異なり、こちらは複数回初期化される可能性があるものを置いてある。
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GroupStack = new Stack<BattleRealEnemyBehaviorGroup>();
        m_CurrentGroup = null;
        m_CurrentUnit = null;
    }

    public override void OnFinalize()
    {
        OnEndBehavior = null;
        ClearRemainData();
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
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        if (m_CurrentUnit == null)
        {
            return;
        }

        m_CurrentUnit.OnLateUpdateUnit(Time.deltaTime);
        if (m_CurrentUnit.IsEndUnit())
        {
            GoNextUnit(true, false, false);
        }
    }

    /// <summary>
    /// 振る舞いを中断する。
    /// </summary>
    public void StopBehavior()
    {
        m_CurrentUnit?.OnStopUnit();
        m_CurrentGroup?.OnStopGroup();
        ClearRemainData();
    }

    /// <summary>
    /// 処理階層をルートから始める
    /// </summary>
    public void BuildBehavior(BattleRealEnemyBehaviorGroup behaviorGroup)
    {
        if (behaviorGroup == null)
        {
            return;
        }

        ClearRemainData();
        m_CurrentGroup = GameObject.Instantiate(behaviorGroup);
        m_CurrentGroup.OnStartGroup(m_Enemy, this);
        GoNextUnit(false, false, false);
    }

    /// <summary>
    /// 残っているデータを消す。
    /// </summary>
    private void ClearRemainData()
    {
        m_GroupStack.Clear();
        m_CurrentGroup = null;
        m_CurrentUnit = null;
    }

    /// <summary>
    /// 処理階層を次のUnitへと遷移させる
    /// </summary>
    /// <param name="isForward">グループが指し示すUnitを一つ進めるかどうか</param>
    /// <param name="isSelfLoop">グループ自身の中でループが発生しているかどうか</param>
    /// <param name="isGroupEnded">グループが終了して、その関連で遷移させているかどうか</param>
    private void GoNextUnit(bool isForward, bool isSelfLoop, bool isGroupEnded)
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
                // グループよりユニットの方が終了呼び出しは早い
                m_CurrentUnit?.OnEndUnit();
                m_CurrentUnit = null;

                // 次が無いのでグループの終了判定を見る
                if (m_CurrentGroup.IsEndGroup())
                {
                    m_CurrentGroup.OnEndGroup();
                    m_CurrentGroup = null;

                    if (m_GroupStack.Count > 0)
                    {
                        // 1つ上に戻って次を探す
                        m_CurrentGroup = m_GroupStack.Pop();
                        GoNextUnit(true, false, true);
                    }
                    else
                    {
                        // もう何もないので処理を止める
                        OnEndBehavior?.Invoke();
                    }
                }
                else
                {
                    if (!isSelfLoop)
                    {
                        // 自分自身の最初に戻って次を探す
                        m_CurrentGroup.OnLoopedGroup();
                        GoNextUnit(false, true, true);
                    }
                    else
                    {
                        // 無限ループするので処理を止める
                        Debug.LogErrorFormat("{0} : 無限ループが発生したため終了します", GetType().Name);
                        m_CurrentGroup = null;
                        m_CurrentUnit = null;
                        OnEndBehavior?.Invoke();
                    }
                }
            }
            else
            {
                // 単純にnullなだけなので、次に飛ばす
                GoNextUnit(true, false, false);
            }

            return;
        }

        if (nextReferenceElement is BattleRealEnemyBehaviorGroup nextReferenceGroup)
        {
            // 次のグループが開始するよりも前に現在のユニットは終了する
            if (m_CurrentUnit != null)
            {
                m_CurrentUnit.OnEndUnit();
                m_CurrentUnit = null;
            }

            // 次のUnitがGroupだった場合は、スタックに入れてさらに下の階層をたどる
            m_GroupStack.Push(m_CurrentGroup);
            m_CurrentGroup = GameObject.Instantiate(nextReferenceGroup);
            m_CurrentGroup.OnStartGroup(m_Enemy, this);
            GoNextUnit(false, false, false);
            return;
        }

        // グループ終了の場合は既にOnEndUnitが呼ばれているはずなのでスルー
        if (!isGroupEnded)
        {
            m_CurrentUnit?.OnEndUnit();
        }

        if (nextReferenceElement is BattleRealEnemyBehaviorUnit nextReferenceUnit)
        {
            m_CurrentUnit = GameObject.Instantiate(nextReferenceUnit);
            m_CurrentUnit.OnStartUnit(m_Enemy, this);
        }
        else
        {
            Debug.LogErrorFormat("{0} : 適切でない型のSequenceUnitです {1}", GetType().Name, nextReferenceElement.GetType().Name);
        }
    }
}
