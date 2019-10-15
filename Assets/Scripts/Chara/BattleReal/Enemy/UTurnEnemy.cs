using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ある円周を軌道として動く敵のコントローラ。
/// </summary>
public class UTurnEnemy : BattleRealEnemyController
{
	protected enum E_PHASE
	{
		BEGIN_STRAIGHT,
		CIRCLE_MOVE,
		END_STRAIGHT,
	}

    protected const string VISIBLE_SHOT_TIMER_KEY = "VisibleShotTimer";

	// 初期出現地点に対して、相対座標で直進終了地点を定める
	protected Vector3 m_RelativeStraightMoveEndPosition;

	// 直進時の移動速度
	protected float m_StraightMoveSpeed;

	// 直進終点の相対座標で円の中心点を定める
	protected Vector3 m_RelativeCircleCenterPosition;

	// 直進から切り替わった時に円周上をどのくらいの角度移動するか(Degree)
	protected float m_CircleMoveAngle;

	// 円周移動時の移動角速度
	protected float m_CircleMoveSpeed;

	protected float m_VisibleOffsetShotTime;
	protected EnemyShotParam m_StraightMoveShotParam;
	protected EnemyShotParam m_CircleMoveShotParam;

	// 処理分けを行うためのデータ
	protected E_PHASE m_Phase;

	// 直進時の実際に行きつく先の終了座標
	protected Vector3 m_FactStraightMoveEndPosition;

	// 直進時の進行方向
	protected Vector3 m_StraightMoveDirection;

	// 円の中心点の実際の座標
	protected Vector3 m_FactCircleCenterPosition;

	// 円の半径
	protected float m_CircleRadius;

	// 円周移動開始時の角度
	protected float m_BeginCircleMoveAngle;

	// 現在の移動角度
	protected float m_NowCircleMoveAngle;

	// 弾発射間隔のカウンター
	protected float m_ShotTimeCount;

    protected Vector3 m_ShotPosition;

    protected override void OnSetParamSet()
    {
        base.OnSetParamSet();

        if (BehaviorParamSet is BattleRealEnemyUturnParamSet paramSet)
        {
            m_RelativeStraightMoveEndPosition = paramSet.RelativeStraightMoveEndPosition;
            m_StraightMoveSpeed = paramSet.StraightMoveSpeed;
            m_RelativeCircleCenterPosition = paramSet.RelativeCircleCenterPosition;
            m_CircleMoveAngle = paramSet.CircleMoveAngle;
            m_CircleMoveSpeed = paramSet.CircleMoveSpeed;

            m_StraightMoveShotParam = paramSet.StraightMoveShotParam;
            m_CircleMoveShotParam = paramSet.CircleMoveShotParam;

            m_VisibleOffsetShotTime = paramSet.VisibleOffsetShotTime;

            m_ShotPosition = paramSet.ShotPosition;
        } else
        {
            Debug.LogError("BehaviorParamSetが不適切です。");
        }
    }

    public override void OnStart()
	{
		base.OnStart();
		m_Phase = E_PHASE.BEGIN_STRAIGHT;

		Vector3 startPos = transform.localPosition;

		// 実際の直進の終了座標を求める
		m_FactStraightMoveEndPosition = m_RelativeStraightMoveEndPosition + startPos;
		// 初期地点から終了地点に対する相対座標をそのまま正規化して方向ベクトルにする
		m_StraightMoveDirection = m_RelativeStraightMoveEndPosition.normalized;

		// 実際の円の中心座標を求める
		m_FactCircleCenterPosition = m_FactStraightMoveEndPosition + m_RelativeCircleCenterPosition;

		// 円の中心座標から初期角度を求める
		m_BeginCircleMoveAngle = Mathf.Atan2( m_RelativeCircleCenterPosition.z, m_RelativeCircleCenterPosition.x ) * Mathf.Rad2Deg + 180;
		m_NowCircleMoveAngle = m_BeginCircleMoveAngle;
		// 円の半径を求める
		m_CircleRadius = m_RelativeCircleCenterPosition.magnitude;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		Move();
        Shot();
    }

    public override void OnLateUpdate()
    {
        base.OnLateUpdate();

        var isOutOfField = BattleRealStageManager.Instance.IsOutOfField(transform);

        if (!isOutOfField)
        {
            var timer = Timer.CreateTimeoutTimer(E_TIMER_TYPE.SCALED_TIMER, m_VisibleOffsetShotTime, () =>
            {
                OnShot(m_StraightMoveShotParam);
            });
            RegistTimer(VISIBLE_SHOT_TIMER_KEY, timer);
        }
    }

    protected virtual void Move()
	{
		float deltaTime = Time.deltaTime;

		switch( m_Phase )
		{
			case E_PHASE.BEGIN_STRAIGHT:
				Vector3 beforeDir = m_FactStraightMoveEndPosition - transform.localPosition;
				Vector3 beginDeltaMove = m_StraightMoveDirection * m_StraightMoveSpeed * deltaTime;
				transform.localPosition += beginDeltaMove;

				if( Vector2.Dot( beforeDir.ToVector2XZ(), beginDeltaMove.ToVector2XZ() ) <= 0 )
				{
					m_Phase = E_PHASE.CIRCLE_MOVE;
					transform.localPosition = m_FactStraightMoveEndPosition;
				}

				break;

			case E_PHASE.CIRCLE_MOVE:
				m_NowCircleMoveAngle += m_CircleMoveSpeed * deltaTime;
				SetPositionOnCircle( m_NowCircleMoveAngle );

				if( Mathf.Abs( m_NowCircleMoveAngle - m_BeginCircleMoveAngle ) >= m_CircleMoveAngle )
				{
					m_Phase = E_PHASE.END_STRAIGHT;
					float endAngle = m_BeginCircleMoveAngle + Mathf.Sign( m_CircleMoveSpeed ) * m_CircleMoveAngle;
					SetPositionOnCircle( endAngle );
				}

				break;

			case E_PHASE.END_STRAIGHT:
				Vector3 endDeltaMove = -m_StraightMoveDirection * m_StraightMoveSpeed * deltaTime;
				transform.localPosition += endDeltaMove;
				break;
		}

        var p = BattleRealPlayerManager.Instance.Player.transform.position;
        p.y = transform.position.y;
        transform.LookAt(p);
    }

	protected virtual void SetPositionOnCircle( float angle )
	{
		float radAngle = angle * Mathf.Deg2Rad;
		var pos = new Vector3( Mathf.Cos( radAngle ), 0f, Mathf.Sin( radAngle ) ) * m_CircleRadius + m_FactCircleCenterPosition;
		transform.localPosition = pos;
	}

	protected virtual void Shot()
	{
		m_ShotTimeCount += Time.deltaTime;

		if( m_Phase == E_PHASE.BEGIN_STRAIGHT )
		{
			if( m_ShotTimeCount >= m_StraightMoveShotParam.Interval )
			{
				m_ShotTimeCount = 0;
				OnShot( m_StraightMoveShotParam );
			}
		}
		else if( m_Phase == E_PHASE.CIRCLE_MOVE )
		{
			if( m_ShotTimeCount >= m_CircleMoveShotParam.Interval )
			{
				m_ShotTimeCount = 0;
				OnShot( m_CircleMoveShotParam );
			}
		}
	}

	protected virtual void OnShot( EnemyShotParam param )
	{
		int num = param.Num;
		float angle = param.Angle;
		var spreadAngles = GetBulletSpreadAngles( num, angle );
		var shotParam = new BulletShotParam( this );
        shotParam.Position = m_ShotPosition + transform.position;

        for ( int i = 0; i < num; i++ )
		{
			var bullet = BulletController.ShotBullet( shotParam );
			bullet.SetRotation( new Vector3( 0, spreadAngles[i], 0 ), E_RELATIVE.RELATIVE );
		}
	}
}
