using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletOrbitalParam
{

	[Tooltip( "この弾の標的となるもの" )]
	public E_ATTACK_TARGET Target;

	[Space()]
	[Header( "Transform" )]

	[Tooltip( "これが適用された時のPosition" )]
	public Vector3 Position;

	[Tooltip( "Positionの適用は絶対か相対か" )]
	public E_RELATIVE PositionRelative;

	[Tooltip( "これが適用された時のRotation" )]
	public Vector3 Rotation;

	[Tooltip( "Rotationの適用は絶対か相対か" )]
	public E_RELATIVE RotationRelative;

	[Tooltip( "これが適用された時のScale" )]
	public Vector3 Scale;

	[Tooltip( "Scaleの適用は絶対か相対か" )]
	public E_RELATIVE ScaleRelative;

	[Tooltip( "これが適用された時のDeltaRotation" )]
	public Vector3 DeltaRotation;

	[Tooltip( "DeltaRotationの適用は絶対か相対か" )]
	public E_RELATIVE DeltaRotationRelative;

	[Tooltip( "これが適用された時のDeltaScale" )]
	public Vector3 DeltaScale;

	[Tooltip( "DeltaScaleの適用は絶対か相対か" )]
	public E_RELATIVE DeltaScaleRelative;


	[Space()]
	[Header( "Damage" )]

	[Tooltip( "これが適用された時のDamage" )]
	public float Damage;

	[Tooltip( "Damageの適用は絶対か相対か" )]
	public E_RELATIVE DamageRelative;


	[Space()]
	[Header( "Speed" )]

	[Tooltip( "これが適用された時のSpeed" )]
	public float Speed;

	[Tooltip( "Speedの適用は絶対か相対か" )]
	public E_RELATIVE SpeedRelative;

	[Tooltip( "これが適用された時のAccel" )]
	public float Accel;

	[Tooltip( "Accelの適用は絶対か相対か" )]
	public E_RELATIVE AccelRelative;


	[Space()]
	[Header( "Sticky" )]

	[Tooltip( "これが適用された時のSearch" )]
	public bool IsSearch;

	[Tooltip( "これが適用された時のLerp" )]
	public float Lerp;

	[Tooltip( "Lerpの適用は絶対か相対か" )]
	public E_RELATIVE LerpRelative;

	[Tooltip( "Lerpを有効にする角度 弾の直進方向と弾から対象への方向のなす角が、この角度以下ならばLerpを適用する" )]
	public float LerpRestrictAngle;
}
