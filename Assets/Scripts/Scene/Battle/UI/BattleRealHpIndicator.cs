using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リアルモードの敵HPゲージを表示する
/// </summary>
public class BattleRealHpIndicator : BattleRealObjectBase
{
    /// <summary>
    /// 敵HPのサイクル
    /// <summary>
    E_POOLED_OBJECT_CYCLE m_Cycle;

    [SerializeField]
    private Text m_HPText;

    #region  Get&Set

    public E_POOLED_OBJECT_CYCLE GetCycle(){
        return m_Cycle;
    }

    public void SetCycle(E_POOLED_OBJECT_CYCLE cycle){
        m_Cycle = cycle;
    }

    #endregion    
    
    #region GameCycle

    protected override void OnAwake(){

    }

    protected override void OnDestroyed(){

    }

    public override void OnInitialize(){

    }

    public override void OnFixedUpdate(){

    }

    public override void OnUpdate(){

    }

    public override void OnLateUpdate(){

    }

    #endregion

    #region impl Collision
    public override void ClearColliderFlag(){

    }

    public override void ProcessCollision(){

    }
    
    #endregion
}
