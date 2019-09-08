using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class XL_StageEnemyParam : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public string Conditions;
		public int EnemyId;
		public int BulletSetId;
		public float AppearViewportX;
		public float AppearViewportY;
		public float AppearOffsetX;
		public float AppearOffsetZ;
		public float AppearOffsetY;
		public float AppearRotateY;
		public int IsBoss;
		public float Hp;
		public string Drop;
		public string Defeat;
		public string OtherParameters;
	}
}