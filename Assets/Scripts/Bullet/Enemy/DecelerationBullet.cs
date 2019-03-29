using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 一定まで減速する弾コントローラ。
/// </summary>
public class DecelerationBullet : BulletController
{
	private const string DT_KEY = "Deceleration Threshold";

	protected float m_DecelerationThreshold;

	public override void ChangeOrbital( BulletOrbitalParam orbitalParam )
	{
		base.ChangeOrbital( orbitalParam );

		// オプション
		OptionValueParam param = orbitalParam.OptionValueParams.ToList().Find( p => p.Key == DT_KEY );

		if( param.Key != DT_KEY )
		{
			Debug.LogErrorFormat( "OptionParam {0} が存在しません。", DT_KEY );
			return;
		}

		m_DecelerationThreshold = param.Value;
	}

	public override void OnUpdate()
	{
		if( GetBulletParam() == null )
		{
			return;
		}

		SetRotation( GetNowDeltaRotation() * Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE );
		SetScale( GetNowDeltaScale() * Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE );

		SetNowSpeed( GetNowAccel() * Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE );

		if( GetNowSpeed() < m_DecelerationThreshold )
		{
			SetNowSpeed( m_DecelerationThreshold );
		}

		if( GetTarget() != null )
		{
			Vector3 targetDeltaPos = GetTarget().transform.position - transform.position;
			transform.forward = Vector3.Lerp( transform.forward, targetDeltaPos.normalized, GetNowLerp() );
		}

		var speed = GetNowSpeed() * Time.deltaTime;
		SetPosition( transform.forward * speed, E_ATTACK_PARAM_RELATIVE.RELATIVE );

		SetNowLifeTime( Time.deltaTime, E_ATTACK_PARAM_RELATIVE.RELATIVE );

		if( GetNowLifeTime() > GetBulletParam().LifeTime )
		{
			DestroyBullet();
		}
	}
}
