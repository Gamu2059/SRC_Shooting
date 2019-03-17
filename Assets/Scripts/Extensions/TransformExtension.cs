using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
	public static Vector3 GetMoveObjectHolderBasePosition( this Transform transform )
	{
		return transform.position.ToMoveObjectHolderBasePosition();
	}

	public static Vector3 GetMoveObjectHolderBaseEulerAngles( this Transform transform )
	{
		return transform.eulerAngles.ToMoveObjectHolderBaseEulerAngles();
	}
}
