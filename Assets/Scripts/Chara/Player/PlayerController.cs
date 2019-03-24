using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharaController
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

	[Space()]
	[Header( "�v���C���[�L������p �v���e�N�^�̃p�����[�^" )]

	[SerializeField, Tooltip( "�v���e�N�^�̃g�����X�t�H�[���z��" )]
	protected Transform[] m_Protectors;

	[SerializeField, Tooltip( "�v���e�N�^�̉�]���a" )]
	protected float m_ProtectorRadius;

	[SerializeField, Tooltip( "�v���e�N�^�̉�]���x" )]
	protected float m_ProtectorSpeed;

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
		PlayerCharaManager.Instance.RegistChara( this );
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
	}

	public override void OnStart()
	{
		base.OnStart();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		UpdateProtector();
	}

	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
	}

	/// <summary>
	/// �ʏ�e�𔭎˂���B
	/// ���̃��\�b�h���I�[�o�[���[�h���Ă��ꂼ��̃L�����ŗL�̏������L�q���ĉ������B
	/// </summary>
	public virtual void ShotBullet()
	{
		// �����I�[�o�[���[�h���Ȃ��ꍇ�͓K���ɒe���΂�
		BulletController.ShotBullet( this );
	}

	/// <summary>
	/// �{�����g�p����B
	/// </summary>
	public virtual void ShotBomb()
	{

	}



	/// <summary>
	/// �L�������ړ�������B
	/// �ړ����x�̓L�����Ɍ��ݐݒ肳��Ă�����̂ƂȂ�B
	/// </summary>
	/// <param name="moveDirection"> �ړ����� </param>
	public virtual void Move( Vector3 moveDirection )
	{
		Vector3 move = moveDirection.normalized * m_MoveSpeed * Time.deltaTime;
		transform.Translate( move, Space.World );
	}


	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		base.OnSuffer( bullet, colliderData );
	}

	public int GetLevel()
	{
		return m_Lv;
	}

	protected virtual void UpdateProtector()
	{
		m_ProtectorRad += m_ProtectorSpeed * Time.deltaTime;
		m_ProtectorRad %= Mathf.PI * 2;
		float unitAngle = Mathf.PI * 2 / m_Protectors.Length;

		for( int i = 0; i < m_Protectors.Length; i++ )
		{
			float angle = unitAngle * i + m_ProtectorRad;
			float x = m_ProtectorRadius * Mathf.Cos( angle );
			float z = m_ProtectorRadius * Mathf.Sin( angle );
			m_Protectors[i].localPosition = new Vector3( x, 0, z );
			m_Protectors[i].LookAt( transform );
		}
	}

	public override void Dead()
	{
		base.Dead();

		gameObject.SetActive( false );
		BattleManager.Instance.GameOver();
	}
}
