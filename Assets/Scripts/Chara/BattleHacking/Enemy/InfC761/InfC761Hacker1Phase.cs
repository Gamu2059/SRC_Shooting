using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfC761Hacker1Phase : BattleHackingBossBehavior
{
    // InfC761 Hacker1の見た目に強く依存しているコンポーネント
    private InfC761Hacker1GraphicsController m_GraphicsController;


    public InfC761Hacker1Phase(BattleHackingEnemyController enemy, BattleHackingBossBehaviorParamSet paramSet) : base(enemy, paramSet)
    {

    }


    public override void OnInitialize()
    {
        base.OnInitialize();
        m_GraphicsController = Enemy.GetComponent<InfC761Hacker1GraphicsController>();
    }


    public override void OnFinalize()
    {
        m_GraphicsController = null;
        base.OnFinalize();
    }


    /// <summary>
    /// この行動パターンに入った瞬間に呼び出される
    /// </summary>
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

        if (m_GraphicsController != null)
        {
            m_GraphicsController.OnUpdate();
        }
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (m_GraphicsController != null)
        {
            m_GraphicsController.OnFixedUpdate();
        }
    }
}
