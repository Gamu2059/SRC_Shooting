using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inf-C-761の振る舞いの基底クラス。
/// </summary>
public class InfC761Hacker1Phase : BattleHackingBossBehavior
{
    private InfC761Hacker1GraphicsController m_GraphicsController;

    public InfC761Hacker1Phase(BattleHackingEnemyController enemy, BattleHackingBossBehaviorUnitParamSet paramSet) : base(enemy, paramSet)
    {

    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GraphicsController = Enemy.GetComponent<InfC761Hacker1GraphicsController>();
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
