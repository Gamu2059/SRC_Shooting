using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bulletに渡すパラメータ。
/// </summary>
[System.Serializable, CreateAssetMenu( menuName = "Param/BulletParam", fileName = "BulletParam" )]
public class BulletParam : ScriptableObject
{
	[Tooltip( "弾の継続時間 これを超えると自動消滅する" )]
	public float LifeTime;

	[Tooltip( "発射時の最初のパラメータ" )]
	public BulletOrbitalParam OrbitalParam;

	[Tooltip( "特殊な条件を満たす時のパラメータ" )]
	public BulletOrbitalParam[] ConditionalOrbitalParams;

	[Tooltip( "オプションのGameObjectパラメータ" )]
	public OptionObjectParam[] OptionObjectParams;

	[Tooltip( "オプションのfloatパラメータ" )]
	public OptionValueParam[] OptionValueParams;
}
