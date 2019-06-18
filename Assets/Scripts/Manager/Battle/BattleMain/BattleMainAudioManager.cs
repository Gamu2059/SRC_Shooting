using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class BattleMainAudioManager : AudioManager<BattleMainAudioManager>
{
#if UNITY_EDITOR
	[CustomEditor( typeof( BattleMainAudioManager ) )]
	public class BattleMainAudioManagerEditor : AudioManagerEditor { }
#endif
}
