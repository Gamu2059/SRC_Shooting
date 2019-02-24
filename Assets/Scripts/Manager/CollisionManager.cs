using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : GlobalSingletonMonoBehavior<CollisionManager>
{
	public enum E_COLLIDER_SHAPE
	{
		// 矩形
		RECT,

		// 楕円
		ELLIPSE
	}

	[System.Serializable]
	public struct ColliderTransform
	{
		public E_COLLIDER_SHAPE ColliderType;

		public Transform Transform;
	}

	[System.Serializable]
	public struct ColliderData
	{
		/// <summary>
		/// 矩形か楕円か
		/// </summary>
		public E_COLLIDER_SHAPE ColliderType;

		/// <summary>
		/// 中心座標
		/// </summary>
		public Vector2 CenterPos;

		/// <summary>
		/// サイズ
		/// </summary>
		public Vector2 Size;

		/// <summary>
		/// 回転(度数法)
		/// </summary>
		public float Angle;
	}

	protected override void OnAwake()
	{
		base.OnAwake();
	}

	private void Start()
	{
		OnStart();
	}

	private void Update()
	{
		OnUpdate();
	}

	private void LateUpdate()
	{
		OnLateUpdate();
	}

	public override void OnInit()
	{
	}

	public override void OnStart()
	{

	}

	public override void OnUpdate()
	{
	}

	/// <summary>
	/// 当たり判定を司るのでLateUpdateで判定を行う。
	/// </summary>
	public override void OnLateUpdate()
	{
		List<Bullet> bullets = BulletManager.Instance.GetBullets();

		foreach( var bullet in bullets )
		{
			ColliderData[] colliderDatas = bullet.GetColliderData();

			// 弾同士の当たり判定処理
			if( bullet.CanHitBullet() )
			{
				foreach( var targetBullet in bullets )
				{
					if( bullet == targetBullet )
					{
						continue;
					}

					BulletCollide( bullet, targetBullet, colliderDatas );
				}
			}

			// 弾とキャラの当たり判定処理
			if( bullet.GetTroop() == CharaControllerBase.E_CHARA_TROOP.ENEMY )
			{
				var targetChara = PlayerCharaManager.Instance.GetCurrentController();

				CharaCollide( bullet, targetChara, colliderDatas );
			}
			else
			{
				var charaList = EnemyCharaManager.Instance.GetControllers();

				foreach( var targetChara in charaList )
				{
					CharaCollide( bullet, targetChara, colliderDatas );
				}
			}
		}
	}

	private void BulletCollide( Bullet bullet, Bullet target, ColliderData[] bulletDatas )
	{
		ColliderData[] targetDatas = target.GetColliderData();

		foreach( var baseData in bulletDatas )
		{
			foreach( var targetData in targetDatas )
			{
				bool isHit = IsCollide( baseData, targetData );

				if( isHit )
				{
					target.OnSuffer( bullet, targetData );
					bullet.OnHitBullet( target );
				}
			}
		}
	}

	private void CharaCollide( Bullet bullet, CharaControllerBase target, ColliderData[] bulletDatas )
	{
		ColliderData[] targetDatas = target.GetColliderData();

		foreach( var baseData in bulletDatas )
		{
			foreach( var targetData in targetDatas )
			{
				bool isHit = IsCollide( baseData, targetData );

				if( isHit )
				{
					target.OnSuffer( bullet, targetData );
					bullet.OnHitCharacter( target );
				}
			}
		}
	}

	/// <summary>
	/// 二つの衝突情報が、互いに衝突しているかを判定する。
	/// </summary>
	/// <param name="collider1"></param>
	/// <param name="collider2"></param>
	/// <returns></returns>
	public bool IsCollide( ColliderData collider1, ColliderData collider2 )
	{
		if( collider1.ColliderType == E_COLLIDER_SHAPE.RECT && collider2.ColliderType == E_COLLIDER_SHAPE.RECT )
		{
			return IsCollideRectAndRect( collider1, collider2 );
		}
		else if( collider1.ColliderType == E_COLLIDER_SHAPE.ELLIPSE && collider2.ColliderType == E_COLLIDER_SHAPE.ELLIPSE )
		{
			return IsCollideEllipseAndEllipse( collider1, collider2 );
		}
		else if( collider1.ColliderType == E_COLLIDER_SHAPE.RECT && collider2.ColliderType == E_COLLIDER_SHAPE.ELLIPSE )
		{
			return IsCollideRectAndEllipse( collider1, collider2 );
		}
		else
		{
			return IsCollideRectAndEllipse( collider2, collider1 );
		}
	}

	private bool IsCollideRectAndRect( ColliderData rect1, ColliderData rect2 )
	{
		Vector2[] corners1 = GetCornerPosFromRect( rect1 );
		Vector2[] corners2 = GetCornerPosFromRect( rect2 );

		if( IsCollideRectAndRect( corners1, corners2 ) )
		{
			return true;
		}

		return IsCollideRectAndRect( corners2, corners1 );

	}

	private bool IsCollideRectAndRect( Vector2[] corners1, Vector2[] corners2 )
	{
		for( int j = 0; j < corners2.Length; j++ )
		{
			bool flag = true;

			for( int i = 0; i < corners1.Length; i++ )
			{
				Vector2 baseV = corners1[( i + 1 ) % corners1.Length] - corners1[i];
				Vector2 targetV = corners2[j] - corners1[i];

				if( baseV.x * targetV.y - targetV.x * baseV.y > 0 )
				{
					flag = false;
					break;
				}
			}

			if( flag )
			{
				return true;
			}
		}

		return false;
	}

	private bool IsCollideRectAndEllipse( ColliderData rect, ColliderData ellipse )
	{
		Vector2[] corners = GetCornerPosFromRect( rect );
		float cos = Mathf.Cos( ellipse.Angle * Mathf.Deg2Rad );
		float sin = Mathf.Sin( ellipse.Angle * Mathf.Deg2Rad );
		float scaleRate = ellipse.Size.x / ellipse.Size.y;

		//Debug.Log( "Corner" );
		//Debug.Log( "CenterPos:" + rect.CenterPos + ", Size:" + rect.Size + ", Angle:" + rect.Angle );

		//for( int i = 0; i < corners.Length; i++ )
		//{
		//	Debug.Log( corners[i] );
		//}

		//Debug.Log( "Ellipse" );
		//Debug.Log( "CenterPos:" + ellipse.CenterPos + ", Size:" + ellipse.Size + ", Angle:" + ellipse.Angle );

		for( int i = 0; i < corners.Length; i++ )
		{
			Vector2 offset = corners[i] - ellipse.CenterPos;
			float x = offset.x * cos + offset.y * sin;
			float y = scaleRate * ( -offset.x * sin + offset.y * cos );

			//Debug.Log( "sin" + sin + " cos" + cos );
			//Debug.Log( "x " + x + " y " + y );
			//Debug.Log( "sqrDist : " + x * x + y * y );
			//Debug.Log( "ellipse sqrSize : " + ellipse.Size.x * ellipse.Size.x );

			if( x * x + y * y <= ellipse.Size.x * ellipse.Size.x )
			{
				//Debug.LogError( "Hit!" );
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 楕円と楕円の衝突判定。参考URL : http://marupeke296.com/COL_2D_No7_EllipseVsEllipse.html
	/// </summary>
	private bool IsCollideEllipseAndEllipse( ColliderData ellipse1, ColliderData ellipse2 )
	{
		float deffAngle = ( ellipse1.Angle - ellipse2.Angle ) * Mathf.Deg2Rad;
		Vector2 deltaPos = ellipse2.CenterPos - ellipse1.CenterPos;
		float cos1 = Mathf.Cos( ellipse1.Angle * Mathf.Deg2Rad );
		float sin1 = Mathf.Sin( ellipse1.Angle * Mathf.Deg2Rad );
		float deffCos = Mathf.Cos( deffAngle );
		float deffSin = Mathf.Sin( deffAngle );
		float nx = ellipse2.Size.x * deffCos;
		float ny = -ellipse2.Size.x * deffSin;
		float px = ellipse2.Size.y * deffSin;
		float py = ellipse2.Size.y * deffCos;
		float ox = cos1 * deltaPos.x + sin1 * deltaPos.y;
		float oy = -sin1 * deltaPos.x + cos1 * deltaPos.y;

		float rx_pow2 = 1f / ( ellipse1.Size.x * ellipse1.Size.x );
		float ry_pow2 = 1f / ( ellipse1.Size.y * ellipse1.Size.y );
		float A = rx_pow2 * nx * nx + ry_pow2 * ny * ny;
		float B = rx_pow2 * px * px + ry_pow2 * py * py;
		float D = 2 * rx_pow2 * nx * px + 2 * ry_pow2 * ny * py;
		float E = 2 * rx_pow2 * nx * ox + 2 * ry_pow2 * ny * oy;
		float F = 2 * rx_pow2 * px * ox + 2 * ry_pow2 * py * oy;
		float G = ( ox / ellipse1.Size.x ) * ( ox / ellipse1.Size.x ) + ( oy / ellipse1.Size.y ) * ( oy / ellipse1.Size.y ) - 1;

		float tmp1 = 1f / ( D * D - 4 * A * B );
		float h = ( F * D - 2 * E * B ) * tmp1;
		float k = ( E * D - 2 * A * F ) * tmp1;
		float Th = Mathf.Atan2( D, B - A ) * 0.5f;

		float CosTh = Mathf.Cos( Th );
		float SinTh = Mathf.Sin( Th );
		float A_tt = A * CosTh * CosTh + B * SinTh * SinTh - D * CosTh * SinTh;
		float B_tt = A * SinTh * SinTh + B * CosTh * CosTh + D * CosTh * SinTh;
		float KK = A * h * h + B * k * k + D * h * k - E * h - F * k + G;

		// 念のため
		if( KK > 0 )
		{
			KK = 0;
		}

		float Rx_tt = 1 + Mathf.Sqrt( -KK / A_tt );
		float Ry_tt = 1 + Mathf.Sqrt( -KK / B_tt );
		float x_tt = CosTh * h - SinTh * k;
		float y_tt = SinTh * h + CosTh * k;
		float JudgeValue = x_tt * x_tt / ( Rx_tt * Rx_tt ) + y_tt * y_tt / ( Ry_tt * Ry_tt );

		if( JudgeValue <= 1 )
		{
			return true;
		}

		return false;
	}

	public Vector2[] GetCornerPosFromRect( ColliderData colliderData )
	{
		Vector2 halfSize = colliderData.Size / 2f;
		Vector2[] cornerPos = new Vector2[4];
		cornerPos[0] = new Vector2( halfSize.x, halfSize.y );
		cornerPos[1] = new Vector2( halfSize.x, -halfSize.y );
		cornerPos[2] = new Vector2( -halfSize.x, -halfSize.y );
		cornerPos[3] = new Vector2( -halfSize.x, halfSize.y );

		float cos = Mathf.Cos( colliderData.Angle * Mathf.Deg2Rad );
		float sin = Mathf.Sin( colliderData.Angle * Mathf.Deg2Rad );

		for( int i = 0; i < cornerPos.Length; i++ )
		{
			float x = cornerPos[i].x;
			float y = cornerPos[i].y;
			cornerPos[i].x = x * cos - y * sin;
			cornerPos[i].y = x * sin + y * cos;

			cornerPos[i] += colliderData.CenterPos;
		}

		return cornerPos;
	}
}
