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


	/// <summary>
	/// マスターデータから取得するパラメータセット
	/// </summary>
	protected StringParamSet m_ParamSet;

	/// <summary>
	/// 敵処理で用いるタイマーを保持するリスト
	/// このリストにタイマーを登録しないと潜在的な例外発生を招く場合があります
	/// </summary>
	protected Dictionary<string, Timer> m_TimerDict;

	private bool m_CanOutDestroy;



	#region Getter & Setter

	public StringParamSet GetParamSet()
	{
		return m_ParamSet;
	}

	#endregion



	private void Start()
	{
		// 開発時専用で、自動的にマネージャにキャラを追加するためにUnityのStartを用いています
		EnemyCharaManager.Instance.RegistEnemy( this );
	}



	public override void OnInitialize()
	{
		base.OnInitialize();

		m_TimerDict = new Dictionary<string, Timer>();
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
			return;
		}

		Timer timer = null;

		if( m_TimerDict.ContainsKey( "HitInvincibleTimer" ) )
		{
			timer = m_TimerDict["HitInvincibleTimer"];
		}

		if( timer == null )
		{
			timer = Timer.CreateTimeoutTimer( E_TIMER_TYPE.SCALED_TIMER, m_OnHitInvincibleDuration, () =>
			{
				timer = null;
			} );
			BattleMainTimerManager.Instance.RegistTimer( timer );

			m_TimerDict.Add( "HitInvincibleTimer", timer );

			base.OnSuffer( bullet, colliderData );
		}
	}

	public override void Dead()
	{
		base.Dead();

		DestroyAllTimer();
		EnemyCharaManager.Instance.DestroyEnemy( this );
	}

	/// <summary>
	/// この弾にタイマーを登録する。
	/// </summary>
	/// <param name="key">タイマーのキー</param>
	/// <param name="timer">タイマー</param>
	public void RegistTimer( string key, Timer timer )
	{
		if( m_TimerDict == null || m_TimerDict.ContainsKey( key ) )
		{
			return;
		}

		m_TimerDict.Add( key, timer );
		BattleMainTimerManager.Instance.RegistTimer( timer );
	}

	/// <summary>
	/// 指定したキーに対するタイマーを完全破棄する。
	/// </summary>
	/// <param name="key">タイマーのキー</param>
	public void DestroyTimer( string key )
	{
		if( m_TimerDict == null || !m_TimerDict.ContainsKey( key ) )
		{
			return;
		}

		var timer = m_TimerDict[key];
		m_TimerDict.Remove( key );

		if( timer != null )
		{
			timer.DestroyTimer();
		}
	}

	/// <summary>
	/// この弾に紐づけられている全てのタイマーを完全破棄する。
	/// </summary>
	public void DestroyAllTimer()
	{
		if( m_TimerDict == null )
		{
			return;
		}

		foreach( var timer in m_TimerDict.Values )
		{
			timer.DestroyTimer();
		}

		m_TimerDict.Clear();
	}
}
