using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vector2を拡張するクラス。
/// </summary>
public static class Vector2Extension
{
	public static Vector3 ToVector3XZ( this Vector2 v )
	{
		return new Vector3( v.x, 0, v.y );
	}
}
