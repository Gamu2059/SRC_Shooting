using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
	public static Vector3 ToMoveObjectHolderBasePosition( this Vector3 position )
	{
		return StageManager.Instance.GetMoveObjectHolderBasePosition( position );
	}

	public static Vector3 ToMoveObjectHolderBaseEulerAngles( this Vector3 eulerAngles )
	{
		return StageManager.Instance.GetMoveObjectHolderBaseEulerAngles( eulerAngles );
	}
}
