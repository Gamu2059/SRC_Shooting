using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupController : BattleControllableMonoBehavior
{
    [SerializeField]
    private EnemyGroupParam m_EnemyGroupParam;

    private List<EnemyGroupParam.EnemyParam> m_EnemyParams;

    private List<EnemyController> m_CreatedEnemyControllers;

    private float m_AppearTimeCount;

    public override void OnStart()
    {
        base.OnStart();

        m_EnemyParams = new List<EnemyGroupParam.EnemyParam>();

        m_CreatedEnemyControllers = new List<EnemyController>();

        m_EnemyParams.AddRange(m_EnemyGroupParam.GetEnemyGroupParams());

        m_AppearTimeCount = 0;
    }

    public override void OnUpdate()
    {

        base.OnUpdate();

        m_AppearTimeCount += Time.deltaTime;

        List<EnemyGroupParam.EnemyParam> removedEnemies = new List<EnemyGroupParam.EnemyParam>();

        foreach(var enemyParam in m_EnemyParams)
        {
            if(m_AppearTimeCount >= enemyParam.Appear)
            {
                m_CreatedEnemyControllers.Add(EnemyCharaManager.Instance.CreateEnemyFromEnemyParam(enemyParam.ListIndex));
                removedEnemies.Add(enemyParam);
            }
        }

        // 生成したEnemyを制御
        GroupUpdate();

        UpdateWithinGroup();

        foreach(var removedEnemy in removedEnemies)
        {
            m_EnemyParams.Remove(removedEnemy);
        }
    }

    protected virtual void GroupUpdate()
    {
        // ここ何書けばいいの？
    }

    private void UpdateWithinGroup()
    {
        foreach(var createdEnemyController in m_CreatedEnemyControllers)
        {
            createdEnemyController.OnUpdate();
        }
    }
}