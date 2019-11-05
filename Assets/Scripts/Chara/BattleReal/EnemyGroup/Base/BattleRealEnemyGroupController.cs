using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BattleRealEnemyGroupController : ControllableMonoBehavior
{
    #region Field

    private BattleRealEnemyGroupGenerateParamSet m_GenerateParamSet;
    protected BattleRealEnemyGroupGenerateParamSet GenerateParamSet => m_GenerateParamSet;

    private BattleRealEnemyGroupBehaviorParamSet m_BehaviorParamSet;
    protected BattleRealEnemyGroupBehaviorParamSet BehaviorParamSet => m_BehaviorParamSet;

    private E_POOLED_OBJECT_CYCLE m_Cycle;

    private List<EnemyIndividualGenerateParamSet> m_IndividualParamSets;
    private List<EnemyIndividualGenerateParamSet> m_RemoveIndividualParamSets;

    private List<BattleRealEnemyController> m_CreatedEnemyControllers;

    private float m_CreateEnemyCount;

    #endregion

    #region Get & Set

    public E_POOLED_OBJECT_CYCLE GetCycle()
    {
        return m_Cycle;
    }

    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle)
    {
        m_Cycle = cycle;
    }

    #endregion

    #region Game Cycle

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_IndividualParamSets = new List<EnemyIndividualGenerateParamSet>();
        m_RemoveIndividualParamSets = new List<EnemyIndividualGenerateParamSet>();
        m_CreatedEnemyControllers = new List<BattleRealEnemyController>();
    }

    public override void OnFinalize()
    {
        for (int i = 0; i < m_CreatedEnemyControllers.Count; i++)
        {
            var enemy = m_CreatedEnemyControllers[i];
            if (enemy != null)
            {
                enemy.Destroy();
            }
        }
        m_CreatedEnemyControllers.Clear();
        m_CreatedEnemyControllers = null;
        m_IndividualParamSets.Clear();
        m_IndividualParamSets = null;
        m_RemoveIndividualParamSets = null;

        m_BehaviorParamSet = null;
        m_GenerateParamSet = null;

        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (m_GenerateParamSet != null)
        {
            var individualParamSets = m_GenerateParamSet.IndividualGenerateParamSets;
            m_IndividualParamSets.AddRange(individualParamSets);
        }

        var viewPortPos = m_GenerateParamSet.ViewPortPos;
        var offsetPos = m_GenerateParamSet.OffsetPosFromViewPort;
        var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewPortPos.x, viewPortPos.y);
        pos += offsetPos.ToVector3XZ();
        transform.position = pos;

        var angles = transform.eulerAngles;
        angles.y = m_GenerateParamSet.GenerateAngle;
        transform.eulerAngles = angles;

        m_CreateEnemyCount = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        for (int i = 0; i < m_IndividualParamSets.Count; i++)
        {
            var paramSet = m_IndividualParamSets[i];
            if (m_CreateEnemyCount >= paramSet.GenerateTime)
            {
                CreateEnemy(paramSet);
                m_RemoveIndividualParamSets.Add(paramSet);
            }
        }

        if (m_RemoveIndividualParamSets.Count > 0)
        {
            foreach (var paramSet in m_RemoveIndividualParamSets)
            {
                m_IndividualParamSets.Remove(paramSet);
            }
            m_RemoveIndividualParamSets.Clear();
        }

        m_CreateEnemyCount += Time.deltaTime;
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (m_GenerateParamSet == null)
        {
            Destory();
            return;
        }

        int removedNum = 0;
        for (int i = 0; i < m_CreatedEnemyControllers.Count; i++)
        {
            if (m_CreatedEnemyControllers[i] == null)
            {
                removedNum++;
            }
        }

        if (removedNum >= m_GenerateParamSet.IndividualGenerateParamSets.Length)
        {
            Destory();
        }
    }

    #endregion

    public void SetParamSet(BattleRealEnemyGroupGenerateParamSet paramSet)
    {
        m_GenerateParamSet = paramSet;
        m_BehaviorParamSet = m_GenerateParamSet.EnemyGroupBehaviorParamSet;
        OnSetParamSet();
    }

    protected virtual void OnSetParamSet()
    {

    }

    private void CreateEnemy(EnemyIndividualGenerateParamSet individualParamSet)
    {
        if (individualParamSet == null)
        {
            return;
        }

        var generateParamSet = individualParamSet.EnemyGenerateParamSet;
        var behaviorParamSet = individualParamSet.EnemyBehaviorParamSet;
        var enemy = BattleRealEnemyManager.Instance.CreateEnemy(generateParamSet, behaviorParamSet);
        if (enemy == null)
        {
            return;
        }
        m_CreatedEnemyControllers.Add(enemy);

        var enemyT = enemy.transform;
        enemyT.SetParent(transform);

        var viewPortPos = individualParamSet.ViewPortPos;
        var offsetPos = individualParamSet.OffsetPosFromViewPort;
        var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewPortPos.x, viewPortPos.y);
        pos += offsetPos.ToVector3XZ();

        if (individualParamSet.Relative == E_RELATIVE.RELATIVE)
        {
            enemyT.localPosition = pos;
            var angles = enemyT.localEulerAngles;
            angles.y = individualParamSet.GenerateAngle;
            enemyT.localEulerAngles = angles;
        }
        else
        {
            enemyT.position = pos;
            var angles = enemyT.eulerAngles;
            angles.y = individualParamSet.GenerateAngle;
            enemyT.eulerAngles = angles;
        }

        // あくまでY軸は基準に合わせる
        pos = enemyT.position;
        pos.y = ParamDef.BASE_Y_POS;
        enemyT.position = pos;
    }

    public void Destory()
    {
        BattleRealEnemyGroupManager.Instance.DestroyEnemyGroup(this);
    }
}