using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharaController
{
	[Space()]
	[Header( "敵専用 パラメータ" )]

	[SerializeField, Tooltip( "ボスかどうか" )]
	private bool m_IsBoss;

	[SerializeField, Tooltip( "死亡時エフェクト" )]
	private GameObject m_DeadEffect;

	[SerializeField, Tooltip( "被弾直後の無敵時間" )]
	private float m_OnHitInvincibleDuration;

	protected StringParamSet m_ParamSet;

	private bool m_CanOutDestroy;

	private Timer m_OnHitInvincibleTimer;






	private void Start()
	{
		// 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
		EnemyCharaManager.Instance.RegistEnemy( this );
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
		m_CanOutDestroy = false;
	}

	public virtual void SetStringParam( string param )
	{
		m_ParamSet = StringParamTranslator.TranslateString( param );
	}

	protected virtual void OnBecameVisible()
	{
		m_CanOutDestroy = true;
	}

	protected virtual void OnBecameInvisible()
	{
		if( m_CanOutDestroy )
		{
			EnemyCharaManager.Instance.DestroyEnemy( this );
		}
	}

	public override void OnSuffer( BulletController bullet, ColliderData colliderData )
	{
		if( m_OnHitInvincibleDuration <= 0 )
		{
			base.OnSuffer( bullet, colliderData );
		}
		else if( m_OnHitInvincibleTimer == null )
		{
			m_OnHitInvincibleTimer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, m_OnHitInvincibleDuration, () =>
			{
				m_OnHitInvincibleTimer = null;
			} );
			BattleMainTimerManager.Instance.RegistTimer( m_OnHitInvincibleTimer );

			base.OnSuffer( bullet, colliderData );
		}
	}

	public override void Dead()
	{
		base.Dead();

		BattleMainTimerManager.Instance.RemoveTimer( m_OnHitInvincibleTimer );
		m_OnHitInvincibleTimer = null;
		EnemyCharaManager.Instance.DestroyEnemy( this );
	}


}
