using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BulletSpreadParam
{
	[Tooltip( "拡散時の弾数" )]
	public int BulletNum;

	[Tooltip( "拡散時の半径" )]
	public float Radius;

	[Tooltip( "拡散時の弾間の角度" )]
	public float DeltaAngle;
}
