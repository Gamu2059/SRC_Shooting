using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaserOrbitalParam
{
	[Header( "Anchor" )]

	[Tooltip( "矩形のどこを基準にトランスフォームを適用するか" )]
	public Vector3 Anchor;

	[Tooltip( "Anchorの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE AnchorRelative;

	[Header( "Transform" )]

	[Tooltip( "これが適用された時のAnchoredPosition" )]
	public Vector3 AnchoredPosition;

	[Tooltip( "AnchoredPositionの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE AnchoredPositionRelative;

	[Tooltip( "これが適用された時のRotation" )]
	public Vector3 Rotation;

	[Tooltip( "Rotationの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE RotationRelative;

	[Tooltip( "これが適用された時のScale" )]
	public Vector3 Scale;

	[Tooltip( "Scaleの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE ScaleRelative;

	[Tooltip( "これが適用された時のDeltaRotation" )]
	public Vector3 DeltaRotation;

	[Tooltip( "DeltaRotationの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE DeltaRotationRelative;

	[Tooltip( "これが適用された時のDeltaScale" )]
	public Vector3 DeltaScale;

	[Tooltip( "DeltaScaleの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE DeltaScaleRelative;

	[Space()]
	[Header( "Hit" )]

	[Tooltip( "これが適用された時のHitSize" )]
	public Vector2 HitSize;

	[Tooltip( "HitSizeの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE HitSizeRelative;

	[Tooltip( "これが適用された時のDeltaHitSize" )]
	public Vector2 DeltaHitSize;

	[Tooltip( "DeltaHitSizeの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE DeltaHitSizeRelative;

	[Tooltip( "これが適用された時のDamage" )]
	public float Damage;

	[Tooltip( "Damageの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE DamageRelative;
	[Space()]
	[Header( "Sticky" )]

	[Tooltip( "これが適用された時のSearch" )]
	public bool IsSearch;

	[Tooltip( "これが適用された時のLerp" )]
	public float Lerp;

	[Tooltip( "Lerpの適用は絶対か相対か" )]
	public E_ATTACK_PARAM_RELATIVE LerpRelative;


	[Space()]
	[Header( "Option" )]

	[Tooltip( "オプションのGameObjectパラメータ" )]
	public OptionObjectParam[] OptionObjectParams;

	[Tooltip( "オプションのfloatパラメータ" )]
	public OptionValueParam[] OptionValueParams;

}
