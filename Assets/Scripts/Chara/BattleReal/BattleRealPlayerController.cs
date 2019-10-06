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

	#region Field Inspector

	[Space()]
	[Header( "�v���C���[�L������p ��{�X�e�[�^�X" )]

	[SerializeField, Tooltip( "�L�����̃��x��" )]
	private int m_Lv;

	[SerializeField, Tooltip( "���x���㏸�ɕK�v�Ȍo���l" )]
	private int m_Exp;

	[SerializeField, Tooltip( "�L�����̈ړ����x" )]
	private float m_MoveSpeed = 5f;

	[SerializeField, Tooltip( "�L�����̃��C�t�T�C�N��" )]
	private E_PLAYER_LIFE_CYCLE m_LifeCycle;

	#endregion

	#region Field

	/// <summary>
	/// �v���e�N�^�̉�]�̊�p�x
	/// </summary>
	protected float m_ProtectorRad;

	#endregion

	#region Getter & Setter

	public int GetLv()
	{
		return m_Lv;
	}

	public void SetLv( int lv )
	{
		m_Lv = lv;
	}

	public void AddLv( int lv )
	{
		m_Lv += lv;
	}

	public int GetExp()
	{
		return m_Exp;
	}

	public void SetExp( int exp )
	{
		m_Exp = exp;
	}

	public void AddExp( int exp )
	{
		m_Exp += exp;
	}

	public float GetMoveSpeed()
	{
		return m_MoveSpeed;
	}

	public void SetMoveSpeed( float moveSpeed )
	{
		m_MoveSpeed = moveSpeed;
	}

	public E_PLAYER_LIFE_CYCLE GetLifeCycle()
	{
		return m_LifeCycle;
	}

	public void SetLifeCycle( E_PLAYER_LIFE_CYCLE lifeCycle )
	{
		m_LifeCycle = lifeCycle;
	}

	#endregion

	private void Start()
	{
        // �J������p�ŁA�����I�Ƀ}�l�[�W���ɃL������ǉ����邽�߂�Unity��Start��p���Ă��܂�
        BattleRealPlayerManager.RegistPlayer(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    /// <summary>
    /// �ʏ�e�𔭎˂���B
    /// ���̃��\�b�h���I�[�o�[���[�h���Ă��ꂼ��̃L�����ŗL�̏������L�q���ĉ������B
    /// </summary>
    public virtual void ShotBullet(E_INPUT_STATE state)
	{
		// �����I�[�o�[���[�h���Ȃ��ꍇ�͓K���ɒe���΂�
		BulletController.ShotBullet( this );
	}

	/// <summary>
	/// �{�����g�p����B
	/// </summary>
	public virtual void ShotBomb(E_INPUT_STATE state)
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
