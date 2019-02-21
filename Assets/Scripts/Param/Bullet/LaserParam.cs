using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Laserに渡すパラメータ。
/// </summary>
[System.Serializable, CreateAssetMenu( menuName = "Param/LaserParam", fileName = "LaserParam" )]
public class LaserParam : ScriptableObject
{
	[Tooltip( "レーザーの射出演出時間 これを超えると継続時間に移行する" )]
	public float BeginTime;

	[Tooltip( "レーザーの継続時間 これを超えると消滅時間に移行する" )]
	public float LifeTime;

	[Tooltip( "レーザーの消滅演出時間 これを超えると自動消滅する" )]
	public float EndTime;

	public LaserOrbitalParam BeginOrbitalParam;
	public LaserOrbitalParam OrbitalParam;
	public LaserOrbitalParam EndOrbitalParam;

	public LaserOrbitalParam[] ConditionalOrbitalParams;

	[Tooltip( "オプションのGameObjectパラメータ" )]
	public OptionObjectParam[] OptionObjectParams;

	[Tooltip( "オプションのfloatパラメータ" )]
	public OptionValueParam[] OptionValueParams;
}
