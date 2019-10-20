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

    BattleRealEnemyController m_AssignedEnemyController;

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
        this.m_HPText.rectTransform.position = m_AssignedEnemyController.transform.position;
        this.m_HPText.text = string.Format("{0}/{1}", m_AssignedEnemyController.GetNowHP(), m_AssignedEnemyController.GetMaxHP());
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

    public void SetParam(Vector2 vpos, BattleRealEnemyController enemyController){
        m_HPText.rectTransform.position = BattleRealEnemyManager.Instance.GetPositionFromFieldViewPortPosition(vpos.x, vpos.y);
        m_AssignedEnemyController = enemyController;
        if(enemyController.GetHpDisplayType() == E_HP_DISPLAY_TYPE.FROM_BEGIN){
            ChangeDisplayActive(true);
        }else{
            ChangeDisplayActive(false);
        }
    }

    public void ChangeDisplayActive(bool isActive){
        this.gameObject.SetActive(isActive);
    }
}
