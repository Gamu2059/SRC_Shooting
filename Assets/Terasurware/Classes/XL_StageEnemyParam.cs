using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XL_StageEnemyParam : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public float Time;
		public int EnemyViewId;
		public int EnemyMoveId;
		public int BulletSetId;
		public float AppearViewportX;
		public float AppearViewportY;
		public float AppearOffsetX;
		public float AppearOffsetZ;
		public float AppearOffsetY;
		public float AppearRotateY;
		public int IsBoss;
		public string OtherParameters;
	}
}