using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletOrbitalParam
{

	[Tooltip( "この弾の標的となるもの" )]
	public BulletParam.E_BULLET_TARGET Target;

	[Space()]
	[Header( "Transform" )]

	[Tooltip( "これが適用された時のPosition" )]
	public Vector3 Position;

	[Tooltip( "Positionの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE PositionRelative;

	[Tooltip( "これが適用された時のRotation" )]
	public Vector3 Rotation;

	[Tooltip( "Rotationの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE RotationRelative;

	[Tooltip( "これが適用された時のScale" )]
	public Vector3 Scale;

	[Tooltip( "Scaleの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE ScaleRelative;

	[Tooltip( "これが適用された時のDeltaRotation" )]
	public Vector3 DeltaRotation;

	[Tooltip( "DeltaRotationの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE DeltaRotationRelative;

	[Tooltip( "これが適用された時のDeltaScale" )]
	public Vector3 DeltaScale;

	[Tooltip( "DeltaScaleの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE DeltaScaleRelative;

	[Space()]
	[Header( "Spread" )]

	[Tooltip( "これが適用された時の拡散情報" )]
	public BulletSpreadParam SpreadParam;


	[Space()]
	[Header( "Hit" )]

	[Tooltip( "これが適用された時のHitSize" )]
	public float HitSize;

	[Tooltip( "HitSizeの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE HitSizeRelative;

	[Tooltip( "これが適用された時のDeltaHitSize" )]
	public float DeltaHitSize;

	[Tooltip( "DeltaHitSizeの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE DeltaHitSizeRelative;

	[Tooltip( "これが適用された時のDamage" )]
	public float Damage;

	[Tooltip( "Damageの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE DamageRelative;


	[Space()]
	[Header( "Speed" )]

	[Tooltip( "これが適用された時のSpeed" )]
	public float Speed;

	[Tooltip( "Speedの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE SpeedRelative;

	[Tooltip( "これが適用された時のAccel" )]
	public float Accel;

	[Tooltip( "Accelの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE AccelRelative;


	[Space()]
	[Header( "Sticky" )]

	[Tooltip( "これが適用された時のSearch" )]
	public bool IsSearch;

	[Tooltip( "これが適用された時のLerp" )]
	public float Lerp;

	[Tooltip( "Lerpの適用は絶対か相対か" )]
	public BulletParam.E_BULLET_PARAM_RELATIVE LerpRelative;


	[Space()]
	[Header( "Option" )]

	[Tooltip( "オプションのGameObjectパラメータ" )]
	public OptionObjectParam[] OptionObjectParams;

	[Tooltip( "オプションのfloatパラメータ" )]
	public OptionValueParam[] OptionValueParams;
}
