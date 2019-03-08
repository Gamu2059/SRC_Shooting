using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全ての弾オブジェクトの基礎クラス。
/// </summary>
public class Bullet : ControllableMonoBehaviour, ICollisionBase
{
	[Serializable]
	public enum E_BULLET_CYCLE
	{
		/// <summary>
		/// 発射される直前。
		/// </summary>
		STANDBY,

		/// <summary>
		/// 発射された後、動いている状態。
		/// </summary>
		UPDATE,

		/// <summary>
		/// プーリングされた状態。
		/// </summary>
		POOLED,
	}



	#region Field Inspector

	/// <summary>
	/// この弾の当たり判定情報。
	/// </summary>
	[SerializeField]
	private CollisionManager.ColliderTransform[] m_ColliderTransforms;

	/// <summary>
	/// この弾のグループ名。
	/// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
	/// </summary>
	[SerializeField]
	private string m_BulletGroupId = "Default Bullet";

	#endregion



	#region Field

	/// <summary>
	/// この弾がプレイヤー側と敵側のどちらに属しているか。
	/// 同じ値同士を持つ弾やキャラには被弾しない。
	/// </summary>
	private CharaControllerBase.E_CHARA_TROOP m_Troop;

	/// <summary>
	/// 弾を発射したキャラ。
	/// </summary>
	private CharaControllerBase m_BulletOwner;

	/// <summary>
	/// 弾の標的となっているキャラ。
	/// </summary>
	private CharaControllerBase m_Target;

	/// <summary>
	/// 発射したキャラの何番目の弾か。
	/// </summary>
	private int m_BulletIndex;

	/// <summary>
	/// この弾のBulletParam。
	/// BulletParamで制御されていない場合、この弾はプログラムで直接制御しなければならない。
	/// </summary>
	private BulletParam m_BulletParam;

	/// <summary>
	/// この弾のOrbialParam。
	/// 何も指定しない場合、この弾はプログラムで直接制御しなければならない。
	/// </summary>
	private BulletOrbitalParam m_OrbitalParam;

	/// <summary>
	/// この弾の状態。
	/// </summary>
	private E_BULLET_CYCLE m_BulletCycle;

	/// <summary>
	/// この弾が何秒生存しているか。
	/// </summary>
	private float m_NowLifeTime;

	/// <summary>
	/// この弾が1秒間にどれくらい回転するか。
	/// </summary>
	private Vector3 m_NowDeltaRotation;

	/// <summary>
	/// この弾が1秒間にどれくらいスケーリングするか。
	/// </summary>
	private Vector3 m_NowDeltaScale;

	/// <summary>
	/// この弾の攻撃力。
	/// </summary>
	private float m_NowDamage;

	/// <summary>
	/// この弾が1秒間にどれくらい移動するか。
	/// </summary>
	private float m_NowSpeed;

	/// <summary>
	/// この弾が1秒間にどれくらい加速するか。
	/// </summary>
	private float m_NowAccel;

	/// <summary>
	/// この弾が発射された瞬間にターゲットに設定されているキャラの方向を向くかどうか。
	/// </summary>
	private bool m_IsSearch;

	/// <summary>
	/// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するか。
	/// </summary>
	private float m_NowLerp;

	/// <summary>
	/// Hackerのリバースハックを受けたかどうか。
	/// </summary>
	private bool m_IsReverseHacked;

	#endregion



	#region Getter & Setter

	/// <summary>
	/// この弾の当たり判定情報を取得する。
	/// </summary>
	public CollisionManager.ColliderTransform[] GetColliderTransforms()
	{
		return m_ColliderTransforms;
	}

	/// <summary>
	/// この弾のグループ名を取得する。
	/// 同じ名前同士の弾は、違うキャラから生成されたものであっても、プールから再利用される。
	/// </summary>
	public string GetBulletGroupId()
	{
		return m_BulletGroupId;
	}

	/// <summary>
	/// この弾がプレイヤー側と敵側のどちらに属しているか。
	/// 同じ値同士を持つ弾やキャラには被弾しない。
	/// </summary>
	public CharaControllerBase.E_CHARA_TROOP GetTroop()
	{
		return m_Troop;
	}

	/// <summary>
	/// この弾がプレイヤー側と敵側のどちらに属しているかを設定する。
	/// </summary>
	public void SetTroop( CharaControllerBase.E_CHARA_TROOP troop )
	{
		m_Troop = troop;
	}

	/// <summary>
	/// この弾を発射したキャラを取得する。
	/// </summary>
	public CharaControllerBase GetBulletOwner()
	{
		return m_BulletOwner;
	}

	/// <summary>
	/// この弾の標的となっているキャラを取得する。
	/// </summary>
	public CharaControllerBase GetTarget()
	{
		return m_Target;
	}

	/// <summary>
	/// この弾の標的となっているキャラを設定する。
	/// </summary>
	public void SetTarget( CharaControllerBase target )
	{
		m_Target = target;
	}

	/// <summary>
	/// 発射したキャラの何番目の弾かを取得する。
	/// </summary>
	public int GetBulletIndex()
	{
		return m_BulletIndex;
	}

	/// <summary>
	/// この弾のBulletParam。
	/// BulletParamで制御されていない場合、この弾はプログラムで直接制御しなければならない。
	/// </summary>
	public BulletParam GetBulletParam()
	{
		return m_BulletParam;
	}

	/// <summary>
	/// この弾のOrbialParam。
	/// 何も指定しない場合、この弾はプログラムで直接制御しなければならない。
	/// </summary>
	public BulletOrbitalParam GetOrbitalParam()
	{
		return m_OrbitalParam;
	}

	/// <summary>
	/// この弾の状態を取得する。
	/// </summary>
	public E_BULLET_CYCLE GetBulletCycle()
	{
		return m_BulletCycle;
	}

	/// <summary>
	/// この弾の状態を設定する。
	/// </summary>
	public void SetBulletCycle( E_BULLET_CYCLE cycle )
	{
		m_BulletCycle = cycle;
	}

	/// <summary>
	/// この弾が何秒生存しているかを取得する。
	/// </summary>
	public float GetNowLifeTime()
	{
		return m_NowLifeTime;
	}

	/// <summary>
	/// この弾のローカル座標を取得する。
	/// </summary>
	public Vector3 GetPosition()
	{
		return transform.localPosition;
	}

	/// <summary>
	/// この弾のローカル座標を設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetPosition( Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		transform.localPosition = GetRelativeValue( relative, GetPosition(), value );
	}

	/// <summary>
	/// この弾のローカルオイラー回転を取得する。
	/// </summary>
	public Vector3 GetRotation()
	{
		return transform.localEulerAngles;
	}

	/// <summary>
	/// この弾のローカルオイラー回転を設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetRotation( Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		transform.localEulerAngles = GetRelativeValue( relative, GetRotation(), value );
	}

	/// <summary>
	/// この弾のローカルスケールを取得する。
	/// </summary>
	public Vector3 GetScale()
	{
		return transform.localScale;
	}

	/// <summary>
	/// この弾のローカルスケールを設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetScale( Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		transform.localScale = GetRelativeValue( relative, GetScale(), value );
	}

	/// <summary>
	/// この弾が1秒間にどれくらい回転するかを取得する。
	/// </summary>
	public Vector3 GetNowDeltaRotation()
	{
		return m_NowDeltaRotation;
	}

	/// <summary>
	/// この弾が1秒間にどれくらい回転するかを設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetNowDeltaRotation( Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowDeltaRotation = GetRelativeValue( relative, GetNowDeltaRotation(), value );
	}

	/// <summary>
	/// この弾が1秒間にどれくらいスケーリングするかを取得する。
	/// </summary>
	public Vector3 GetNowDeltaScale()
	{
		return m_NowDeltaScale;
	}

	/// <summary>
	/// この弾が1秒間にどれくらいスケーリングするかを設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetNowDeltaScale( Vector3 value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowDeltaScale = GetRelativeValue( relative, GetNowDeltaScale(), value );
	}

	/// <summary>
	/// この弾の攻撃力を取得する。
	/// </summary>
	public float GetNowDamage()
	{
		return m_NowDamage;
	}

	/// <summary>
	/// この弾の攻撃力を設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetNowDamage( float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowDamage = GetRelativeValue( relative, GetNowDamage(), value );
	}

	/// <summary>
	/// この弾が1秒間にどれくらい移動するかを取得する。
	/// </summary>
	public float GetNowSpeed()
	{
		return m_NowSpeed;
	}

	/// <summary>
	/// この弾が1秒間にどれくらい移動するかを取得する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetNowSpeed( float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowSpeed = GetRelativeValue( relative, GetNowSpeed(), value );
	}

	/// <summary>
	/// この弾が1秒間にどれくらい加速するかを取得する。
	/// </summary>
	public float GetNowAccel()
	{
		return m_NowAccel;
	}

	/// <summary>
	/// この弾が1秒間にどれくらい加速するかを設定する。
	/// </summary>
	/// <param name="value">設定する値</param>
	/// <param name="relative">値を絶対値として設定するか、相対値として設定するか</param>
	public void SetNowAccel( float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowAccel = GetRelativeValue( relative, GetNowAccel(), value );
	}

	/// <summary>
	/// この弾が発射された瞬間にターゲットに設定されているキャラの方向を向くかどうかを取得する。
	/// </summary>
	public bool IsSearch()
	{
		return m_IsSearch;
	}

	public void SetSearch( bool value )
	{
		m_IsSearch = value;
	}

	/// <summary>
	/// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するかを取得する。
	/// </summary>
	public float GetNowLerp()
	{
		return m_NowLerp;
	}

	/// <summary>
	/// この弾が1秒間にどれくらいターゲットに設定されているキャラを追従するかを設定する。
	/// </summary>
	public void SetNowLerp( float value, E_ATTACK_PARAM_RELATIVE relative = E_ATTACK_PARAM_RELATIVE.ABSOLUTE )
	{
		m_NowLerp = GetRelativeValue( relative, GetNowLerp(), value );
	}

	/// <summary>
	/// Hackerのリバースハックを受けたかどうかを取得する。
	/// </summary>
	public bool IsReverseHacked()
	{
		return m_IsReverseHacked;
	}

	/// <summary>
	/// Hackerのリバースハックを受けたかどうかを設定する。
	/// </summary>
	public void SetReverseHacked( bool value )
	{
		m_IsReverseHacked = value;
	}


	#endregion



	/// <summary>
	/// 弾を生成する。
	/// </summary>
	/// <param name="bulletOwner">弾を発射させるキャラ</param>
	private static Bullet CreateBullet( CharaControllerBase bulletOwner )
	{
		if( bulletOwner == null )
		{
			return null;
		}

		// プレハブを取得
		var bulletPrefab = bulletOwner.GetBulletPrefab();

		if( bulletPrefab == null )
		{
			return null;
		}

		// プールから弾を取得
		var bullet = BulletManager.Instance.GetPoolingBullet( bulletPrefab );

		if( bullet == null )
		{
			return null;
		}

		// 座標を設定
		bullet.SetPosition( bulletOwner.transform.position );

		// 回転を設定
		bullet.SetRotation( bulletOwner.transform.eulerAngles );

		// スケールを設定
		bullet.SetScale( bulletPrefab.transform.localScale );

		bullet.ResetBulletLifeTime();
		bullet.ResetBulletParam();

		bullet.m_BulletOwner = bulletOwner;
		bullet.SetTroop( bulletOwner.GetTroop() );
		bullet.m_BulletParam = null;
		bullet.m_BulletIndex = 0;

		return bullet;
	}

	/// <summary>
	/// 指定したキャラの最初に登録されている弾をBulletParamの指定なしで発射する。
	/// 弾は指定したキャラの位置に生成する。
	/// 発射後はプログラムで制御しないと動かないので注意。
	/// </summary>
	/// <param name="bulletOwner">弾を発射させるキャラ</param>
	/// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
	public static Bullet ShotBulletWithoutBulletParam( CharaControllerBase bulletOwner, bool isCheck = true )
	{
		var bullet = CreateBullet( bulletOwner );

		if( bullet == null )
		{
			return null;
		}

		if( isCheck )
		{
			BulletManager.Instance.CheckStandbyBullet( bullet );
		}

		return bullet;
	}

	/// <summary>
	/// 指定したキャラの最初に登録されている弾を最初に登録されているBulletParamのデフォルトのOrbitalParamで弾を発射する。
	/// 弾は指定したキャラの位置に生成する。
	/// </summary>
	/// <param name="bulletOwner">弾を発射させるキャラ</param>
	/// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
	public static Bullet ShotBullet( CharaControllerBase bulletOwner, bool isCheck = true )
	{
		var bullet = CreateBullet( bulletOwner );

		if( bullet == null )
		{
			return null;
		}

		// BulletParamを取得
		var bulletParam = bulletOwner.GetBulletParam();

		if( bulletParam != null )
		{
			// 軌道を設定
			bullet.ChangeOrbital( bulletParam.GetOrbitalParam() );
		}

		if( isCheck )
		{
			BulletManager.Instance.CheckStandbyBullet( bullet );
		}

		return bullet;
	}

	/// <summary>
	/// 指定したパラメータを用いて弾を生成する。
	/// </summary>
	/// <param name="shotParam">弾を発射させるパラメータ</param>
	private static Bullet CreateBullet( BulletShotParam shotParam )
	{
		var bulletOwner = shotParam.BulletOwner;

		if( bulletOwner == null )
		{
			return null;
		}

		// プレハブを取得
		var bulletPrefab = bulletOwner.GetBulletPrefab( shotParam.BulletIndex );

		if( bulletPrefab == null )
		{
			return null;
		}

		// プールから弾を取得
		var bullet = BulletManager.Instance.GetPoolingBullet( bulletPrefab );

		if( bullet == null )
		{
			return null;
		}

		// 座標を設定
		bullet.SetPosition( shotParam.Position != null ? ( Vector3 )shotParam.Position : bulletOwner.transform.position );

		// 回転を設定
		bullet.SetRotation( shotParam.Rotation != null ? ( Vector3 )shotParam.Rotation : bulletOwner.transform.eulerAngles );

		// スケールを設定
		bullet.SetScale( shotParam.Scale != null ? ( Vector3 )shotParam.Scale : bulletPrefab.transform.localScale );

		bullet.ResetBulletLifeTime();
		bullet.ResetBulletParam();

		bullet.m_BulletOwner = bulletOwner;
		bullet.SetTroop( bulletOwner.GetTroop() );
		bullet.m_BulletParam = null;
		bullet.m_BulletIndex = 0;

		return bullet;
	}

	/// <summary>
	/// 指定したパラメータを用いて弾をBulletParamの指定なしで発射する。
	/// 発射後はプログラムで制御しないと動かないので注意。
	/// </summary>
	/// <param name="bulletOwner">弾を発射させるキャラ</param>
	/// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
	public static Bullet ShotBulletWithoutBulletParam( BulletShotParam shotParam, bool isCheck = true )
	{
		var bullet = CreateBullet( shotParam );

		if( bullet == null )
		{
			return null;
		}

		if( isCheck )
		{
			BulletManager.Instance.CheckStandbyBullet( bullet );
		}

		return bullet;
	}

	/// <summary>
	/// 指定したパラメータを用いて弾を発射する。
	/// </summary>
	/// <param name="shotParam">発射時のパラメータ</param>
	/// <param name="isCheck">trueの場合、自動的にBulletManagerに弾をチェックする</param>
	public static Bullet ShotBullet( BulletShotParam shotParam, bool isCheck = true )
	{
		var bullet = CreateBullet( shotParam );

		if( bullet == null )
		{
			return null;
		}

		// BulletParamを取得
		var bulletParam = shotParam.BulletOwner.GetBulletParam( shotParam.BulletParamIndex );

		if( bulletParam != null )
		{
			// 軌道を設定
			bullet.ChangeOrbital( bulletParam.GetOrbitalParam( shotParam.OrbitalIndex ) );
		}

		if( isCheck )
		{
			BulletManager.Instance.CheckStandbyBullet( bullet );
		}

		return bullet;
	}

	[Obsolete]
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

		m_NowLifeTime = 0;
		ResetBulletParam();

		m_BulletOwner = owner;
		m_Troop = m_BulletOwner.GetTroop();

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

		BulletManager.Instance.CheckStandbyBullet( this );
	}



	/// <summary>
	/// この弾の軌道情報を上書きする。
	/// </summary>
	public virtual void ChangeOrbital( BulletOrbitalParam orbitalParam )
	{
		m_OrbitalParam = orbitalParam;

		// ターゲットの上書き
		if( m_OrbitalParam.Target == E_ATTACK_TARGET.ENEMY )
		{
			m_Target = GetNearestEnemy();
		}
		else if( m_OrbitalParam.Target == E_ATTACK_TARGET.OWNER )
		{
			m_Target = m_BulletOwner;
		}


		// 各種パラメータの上書き
		SetPosition( m_OrbitalParam.Position, m_OrbitalParam.PositionRelative );
		SetRotation( m_OrbitalParam.Rotation, m_OrbitalParam.RotationRelative );
		SetScale( m_OrbitalParam.Scale, m_OrbitalParam.ScaleRelative );
		SetNowDeltaRotation( m_OrbitalParam.DeltaRotation, m_OrbitalParam.DeltaRotationRelative );
		SetNowDeltaScale( m_OrbitalParam.DeltaScale, m_OrbitalParam.DeltaScaleRelative );
		SetNowDamage( m_OrbitalParam.Damage, m_OrbitalParam.DamageRelative );
		SetNowSpeed( m_OrbitalParam.Speed, m_OrbitalParam.SpeedRelative );
		SetNowAccel( m_OrbitalParam.Accel, m_OrbitalParam.AccelRelative );
		SetSearch( m_OrbitalParam.IsSearch );
		SetNowLerp( m_OrbitalParam.Lerp, m_OrbitalParam.LerpRelative );

		// オプションパラメータは後で実装


		LookAtTarget();
	}

	/// <summary>
	/// この弾を破棄する。
	/// </summary>
	public virtual void DestroyBullet()
	{
		BulletManager.Instance.CheckPoolBullet( this );
	}

	/// <summary>
	/// Searchがtrueかつターゲットキャラが設定されている時、ターゲットキャラの方向を向く。
	/// </summary>
	public void LookAtTarget()
	{
		if( m_Target != null && m_IsSearch )
		{
			transform.LookAt( m_Target.transform );
		}
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
	public CharaControllerBase GetNearestEnemy()
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

	/// <summary>
	/// 弾の残存時間をリセットする。
	/// </summary>
	public void ResetBulletLifeTime()
	{
		m_NowLifeTime = 0;
	}

	/// <summary>
	/// 全ての軌道用のパラメータをリセットする。
	/// </summary>
	public void ResetBulletParam()
	{
		m_NowDeltaRotation = Vector3.zero;
		m_NowDeltaScale = Vector3.zero;

		m_NowDamage = 0;

		m_NowSpeed = 0;
		m_NowAccel = 0;

		m_IsSearch = false;
		m_NowLerp = 0;

		m_IsReverseHacked = false;
	}



	/// <summary>
	/// この弾が発射された瞬間に呼び出される処理。
	/// </summary>
	public override void OnInitialize()
	{
	}

	/// <summary>
	/// この弾が消滅する瞬間に呼び出される処理。
	/// </summary>
	public override void OnFinalize()
	{
	}

	public override void OnStart()
	{
	}

	public override void OnUpdate()
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

		m_NowLifeTime += Time.deltaTime;

		if( m_NowLifeTime > m_BulletParam.LifeTime )
		{
			DestroyBullet();
		}
	}

	public override void OnLateUpdate()
	{
	}

	protected virtual void OnBecameInvisible()
	{
		// 画面から見えなくなったら弾を破棄する
		DestroyBullet();
	}



	/// <summary>
	/// この弾の当たり判定情報を取得する。
	/// </summary>
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
			c.Angle = -t.eulerAngles.y;
			c.ColliderType = m_ColliderTransforms[i].ColliderType;

			colliders[i] = c;
		}

		return colliders;
	}

	/// <summary>
	/// この弾が他の弾に衝突するかどうかを取得する。
	/// </summary>
	public virtual bool CanHitBullet()
	{
		return false;
	}

	/// <summary>
	/// この弾がキャラに衝突した時に呼び出される処理。
	/// </summary>
	/// <param name="chara">この弾に衝突されたキャラ</param>
	public virtual void OnHitCharacter( CharaControllerBase chara )
	{

	}

	/// <summary>
	/// この弾が他の弾に衝突した時に呼び出される処理。
	/// </summary>
	/// <param name="bullet">この弾に衝突された他の弾</param>
	public virtual void OnHitBullet( Bullet bullet )
	{

	}

	/// <summary>
	/// この弾が他の弾から衝突された時に呼び出される処理。
	/// OnHitBulletはこちら側から衝突した場合に対して、この処理は向こう側から衝突してきた場合に呼び出される。
	/// </summary>
	/// <param name="bullet">この弾に衝突してきた他の弾</param>
	/// <param name="colliderData">衝突を検出したこの弾の衝突情報</param>
	public virtual void OnSuffer( Bullet bullet, CollisionManager.ColliderData colliderData )
	{

	}
}
