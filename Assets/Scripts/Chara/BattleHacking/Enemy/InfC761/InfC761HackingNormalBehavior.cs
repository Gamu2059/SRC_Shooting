using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InfC761の振る舞いの基底クラス。<br/>
/// InfC761のボスの目が動く部分はこのクラスで固有に制御されているため、このクラスは削除しないでください。
/// </summary>
public class InfC761HackingNormalBehavior : BattleHackingBossBehavior
{
    private InfC761HackingGraphicsController m_GraphicsController;

    public InfC761HackingNormalBehavior(BattleHackingEnemyController enemy, BattleHackingBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {

    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GraphicsController = Enemy.GetComponent<InfC761HackingGraphicsController>();
        m_GraphicsController?.OnInitialize();
    }

    public override void OnFinalize()
    {
        m_GraphicsController?.OnFinalize();
        m_GraphicsController = null;
        base.OnFinalize();
    }

    public override void OnStart()
    {
        base.OnStart();
        if (m_GraphicsController != null)
        {
            m_GraphicsController.SetEnableEyeMove(true);
            m_GraphicsController.OnStart();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_GraphicsController?.OnUpdate();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();
        m_GraphicsController?.OnLateUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        m_GraphicsController?.OnFixedUpdate();
    }
}
