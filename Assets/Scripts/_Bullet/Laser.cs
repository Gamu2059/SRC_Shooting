using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全てのレーザーオブジェクトの基礎クラス。
/// </summary>
public class Laser : BehaviorBase
{

	[System.Serializable]
	public enum E_LASER_LIFE_CYCLE
	{
		BEGIN,
		FIRE,
		END,
	}

	[SerializeField]
	private CharaControllerBase.E_CHARA_TROOP m_Troop;

	[SerializeField]
	private CharaControllerBase m_Owner;

	[SerializeField]
	private CharaControllerBase m_Target;

	[SerializeField]
	private int m_LaserIndex;

	[SerializeField]
	private LaserParam m_LaserParam;

	[SerializeField]
	private LaserOrbitalParam m_OrbitalParam;

	[SerializeField]
	protected int m_OrbitalIndex;

	protected bool m_IsStarted;
	protected E_LASER_LIFE_CYCLE m_LifeCycle;
	protected float m_NowLifeTime;

	protected Vector3 m_NowAnchor;

	protected Vector3 m_NowAnchoredPosition;

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
		OnUpdateLaserOrbital();
	}

	/// <summary>
	/// StartやUpdateの後で呼び出される。
	/// </summary>
	public override void OnLateUpdate()
	{
		base.OnLateUpdate();
		OnLateUpdateLaserOrbital();
	}

	/// <summary>
	/// このレーザーを発射させる。
	/// </summary>
	/// <param name="owner">このレーザーを発射させるキャラ</param>
	/// <param name="position">このレーザーを発射させる基準座標</param>
	/// <param name="rotation">このレーザーを発射させる基準回転</param>
	/// <param name="originScale">このレーザーの元々のスケール</param>
	/// <param name="bulletIndex">owneの何番目のプレハブのレーザーなのかを意味するindex</param>
	/// <param name="bulletParam">レーザーの全体のパラメータ</param>
	/// <param name="orbitalIndex">レーザーの軌道パラメータのインデックス defaultで初期軌道パラメータになる</param>
	public void ShotLaser( CharaControllerBase owner, Vector3 position, Vector3 rotation, Vector3 originScale, int laserIndex, LaserParam laserParam, int orbitalIndex = -1 )
	{
		if( owner == null || laserParam == null )
		{
			DestroyLaser();
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

		m_LaserIndex = laserIndex;

		m_LaserParam = laserParam;

		LaserOrbitalParam orbitalParam = laserParam.BeginOrbitalParam;

		m_OrbitalIndex = orbitalIndex;

		ChangeOrbital( orbitalParam );

		gameObject.SetActive( true );
		//BulletManager.Instance.AddBullet( this );
		OnShotBullet();
	}

	/// <summary>
	/// このレーザーを破棄する。
	/// </summary>
	public virtual void DestroyLaser()
	{
		gameObject.SetActive( false );
	}

	/// <summary>
	/// この弾の軌道情報を上書きする。
	/// </summary>
	public virtual void ChangeOrbital( LaserOrbitalParam orbitalParam )
	{
		if( m_Owner == null || m_LaserParam == null )
		{
			return;
		}

		m_OrbitalParam = orbitalParam;

		// ターゲットの上書き
		m_Target = GetNearestEnemy();

		// 各種パラメータの上書き
		m_NowAnchor = GetRelativeValue( m_OrbitalParam.AnchorRelative, m_NowAnchor, m_OrbitalParam.Anchor );

		Vector3 laserAnchoredPos = GetRelativeValue( m_OrbitalParam.AnchoredPositionRelative, m_NowAnchoredPosition, m_OrbitalParam.AnchoredPosition );
		Vector3 laserRot = GetRelativeValue( m_OrbitalParam.RotationRelative, transform.eulerAngles, m_OrbitalParam.Rotation );
		Vector3 laserScale = GetRelativeValue( m_OrbitalParam.ScaleRelative, transform.localScale, m_OrbitalParam.Scale );
		transform.eulerAngles = laserRot;
		transform.localScale = laserScale;

		m_NowDeltaRotation = GetRelativeValue( m_OrbitalParam.DeltaRotationRelative, m_NowDeltaRotation, m_OrbitalParam.DeltaRotation );
		m_NowDeltaScale = GetRelativeValue( m_OrbitalParam.DeltaScaleRelative, m_NowDeltaScale, m_OrbitalParam.DeltaScale );

		m_NowHitSize = GetRelativeValue( m_OrbitalParam.HitSizeRelative, m_NowHitSize, m_OrbitalParam.HitSize );
		m_NowDeltaHitSize = GetRelativeValue( m_OrbitalParam.DeltaHitSizeRelative, m_NowDeltaHitSize, m_OrbitalParam.DeltaHitSize );
		m_NowDamage = ( int )GetRelativeValue( m_OrbitalParam.DamageRelative, m_NowDamage, m_OrbitalParam.Damage );

		m_NowSearch = m_OrbitalParam.IsSearch;
		m_NowLerp = GetRelativeValue( m_OrbitalParam.LerpRelative, m_NowLerp, m_OrbitalParam.Lerp );

		if( m_Target != null && m_NowSearch )
		{
			transform.LookAt( m_Target.transform );
		}

		// オプションパラメータは後で実装

		// 位置をアンカーに合わせる
		transform.position = laserAnchoredPos;
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
			EnemyController[] enemies = EnemyCharaManager.Instance.GetAllEnemyControllers();
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
	/// レーザーの軌道を更新する。
	/// レーザーの軌道を特殊なものにしたい場合は、このメソッドをオーバーライドして下さい。
	/// </summary>
	protected virtual void OnUpdateLaserOrbital()
	{
		if( m_LaserParam == null )
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

		if( m_NowLifeTime > m_LaserParam.LifeTime )
		{
			DestroyLaser();
		}
	}

	/// <summary>
	/// レーザーの軌道を更新する。
	/// レーザーの軌道を特殊なものにしたい場合は、このメソッドをオーバーライドして下さい。
	/// </summary>
	protected virtual void OnLateUpdateLaserOrbital()
	{
		if( m_LaserParam == null )
		{
			return;
		}
	}

	/// <summary>
	/// 全ての軌道用のパラメータを0にする。
	/// </summary>
	protected void InitNowBulletParams()
	{
		m_NowAnchor = Vector3.zero;

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
}
