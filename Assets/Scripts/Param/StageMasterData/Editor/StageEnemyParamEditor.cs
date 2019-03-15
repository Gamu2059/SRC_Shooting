using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 参考 : Unity ReorderableList
/// </summary>
[CustomEditor( typeof( StageEnemyParam ) )]
public class StageEnemyParamEditor : Editor
{
	private List<ReorderableList> m_ListFields;
	private bool m_IsShowingSort;

	private void OnEnable()
	{
		m_ListFields = SortableListEditorBase.InitSortableList( target.GetType(), serializedObject );
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
}