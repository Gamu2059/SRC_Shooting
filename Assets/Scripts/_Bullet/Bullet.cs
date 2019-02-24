using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全ての弾オブジェクトの基礎クラス。
/// </summary>
public class Bullet : BehaviorBase, ICollisionBase
{
	[Header( "Hit Info" )]

	[SerializeField]
	protected CollisionManager.ColliderTransform[] m_ColliderTransforms;

	[Header( "Parameter" )]

	[SerializeField]
	protected CharaControllerBase.E_CHARA_TROOP m_Troop;

	/// <summary>
	/// 弾を発射したキャラ
	/// </summary>
	[SerializeField]
	protected CharaControllerBase m_Owner;

	/// <summary>
	/// 弾の標的となっているキャラ
	/// </summary>
	[SerializeField]
	protected CharaControllerBase m_Target;

	[SerializeField]
	protected int m_BulletIndex;

	[SerializeField]
	protected BulletParam m_BulletParam;

	[SerializeField]
	protected BulletOrbitalParam m_OrbitalParam;

	protected bool m_IsStarted;
	protected float m_NowLifeTime;

	protected Vector3 m_NowDeltaRotation;
	protected Vector3 m_NowDeltaScale;

	protected Vector3 m_NowHitSize;
	protected Vector3 m_NowDeltaHitSize;
	protected int m_NowDamage;

	protected float m_NowSpeed;
	protected float m_NowAccel;

	protected bool m_NowSearch;
	protected float m_NowLerp;

	protected bool m_IsReverseHacked; // ハッカーのリバースハックを受けたかどうか


	public CollisionManager.ColliderTransform[] GetColliderTransforms()
	{
		return m_ColliderTransforms;
	}

	public CharaControllerBase.E_CHARA_TROOP GetTroop()
	{
		return m_Troop;
	}

	public BulletParam GetBulletParam()
	{
		return m_BulletParam;
	}

	public BulletOrbitalParam GetOrbitalParam()
	{
		return m_OrbitalParam;
	}



	/// <summary>
	/// 最初のフレームで呼び出される。
	/// </summary>
	public override void OnStart()
	{
		base.OnStart();
	}

	/// <summary>
	/// 最初以降のフレームで呼び出される。
	/// </summary>
	public override void OnUpdate()
	{
		if( !m_IsStarted )
		{
			m_IsStarted = true;
			OnStart();
			return;
		}

		base.OnUpdate();
		OnUpdateBulletOrbital();
	}

	/// <summary>
	/// StartやUpdateの後で呼び出される。
	/// </summary>
	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
		OnLateUpdateBulletOrbital();
	}



	/// <summary>
	/// この弾を発射させる。
	/// </summary>
	/// <param name="owner">この弾を発射させるキャラ</param>
	/// <param name="position">この弾を発射させる基準座標</param>
	/// <param name="rotation">この弾を発射させる基準回転</param>
	/// <param name="originScale">この弾の元々のスケール</param>
	/// <param name="bulletIndex">owneの何番目のプレハブの弾なのかを意味するindex</param>
	/// <param name="bulletParam">弾の全体のパラメータ</param>
	/// <param name="orbitalIndex">弾の軌道パラメータのインデックス defaultで初期軌道パラメータになる</param>
	public void ShotBullet( CharaControllerBase owner, Vector3 position, Vector3 rotation, Vector3 originScale, int bulletIndex, BulletParam bulletParam, int orbitalIndex = -1 )
	{
		if( owner == null || bulletParam == null )
		{
			DestroyBullet();
			return;
		}

		m_IsStarted = false;
		m_NowLifeTime = 0;
		InitNowBulletParams();

		m_Owner = owner;
		m_Troop = m_Owner.GetTroop();

		transform.position = position;
		transform.eulerAngles = rotation;
		transform.localScale = originScale;

		m_BulletIndex = bulletIndex;

		m_BulletParam = bulletParam;

		BulletOrbitalParam orbitalParam;

		if( orbitalIndex < 0 || orbitalIndex >= m_BulletParam.ConditionalOrbitalParams.Length )
		{
			orbitalParam = m_BulletParam.OrbitalParam;
		}
		else
		{
			orbitalParam = m_BulletParam.ConditionalOrbitalParams[orbitalIndex];
		}

		ChangeOrbital( orbitalParam );

		gameObject.SetActive( true );
		BulletManager.Instance.AddBullet( this );
		OnShotBullet();
	}

	/// <summary>
	/// この弾を破棄する。
	/// </summary>
	public virtual void DestroyBullet()
	{
		OnDestroyBullet();
		BulletManager.Instance.CheckRemovingBullet( this );
		gameObject.SetActive( false );
	}

	/// <summary>
	/// この弾の軌道情報を上書きする。
	/// </summary>
	public virtual void ChangeOrbital( BulletOrbitalParam orbitalParam )
	{
		if( m_Owner == null || m_BulletParam == null )
		{
			return;
		}

		m_OrbitalParam = orbitalParam;

		// ターゲットの上書き
		if( m_OrbitalParam.Target == E_ATTACK_TARGET.ENEMY )
		{
			m_Target = GetNearestEnemy();
		}
		else if( m_OrbitalParam.Target == E_ATTACK_TARGET.OWNER )
		{
			m_Target = m_Owner;
		}


		// 各種パラメータの上書き

		Vector3 bulletPos = GetRelativeValue( m_OrbitalParam.PositionRelative, transform.position, m_OrbitalParam.Position );
		Vector3 bulletRot = GetRelativeValue( m_OrbitalParam.RotationRelative, transform.eulerAngles, m_OrbitalParam.Rotation );
		Vector3 bulletScale = GetRelativeValue( m_OrbitalParam.ScaleRelative, transform.localScale, m_OrbitalParam.Scale );
		transform.position = bulletPos;
		transform.eulerAngles = bulletRot;
		transform.localScale = bulletScale;

		m_NowDeltaRotation = GetRelativeValue( m_OrbitalParam.DeltaRotationRelative, m_NowDeltaRotation, m_OrbitalParam.DeltaRotation );
		m_NowDeltaScale = GetRelativeValue( m_OrbitalParam.DeltaScaleRelative, m_NowDeltaScale, m_OrbitalParam.DeltaScale );

		m_NowHitSize = GetRelativeValue( m_OrbitalParam.HitSizeRelative, m_NowHitSize, m_OrbitalParam.HitSize );
		m_NowDeltaHitSize = GetRelativeValue( m_OrbitalParam.DeltaHitSizeRelative, m_NowDeltaHitSize, m_OrbitalParam.DeltaHitSize );
		m_NowDamage = ( int )GetRelativeValue( m_OrbitalParam.DamageRelative, m_NowDamage, m_OrbitalParam.Damage );

		m_NowSpeed = GetRelativeValue( m_OrbitalParam.SpeedRelative, m_NowSpeed, m_OrbitalParam.Speed );
		m_NowAccel = GetRelativeValue( m_OrbitalParam.AccelRelative, m_NowAccel, m_OrbitalParam.Accel );

		m_NowSearch = m_OrbitalParam.IsSearch;
		m_NowLerp = GetRelativeValue( m_OrbitalParam.LerpRelative, m_NowLerp, m_OrbitalParam.Lerp );

		if( m_Target != null && m_NowSearch )
		{
			transform.LookAt( m_Target.transform );
		}

		// オプションパラメータは後で実装
	}

	protected Vector3 GetRelativeValue( E_ATTACK_PARAM_RELATIVE relative, Vector3 baseValue, Vector3 relativeValue )
	{
		if( relative == E_ATTACK_PARAM_RELATIVE.RELATIVE )
		{
			return baseValue + relativeValue;
		}
		else
		{
			return relativeValue;
		}
	}

	protected float GetRelativeValue( E_ATTACK_PARAM_RELATIVE relative, float baseValue, float relativeValue )
	{
		if( relative == E_ATTACK_PARAM_RELATIVE.RELATIVE )
		{
			return baseValue + relativeValue;
		}
		else
		{
			return relativeValue;
		}
	}

	/// <summary>
	/// この弾の最も近くにいる敵を探し出す。
	/// </summary>
	protected CharaControllerBase GetNearestEnemy()
	{
		if( m_Troop == CharaControllerBase.E_CHARA_TROOP.ENEMY )
		{
			return PlayerCharaManager.Instance.GetCurrentController();
		}
		else
		{
			EnemyController[] enemies = EnemyCharaManager.Instance.GetControllers();
			CharaControllerBase nearestEnemy = null;
			float minSqrDist = float.MaxValue;

			foreach( var enemy in enemies )
			{
				float sqrDist = ( transform.position - enemy.transform.position ).sqrMagnitude;

				if( sqrDist < minSqrDist )
				{
					minSqrDist = sqrDist;
					nearestEnemy = enemy;
				}
			}

			return nearestEnemy;
		}
	}

	protected virtual void OnShotBullet()
	{

	}

	protected virtual void OnDestroyBullet()
	{

	}

	/// <summary>
	/// 弾の軌道を更新する。
	/// 弾の軌道を特殊なものにしたい場合は、このメソッドをオーバーライドして下さい。
	/// </summary>
	protected virtual void OnUpdateBulletOrbital()
	{
		if( m_BulletParam == null )
		{
			return;
		}

		Vector3 rot = transform.eulerAngles;
		Vector3 scl = transform.localScale;
		rot += m_NowDeltaRotation * Time.deltaTime;
		scl += m_NowDeltaScale * Time.deltaTime;
		transform.eulerAngles = rot;
		transform.localScale = scl;

		m_NowSpeed += m_NowAccel * Time.deltaTime;

		if( m_Target != null )
		{
			Vector3 deltaPos = m_Target.transform.position - transform.position;
			transform.forward = Vector3.Lerp( transform.forward, deltaPos.normalized, m_NowLerp );
		}

		transform.Translate( transform.forward * m_NowSpeed * Time.deltaTime, Space.World );

		m_NowHitSize += m_NowDeltaHitSize * Time.deltaTime;

		m_NowLifeTime += Time.deltaTime;

		if( m_NowLifeTime > m_BulletParam.LifeTime )
		{
			DestroyBullet();
		}
	}

	/// <summary>
	/// 弾の軌道を更新する。
	/// 弾の軌道を特殊なものにしたい場合は、このメソッドをオーバーライドして下さい。
	/// </summary>
	protected virtual void OnLateUpdateBulletOrbital()
	{
		if( m_BulletParam == null )
		{
			return;
		}
	}

	/// <summary>
	/// 全ての軌道用のパラメータを0にする。
	/// </summary>
	protected void InitNowBulletParams()
	{
		m_NowDeltaRotation = Vector3.zero;
		m_NowDeltaScale = Vector3.zero;

		m_NowHitSize = Vector3.zero;
		m_NowDeltaHitSize = Vector3.zero;
		m_NowDamage = 0;

		m_NowSpeed = 0;
		m_NowAccel = 0;

		m_NowSearch = false;
		m_NowLerp = 0;

		m_IsReverseHacked = false;
	}

	protected virtual void OnBecameInvisible()
	{
		DestroyBullet();
	}

	public virtual CollisionManager.ColliderData[] GetColliderData()
	{
		int hitNum = m_ColliderTransforms.Length;
		var colliders = new CollisionManager.ColliderData[hitNum];

		for( int i = 0; i < hitNum; i++ )
		{
			Transform t = m_ColliderTransforms[i].Transform;
			var c = new CollisionManager.ColliderData();
			c.CenterPos = new Vector2( t.position.x, t.position.z );
			c.Size = new Vector2( t.lossyScale.x, t.lossyScale.z );
			c.Angle = t.eulerAngles.y;
			c.ColliderType = m_ColliderTransforms[i].ColliderType;

			colliders[i] = c;
		}

		return colliders;
	}

	public virtual bool CanHitBullet()
	{
		return false;
	}

	public virtual void OnHitCharacter( CharaControllerBase chara )
	{

	}

	public virtual void OnHitBullet( Bullet bullet )
	{

	}

	public virtual void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{

	}
}
