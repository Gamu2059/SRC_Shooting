using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BattleReal.EnemyGenerator;

/// <summary>
/// 敵グループの制御クラス。
/// </summary>
[Serializable]
public class BattleRealEnemyGroupController : ControllableMonoBehavior
{
    #region Field

    protected BattleRealEnemyGroupParam Param { get; private set; }
    protected BattleRealEnemyGeneratorBase EnemyGenerator { get; private set; }
    protected BattleRealEnemyGroupBehaviorUnitBase Behavior { get; private set; }
    private E_POOLED_OBJECT_CYCLE m_Cycle;

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

    public void SetParam(BattleRealEnemyGroupParam param)
    {
        Param = param;
        OnSetParam();
    }

    protected virtual void OnSetParam()
    {

    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    public override void OnFinalize()
    {
        Behavior?.OnEnd();
        EnemyGenerator?.OnEnd();
        Behavior?.OnFinalize();
        EnemyGenerator?.OnFinalize();
        Behavior = null;
        EnemyGenerator = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();

        if (Param == null)
        {
            Debug.LogWarningFormat("BattleRealEnemyGroupParam is null.");
            Destory();
            return;
        }

        EnemyGenerator = Instantiate(Param.EnemyGenerator);
        Behavior = Instantiate(Param.Behavior);

        var viewPortPos = Param.ViewPortPos;
        var offsetPos = Param.OffsetPosFromViewPort;
        var pos = BattleRealStageManager.Instance.GetPositionFromFieldViewPortPosition(viewPortPos.x, viewPortPos.y) + offsetPos;

        var angles = transform.eulerAngles;
        angles.y = Param.GenerateAngle;

        transform.SetPositionAndRotation(pos, Quaternion.Euler(angles));

        EnemyGenerator.SetEnemyGroup(this);
        Behavior.SetEnemyGroup(this);

        EnemyGenerator.OnInitialize();
        Behavior.OnInitialize();

        EnemyGenerator.OnStart();
        Behavior.OnStart();

        BattleRealEventManager.Instance.AddEvent(Param.OnGenerateGroupEvents);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        EnemyGenerator?.OnUpdate();
        Behavior?.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        EnemyGenerator?.OnLateUpdate();
        Behavior?.OnLateUpdate();
        CheckDestroy();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        EnemyGenerator?.OnFixedUpdate();
        Behavior?.OnFixedUpdate();
    }

    #endregion

    public void Destory()
    {
        BattleRealEnemyGroupManager.Instance.DestroyEnemyGroup(this);
    }

    private void CheckDestroy()
    {
        if (!Param.UseDestroyCondition)
        {
            return;
        }

        if (BattleRealEventManager.Instance == null)
        {
            return;
        }

        var condition = Param.DestroyCondition;
        if (BattleRealEventManager.Instance.IsMeetRootCondition(ref condition))
        {
            Destory();
        }
    }
}