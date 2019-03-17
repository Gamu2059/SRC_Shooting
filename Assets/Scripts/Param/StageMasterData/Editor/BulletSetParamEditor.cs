#if UNITY_EDITOR

using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 参考 : Unity ReorderableList
/// </summary>
[CustomEditor( typeof( BulletSetParam ) )]
public class BulletSetParamEditor : Editor
{
	private List<ReorderableList> m_ListFields;
	private bool m_IsShowingSort;

	private void OnEnable()
	{
		m_ListFields = InitSortableList( target.GetType(), serializedObject );
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField( "Expansion" );

		m_IsShowingSort = EditorGUILayout.Toggle( "Is Showing Sorting List", m_IsShowingSort );

		if( m_IsShowingSort )
		{
			foreach( var l in m_ListFields )
			{
				l.DoLayoutList();
			}
		}
	}

	/// <summary>
	/// 指定したクラスのフィールドからリストや配列を検出してReorderableListのリストを取得する。
	/// </summary>
	private static List<ReorderableList> InitSortableList( Type type, SerializedObject serializedObject )
	{
		var fields = type.GetFields( BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );

		var listFields = new List<ReorderableList>();

		foreach( var f in fields )
		{
			var prop = serializedObject.FindProperty( f.Name );

			Type fType = f.FieldType;
			bool isCollection = false;

			if( fType.IsArray )
			{
				isCollection = true;
			}
			else if( fType.IsGenericType && typeof( List<> ).IsAssignableFrom( fType.GetGenericTypeDefinition() ) )
			{
				isCollection = true;
			}

			if( isCollection )
			{
				listFields.Add( new ReorderableList( serializedObject, prop ) );
			}
		}

		foreach( var l in listFields )
		{
			SetupCallback( l );
		}

		return listFields;
	}

	/// <summary>
	/// ReorderableListの初期化。
	/// </summary>
	private static void SetupCallback( ReorderableList list )
	{
		if( list == null )
		{
			return;
		}

		var prop = list.serializedProperty;

		list.drawElementCallback = ( rect, idx, isActive, isFocused ) =>
		{
			var element = prop.GetArrayElementAtIndex( idx );
			EditorGUI.PropertyField( rect, element );
		};
		list.drawHeaderCallback = ( rect ) =>
		{
			EditorGUI.LabelField( rect, prop.displayName );
		};
		// フッターは操作できないようにする
		list.drawFooterCallback = rect => {;};
	}
}

#endif