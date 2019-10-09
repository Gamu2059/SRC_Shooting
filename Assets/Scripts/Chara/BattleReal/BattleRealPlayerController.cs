using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���A�����[�h�̃v���C���[�R���g���[��
/// </summary>
public class BattleRealPlayerController : CharaController
{
	/// <summary>
	/// �v���C���[�L�����̃��C�t�T�C�N��
	/// </summary>
	[System.Serializable]
	public enum E_PLAYER_LIFE_CYCLE
	{
		/// <summary>
		/// �퓬��ʂɂ͏o�Ă��Ȃ�
		/// </summary>
		AHEAD,

		/// <summary>
		/// ���ݐ퓬��
		/// </summary>
		SORTIE,

		/// <summary>
		/// ���S�ɂ��퓬��ʂ���ޏ�
		/// </summary>
		DEAD,
	}

    #region Field

    private BattleRealPlayerParamSet m_ParamSet;

    private int m_Level;

    private float m_ShotRemainTime;

    #endregion

	private void Start()
	{
        // �J������p�ŁA�����I�Ƀ}�l�[�W���ɃL������ǉ����邽�߂�Unity��Start��p���Ă��܂�
        BattleRealPlayerManager.RegistPlayer(this);
    }

    public override void OnStart()
    {
        base.OnStart();
        m_ShotRemainTime = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        m_ShotRemainTime -= Time.deltaTime;
    }

    public void SetParamSet(BattleRealPlayerParamSet paramSet)
    {
        m_ParamSet = paramSet;
    }

    /// <summary>
    /// �ʏ�e�𔭎˂���B
    /// </summary>
    public virtual void ShotBullet()
	{
        
	}

    public void ChargeLaser()
    {

    }

    public virtual void ShotLaser()
    {

    }

    public void ChargeBomb()
    {

    }

    public virtual void ShotBomb()
    {

    }

    public override void HitItem(ItemController targetItem, ColliderData attackData, ColliderData targetData)
    {
        base.HitItem(targetItem, attackData, targetData);

        if (targetData.CollideName != ItemController.GAIN_COLLIDE)
        {
            return;
        }

        switch(targetItem.GetItemType())
        {
            case E_ITEM_TYPE.SMALL_SCORE:
            case E_ITEM_TYPE.BIG_SCORE:
                //BattleRealPlayerManager.Instance.AddScore(targetItem.GetPoint());
                break;
            case E_ITEM_TYPE.SMALL_SCORE_UP:
            case E_ITEM_TYPE.BIG_SCORE_UP:
                break;
            case E_ITEM_TYPE.SMALL_EXP:
            case E_ITEM_TYPE.BIG_EXP:
                //BattleRealPlayerManager.Instance.AddExp(targetItem.GetPoint());
                break;
            case E_ITEM_TYPE.SMALL_BOMB:
            case E_ITEM_TYPE.BIG_BOMB:
                //BattleRealPlayerManager.Instance.AddBombCharge(targetItem.GetPoint());
                break;
        }
    }

    public int GetLevel()
	{
        //return BattleRealPlayerManager.Instance.GetCurrentLevel().Value;
        return 0;
	}

	public override void Dead()
	{
		if( BattleManager.Instance.m_PlayerNotDead )
		{
			return;
		}

		base.Dead();

		gameObject.SetActive( false );
		BattleManager.Instance.GameOver();
	}
}